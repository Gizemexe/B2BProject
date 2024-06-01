using B2BProject.Models;
using System.Data.Entity;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace B2BProject.Controllers
{
    public class AdminController : Controller
    {
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

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear(); // Oturum verilerini temizle
            Session.Abandon(); // Oturumu sonlandır
            return RedirectToAction("Index","Home");
        }
    }
}
