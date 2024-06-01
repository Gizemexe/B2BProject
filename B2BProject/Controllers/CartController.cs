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
                CalculateTotalPrice(customerId);

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
        private decimal? CalculateTotalPrice(int customerId)
        {
            using (var entities = new B2BDbEntities())
            {
                var totalPrice = entities.Cart
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