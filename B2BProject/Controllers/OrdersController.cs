using B2BProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2BProject.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index()
        {
            var userId = GetUserId();
            using (var entities = new B2BDbEntities())
            {
                var orders = entities.Orders
                                     .Include("OrderDetails.Products")
                                     .Include("Users")
                                     .Where(o => o.OrderDetails.Any(od => od.Products.User_id == userId)) // Sadece bu satıcıya ait ürünlerden siparişleri filtrele
                                     .ToList();

                TempData["orders"] = orders;
                return View(orders);
            }
        }

        [HttpPost]
        public ActionResult BuyNow(string address, decimal? grandTotal)
        {
            int userId = GetUserId();

            using (var entities = new B2BDbEntities())
            {
                var cartItems = entities.Cart
                                        .Where(c => c.User_id == userId)
                                        .ToList();

                if (!cartItems.Any())
                {
                    // Eğer sepet boşsa, kullanıcıya bir mesaj gösterebilir veya başka bir işlem yapabilirsiniz.
                    return RedirectToAction("Index", "Cart");
                }

                var order = new Orders
                {
                    User_id = userId,
                    Date = DateTime.Now,
                    Total = grandTotal,
                    Address = address,
                    Order_status = "Pending",
                    Payment_status = "Unpaid"
                };

                entities.Orders.Add(order);
                entities.SaveChanges();

                int orderId = order.Order_id;

                foreach (var item in cartItems)
                {
                    var product = entities.Products.SingleOrDefault(p => p.Product_id == item.Product_id);
                    if (product != null)
                    {
                        var orderDetail = new OrderDetails
                        {
                            Order_id = orderId,
                            Product_id = item.Product_id,
                            Quantity = item.Quantity,
                            UnitPrice = product.Price,
                            Total = item.Total // Her öğenin kendi toplam tutarı olmalıdır
                        };

                        entities.OrderDetails.Add(orderDetail);

                        // Sepetten öğeyi kaldır
                        entities.Cart.Remove(item);

                        // Ürün stoğunu azalt
                        product.Stock -= item.Quantity;
                    }
                }

                entities.SaveChanges();
            }

            return RedirectToAction("OrderComplete", "Cart");
        }

        public ActionResult MyOrders()
        {
            int userId = GetUserId();

            if (userId == 0)
            {
                return RedirectToAction("Login", "User");
            }

            using (var entities = new B2BDbEntities())
            {
                var orders = entities.Orders
                                     .Where(o => o.User_id == userId)
                                     .OrderByDescending(o => o.Date)
                                     .ToList();

                return View(orders);
            }
        }
        public ActionResult OrderDetails(int orderId)
        {
            using (var entities = new B2BDbEntities())
            {
                var order = entities.OrderDetails
                                    .Include("Products")
                                    .Where(o => o.Order_id == orderId)
                                    .ToList();

                if (order == null)
                {
                    return HttpNotFound();
                }

                return View(order);
            }
        }
        private int GetUserId()
        {
            if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out int userId))
            {
                return userId;
            }
            return 0;
        }
    }
}