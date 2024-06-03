using B2BProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2BProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var entities = new B2BDbEntities())
            {
                var productList = entities.Products.ToList();
                TempData["products"] = productList;
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact(int id)
        {
            using (var entities = new B2BDbEntities())
            {
                var customer = entities.Users.Include("Roles").FirstOrDefault(c => c.User_id == id);
                return View(customer);
            }
        }

        public ActionResult Details(int id)
        {
            using (var entities = new B2BDbEntities())
            {
                var product = entities.Products
                                      .FirstOrDefault(p => p.Product_id == id);

                if (product != null)
                {
                    if (product.Stock < 10)
                    {
                        ViewBag.WarningMessage = $"Warning: Stock for product '{product.Product_name}' is running low! There is left {product.Stock}";
                    }
                    else
                    {
                        ViewBag.StockMessage = $"Stock for product '{product.Product_name}' is {product.Stock}!";
                    }
                }

                return View(product);
            }
        }

        public ActionResult SearchProducts(string search, int categoryId = 0, int minPrice = 0, int maxPrice = int.MaxValue)
        {
            using (var entities = new B2BDbEntities())
            {
                var query = entities.Products.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p => p.Product_name.Contains(search) || p.Description.Contains(search));
                }

                if (categoryId > 0)
                {
                    query = query.Where(p => p.Category_id == categoryId);
                }

                query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);

                var searchResults = query.ToList();

                return View("SearchProducts", searchResults);
            }
        }
        
        public ActionResult Categories(int id)
        {
            using (var entities = new B2BDbEntities())
            {
                var productsList = entities.Products.Where(p => p.Category_id == id).ToList();
                TempData["product"] = productsList;
                return View();
            }
        }
    }
}