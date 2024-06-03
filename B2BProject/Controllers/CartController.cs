using Antlr.Runtime.Misc;
using B2BProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2BProject.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult GetOrderCartItems()
        {
            int customerId = GetCustomerId();
            if (customerId == 0)
            {
                ViewBag.TotalPrice = "There are no products in your cart.";
            }
            else
            {
                decimal? totalPrice = CalculateTotalPrice(customerId);
                decimal shippingFee = 100;
                decimal grandTotal = totalPrice.HasValue ? totalPrice.Value + shippingFee : 0;
                decimal? discountedTotal = CalculateDiscountedTotal(customerId, 10); // Örnek bir indirim oranı belirttim (buraya gerçek indirim oranınızı ekleyin)
                ViewBag.TotalPrice = totalPrice;
                ViewBag.ShippingFee = shippingFee;
                ViewBag.GrandTotal = grandTotal;
                ViewBag.DiscountedTotal = discountedTotal;
                ViewBag.CouponSuccess = "Coupon applied successfully!";
                ViewBag.CouponError = "Invalid coupon code or coupon has expired.";

                using (var entities = new B2BDbEntities())
                {
                    var orderCartItems = entities.Cart
                        .Where(c => c.User_id == customerId)
                        .Include(c => c.Products)
                        .ToList();

                    return View(orderCartItems);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddToOrderCart(int productId, int amount, decimal price, string option)
        {
            int customerId = GetCustomerId();
            if (customerId != 0)
            {
                using (var entities = new B2BDbEntities())
                {
                    var existingCartItem = entities.Cart
                        .FirstOrDefault(c => c.Product_id == productId && c.User_id == customerId && c.Options == option);

                    if (existingCartItem == null)
                    {
                        var cart = new Cart
                        {
                            User_id = customerId,
                            Product_id = productId,
                            Price = price,
                            Quantity = amount,
                            Options = option,
                            Total = price * amount
                        };

                        entities.Cart.Add(cart);
                    }
                    else
                    {
                        existingCartItem.Quantity += amount;
                        existingCartItem.Total += amount * price;
                    }

                    entities.SaveChanges();
                    decimal? totalPrice = CalculateTotalPrice(customerId);
                    ViewBag.CustomerName = customerId;
                    ViewBag.TotalPrice = totalPrice;

                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Login", "Buyer");
        }

        [HttpPost]
        public ActionResult RemoveFromOrderCart(int cartId)
        {
            using (var entities = new B2BDbEntities())
            {
                var cartItem = entities.Cart.Find(cartId);

                if (cartItem != null)
                {
                    if (cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                        cartItem.Total -= cartItem.Price;
                    }
                    else
                    {
                        entities.Cart.Remove(cartItem);
                    }

                    entities.SaveChanges();
                }
            }

            return RedirectToAction("GetOrderCartItems");
        }

        public ActionResult TotalCount()
        {
            using (var entities = new B2BDbEntities())
            {
                if (User.Identity.IsAuthenticated)
                {
                    int customerId = GetCustomerId();
                    int count = entities.Cart.Count(c => c.User_id == customerId);

                    ViewBag.Count = count;
                }
            }
            return PartialView();
        }

        public ActionResult ApplyCoupon(string couponCode)
        {
            int customerId = GetCustomerId();

            if (customerId == 0)
            {
                // Müşteri kimliği bulunamadı, bu yüzden kupon uygulanamaz.
                ViewBag.CouponError = "Invalid operation: Please log in to apply a coupon.";
                return RedirectToAction("Login", "Buyer");
            }

            using (var entities = new B2BDbEntities())
            {
                // Kupon kodunu kontrol ederken aynı zamanda kuponun geçerli ve henüz süresi dolmamış olup olmadığını kontrol ediyoruz.
                var coupon = entities.Coupons
                    .Where(c => c.Coupon_Code == couponCode && c.IsActive && c.End_date >= DateTime.Now)
                    .FirstOrDefault();

                if (coupon != null)
                {
                    // Sepetteki ilgili ürünleri kontrol ediyoruz
                    var cartItems = entities.Cart
                        .Where(c => c.User_id == customerId)
                        .ToList();

                    var relevantCartItem = cartItems.FirstOrDefault(c => c.Product_id == coupon.Product_id);

                    if (relevantCartItem != null)
                    {
                        // Sepette ilgili ürün bulunduğunda kupon indirimi uygulanır

                        // Kuponun indirim oranını alıyoruz.
                        decimal discountRate = coupon.Discount;

                        // Sepetteki ilgili ürünün toplam tutarını alıyoruz.
                        decimal? totalPrice = relevantCartItem.Total;

                        // Toplam tutar varsa ve 0'dan büyükse, kupon indirimi hesaplanır.
                        if (totalPrice.HasValue && totalPrice.Value > 0)
                        {
                            decimal discount = discountRate / 100;
                            decimal? discountedAmount = totalPrice.Value * discount;

                            // İndirim miktarını TempData'ye kaydediyoruz.
                            TempData["DiscountedAmount"] = discountedAmount;

                            // Kupon başarıyla uygulandı.
                            ViewBag.CouponSuccess = "Coupon applied successfully!";
                        }
                        else
                        {
                            // Ürünün toplam tutarı 0 veya daha az olduğu için kupon uygulanamaz.
                            ViewBag.CouponError = "Invalid operation: The total price of the product is less than or equal to zero.";
                        }
                    }
                    else
                    {
                        // Sepette ilgili ürün bulunamadığı için kupon uygulanamaz.
                        ViewBag.CouponError = "Invalid operation: The product associated with the coupon is not in your cart.";
                    }
                }
                else
                {
                    // Kupon kodu geçerli değilse veya süresi dolmuşsa hata mesajı gösterilir.
                    ViewBag.CouponError = "Invalid coupon code or coupon has expired.";
                }
            }

            // Sepete geri yönlendirme yapılır.
            return RedirectToAction("GetOrderCartItems");
        }



        private decimal? CalculateTotalPrice(int customerId)
        {
            using (var entities = new B2BDbEntities())
            {
                var totalPrice = entities.Cart
                    .Where(c => c.User_id == customerId)
                    .Sum(c => (decimal?)c.Total);

                if (totalPrice.HasValue && totalPrice.Value > 0)
                {
                    return totalPrice.Value;
                }
                else
                {
                    return 0;
                }
            }
        }
        [HttpPost]
        public ActionResult ChangeAmount(int cartId, string changeAmount)
        {
            using (var entities = new B2BDbEntities())
            {
                var cartItem = entities.Cart.Find(cartId);

                if (cartItem != null)
                {
                    if (changeAmount == "increase")
                    {
                        cartItem.Quantity++;
                    }
                    else if (changeAmount == "decrease" && cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                    }

                    cartItem.Total = cartItem.Quantity * cartItem.Price;
                    entities.SaveChanges();

                    decimal? totalPrice = CalculateTotalPrice(cartItem.User_id);
                    ViewBag.TotalPrice = totalPrice;
                }

                return RedirectToAction("GetOrderCartItems");
            }
        }

        [HttpPost]
        public ActionResult UpdateOption(int cartId, int productId, bool isKoli)
        {
            var entities = new B2BDbEntities();
            var cartItem = entities.Cart.FirstOrDefault(c => c.Cart_id == cartId && c.Product_id == productId);
            if (cartItem != null)
            {
                if (isKoli)
                {
                    // Kolinin miktarını ve toplam fiyatı güncelle
                    cartItem.Quantity = 12; 
                }
                else
                {
                    
                    cartItem.Quantity = 1;
                }

                cartItem.Total = cartItem.Quantity * cartItem.Products.Price;
                entities.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private decimal? CalculateDiscountedTotal(int customerId, decimal discountRate)
        {
            using (var entities = new B2BDbEntities())
            {
                var totalPrice = entities.Cart
                    .Where(c => c.User_id == customerId)
                    .Sum(c => (decimal?)c.Total);

                if (totalPrice.HasValue && totalPrice.Value > 0)
                {
                    decimal discount = discountRate / 100;
                    decimal? discountedTotal = totalPrice.Value - (totalPrice.Value * discount);
                    return discountedTotal;
                }
                else
                {
                    return 0;
                }
            }
        }
        private int GetCustomerId()
        {
            int customerId = 0;
            if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out customerId))
            {
                return customerId;
            }
            return 0;
        }

        public ActionResult OrderComplete()
        {
            return View();
        }

    }
}