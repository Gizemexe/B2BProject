using projeb2b.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EcommerceProject2.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index()
        {
            using (var entities = new B2BDbEntities())
            {
                var orders = entities.Orders
                                     .Include("OrderDetails.Product")
                                     .Include("User")
                                     .ToList();

                TempData["orders"] = orders;
                return View(orders);
            }
        }

        [HttpPost]
        public ActionResult BuyNow()
        {
            int userId = GetUserId();

            using (var entities = new B2BDbEntities())
            {
                var cartItems = entities.Carts
                                        .Where(c => c.User_id == userId)
                                        .ToList();

                if (!cartItems.Any())
                {
                    // Eğer sepet boşsa, kullanıcıya bir mesaj gösterebilir veya başka bir işlem yapabilirsiniz.
                    return RedirectToAction("Index", "Cart");
                }

                var order = new Order
                {
                    User_id = userId,
                    Date = DateTime.Now,
                    Total = cartItems.Sum(c => c.Total),
                    Address = "User's address here", // Kullanıcının adresi burada belirtilmeli
                    Order_status = "Pending",
                    Payment_status = "Unpaid"
                };

                entities.Orders.Add(order);
                entities.SaveChanges();

                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        Order_id = order.Order_id,
                        Product_id = item.Product_id,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price,
                        Total = item.Total
                    };

                    entities.OrderDetails.Add(orderDetail);

                    var product = entities.Products.SingleOrDefault(p => p.Product_id == item.Product_id);
                    if (product != null)
                    {
                        product.Stock -= item.Quantity;
                    }

                    // Sepetten öğeyi tek tek sil
                    entities.Carts.Remove(item);
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

        private int GetUserId()
        {
            if (Session["User_id"] != null && int.TryParse(Session["User_id"].ToString(), out int userId))
            {
                return userId;
            }
            return 0;
        }
    }
}
