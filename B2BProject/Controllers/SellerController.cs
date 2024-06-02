using projeb2b.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projeb2b.Controllers
{
    public class SellerController : Controller
    {
        // GET: Seller/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Seller/Login
        [HttpPost]
        public ActionResult Login(int d)
        {
            return View();
        }

        // GET: Seller/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Seller/Register
        [HttpPost]
        public ActionResult Register(User seller)
        {
            using (var entities = new B2BDbEntities())
            {
                var Seller = new User
                {
                    Name = seller.Name,
                    Surname = seller.Surname,
                    Company_name = seller.Company_name,
                    Email = seller.Email,
                    Phone = seller.Phone,
                    Password = seller.Password,
                    Rol_id = 2
                };

                entities.Users.Add(Seller);
                entities.SaveChanges();
            }

            return View();
        }

        // GET: Seller/AddCoupon
        [HttpGet]
        public ActionResult AddCoupon()
        {
            return View();
        }

        // POST: Seller/AddCoupon
        [HttpPost]
        public ActionResult AddCoupon(Coupon coupon, List<int> productIds)
        {
            using (var entities = new B2BDbEntities())
            {
                foreach (var productId in productIds)
                {
                    var product = entities.Products.Find(productId);
                    if (product != null)
                    {
                        ApplyDiscountToProduct(coupon, product);
                    }
                }
                entities.Coupons.Add(coupon);
                entities.SaveChanges();
            }

            return View();
        }

        private void ApplyDiscountToProduct(Coupon coupon, Product product)
        {
            product.DiscountedPrice = product.Price - (product.Price * coupon.Discount / 100);
        }

        // POST: Seller/RemoveCoupon
        [HttpPost]
        public ActionResult RemoveCoupon(int couponId)
        {
            using (var entities = new B2BDbEntities())
            {
                var coupon = entities.Coupons.Find(couponId);
                if (coupon != null)
                {
                    entities.Coupons.Remove(coupon);
                    entities.SaveChanges();
                }
            }

            return View();
        }

        // GET: Seller/CheckStock
        [HttpGet]
        public ActionResult CheckStock(int productId)
        {
            using (var entities = new B2BDbEntities())
            {
                var product = entities.Products.Find(productId);
                if (product != null)
                {
                    ViewBag.Stock = product.Stock;
                }
                else
                {
                    ViewBag.Stock = "Product not found";
                }
            }

            return View();
        }

        // GET: Seller/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }

        // POST: Seller/AddProduct
        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            using (var entities = new B2BDbEntities())
            {
                entities.Products.Add(product);
                entities.SaveChanges();
            }

            return View();
        }

        // POST: Seller/RemoveProduct
        [HttpPost]
        public ActionResult RemoveProduct(int productId)
        {
            using (var entities = new B2BDbEntities())
            {
                var product = entities.Products.Find(productId);
                if (product != null)
                {
                    entities.Products.Remove(product);
                    entities.SaveChanges();
                }
            }

            return View();
        }

        // POST: Seller/ApplyDiscount
        [HttpPost]
        public ActionResult ApplyDiscount(int productId, decimal discountPercentage)
        {
            using (var entities = new B2BDbEntities())
            {
                var product = entities.Products.Find(productId);
                if (product != null)
                {
                    product.DiscountedPrice = product.Price - (product.Price * discountPercentage / 100);
                    entities.SaveChanges();
                }
            }

            return View();
        }

        // GET: Seller/Logout
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}
