using B2BProject.Models;
using System.Data.Entity;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;
using System.IO;

namespace B2BProject.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
           
            using (var entities = new B2BDbEntities())
            {
                var orders = entities.Orders
                                     .Include("OrderDetails.Products")
                                     .Include("Users")
                                     .ToList();

                TempData["orders"] = orders;
                return View(orders);
            }
        }
        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users admin)
        {
            using (var entities = new B2BDbEntities())
            {
                if (string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.Password))
                {
                    ViewBag.error = "Email and Password cannot be left blank!";
                    return View();
                }

                var ad = entities.Users.FirstOrDefault(a => a.Email == admin.Email &&
                                                            a.Password == admin.Password &&
                                                            a.Rol_id == 1);

                if (ad != null)
                {
                    FormsAuthentication.SetAuthCookie(admin.Email, false);
                    Session["UserId"] = ad.User_id.ToString();
                    Session["Email"] = ad.Email.ToString();
                    Session["RoleId"] = ad.Rol_id.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Invalid Email or Password!";
                    return View();
                }
            }
        }
        public ActionResult ViewUsers()
        {
            using (var entities = new B2BDbEntities())
            {
                var users = entities.Users.Include("Roles").ToList();
                TempData["Customer"] = users;
                return View();
            }
        }

        [HttpGet]
        public ActionResult UpdateProfile(int id)
        {
            // Retrieve the customer information from the database using the current user's ID
            //int customerId = Convert.ToInt32(Session["Customer_id"]);

            var entities = new B2BDbEntities();
            var customer = entities.Users.Find(id);

            if (customer == null)
            {
                return HttpNotFound(); // Handle the case where the customer is not found
            }

            return View(customer);
        }

        [HttpPost]
        public ActionResult UpdateProfile(Users updatedCustomer)
        {
            if (ModelState.IsValid)
            {
                var customerID = Convert.ToInt32(Session["UserId"]);

                if (customerID > 0)
                {
                    var entities = new B2BDbEntities();
                    var existingCustomer = entities.Users.Find(customerID);

                    if (existingCustomer != null)
                    {
                        // Update properties with new values
                        existingCustomer.Email = updatedCustomer.Email;
                        existingCustomer.Name = updatedCustomer.Name;
                        existingCustomer.Surname = updatedCustomer.Surname;
                        existingCustomer.Password = updatedCustomer.Password;
                        existingCustomer.Phone = updatedCustomer.Phone;
                        existingCustomer.Company_name = updatedCustomer.Company_name;

                        // Save changes to the database
                        entities.Entry(existingCustomer).State = EntityState.Modified;
                        entities.SaveChanges();

                        //TempData["SuccessMessage"] = "Profile updated successfully!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Customer not found");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Customer ID not valid");
                }
            }

            // If ModelState is not valid or customer is not found, return to the update profile page with errors
            return View();
        }
        public ActionResult Categories() 
        {
            var entities = new B2BDbEntities();

            var categoryList = entities.Categories.ToList();

            TempData["categories"] = categoryList;

            return View();
        }
        public ActionResult Category()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Category(Categories cat)
        {
            
                using (var entities = new B2BDbEntities())
                {
                Categories newCategory = new Categories
                {
                    Category_name = cat.Category_name,
                };

                    entities.Categories.Add(newCategory);
                    entities.SaveChanges();
                }
            return RedirectToAction("Category");
        }

        [HttpGet]
        public ActionResult DeleteCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteCategory(int CategoryID)
        {
            using (B2BDbEntities entities = new B2BDbEntities())
            {
                var productsToDelete = entities.Products.Where(p => p.Category_id == CategoryID);
                entities.Products.RemoveRange(productsToDelete);

                var categoryToDelete = entities.Categories.FirstOrDefault(c => c.Category_id == CategoryID);
                if (categoryToDelete != null)
                {
                    entities.Categories.Remove(categoryToDelete);
                }

                entities.SaveChanges();
                return RedirectToAction("Category");
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear(); // Oturum verilerini temizle
            Session.Abandon(); // Oturumu sonlandır
            return RedirectToAction("Index","Home");
        }
        
    }
}
