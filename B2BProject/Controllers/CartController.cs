using projeb2b.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EcommerceProject2.Controllers
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
                CalculateTotalPrice(customerId);

                using (var entities = new B2BDbEntities())
                {
                    var orderCartItems = entities.Carts
                        .Where(c => c.User_id == customerId)
                        .Include(c => c.Product)
                        .ToList();

                    return View(orderCartItems);
                }
            }
            return View();
        }

        // POST: Cart/AddToOrderCart
        [HttpPost]
        public ActionResult AddToOrderCart(int productId, int amount, decimal price, string size)
        {
            int customerId = GetCustomerId();
            if (customerId != 0)
            {
                using (var entities = new B2BDbEntities())
                {
                    var existingCartItem = entities.Carts
                        .FirstOrDefault(c => c.Product_id == productId && c.User_id == customerId);

                    if (existingCartItem == null)
                    {
                        var cartItem = new Cart
                        {
                            User_id = customerId,
                            Product_id = productId,
                            Quantity = amount,
                            Price = price,
                            Total = amount * price
                        };

                        entities.Carts.Add(cartItem);
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
            return RedirectToAction("Login", "User");
        }

        // POST: Cart/RemoveFromOrderCart
        [HttpPost]
        public ActionResult RemoveFromOrderCart(int cartId)
        {
            using (var entities = new B2BDbEntities())
            {
                var cartItem = entities.Carts.Find(cartId);

                if (cartItem != null)
                {
                    if (cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                        cartItem.Total -= cartItem.Price;
                    }
                    else
                    {
                        entities.Carts.Remove(cartItem);
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
                    int count = entities.Carts.Count(c => c.User_id == customerId);

                    ViewBag.Count = count;
                }
            }
            return PartialView();
        }

        // Apply Coupon
        public ActionResult ApplyCoupon(string couponCode)
        {
            using (var entities = new B2BDbEntities())
            {
                var coupon = entities.Coupons
                    .FirstOrDefault(c => c.Coupon_Code == couponCode && c.IsActive);

                if (coupon != null)
                {
                    int customerId = GetCustomerId();
                    decimal? discountedTotal = CalculateDiscountedTotal(customerId, coupon.Discount);

                    ViewBag.DiscountedTotal = discountedTotal.Value;
                    TempData["DiscountedTotal"] = discountedTotal.Value;
                }
                else
                {
                    ViewBag.CouponError = "Invalid coupon code or coupon is not active.";
                    TempData["CouponError"] = "Invalid coupon code or coupon is not active.";
                }

                return RedirectToAction("GetOrderCartItems");
            }
        }

        private decimal? CalculateTotalPrice(int customerId)
        {
            using (var entities = new B2BDbEntities())
            {
                var totalPrice = entities.Carts
                    .Where(c => c.User_id == customerId)
                    .Sum(c => (decimal?)c.Total);

                if (totalPrice.HasValue && totalPrice.Value > 0)
                {
                    ViewBag.TotalPrice = totalPrice.Value;
                    return totalPrice.Value;
                }
                else
                {
                    ViewBag.TotalPrice = "There are no products in your cart.";
                }

                return 0;
            }
        }

        [HttpPost]
        public ActionResult ChangeAmount(int cartId, string changeAmount)
        {
            using (var entities = new B2BDbEntities())
            {
                var cartItem = entities.Carts.Find(cartId);

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

        private decimal? CalculateDiscountedTotal(int customerId, decimal discountRate)
        {
            using (var entities = new B2BDbEntities())
            {
                var totalPrice = entities.Carts
                    .Where(c => c.User_id == customerId)
                    .Sum(c => (decimal?)c.Total);

                if (totalPrice.HasValue && totalPrice.Value > 0)
                {
                    decimal discount = discountRate / 100;
                    decimal? discountedTotal = totalPrice.Value - (totalPrice.Value * discount);

                    ViewBag.DiscountedTotal = discountedTotal.Value;
                    return discountedTotal.Value;
                }
                else
                {
                    ViewBag.DiscountedTotal = "There are no products in your cart.";
                }

                return 0;
            }
        }

        private int GetCustomerId()
        {
            int customerId = 0;
            if (Session["User_id"] != null && int.TryParse(Session["User_id"].ToString(), out customerId))
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