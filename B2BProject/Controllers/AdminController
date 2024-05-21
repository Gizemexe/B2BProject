using projeb2b.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace projeb2b.Controllers
{
    public class AdminController : Controller
    {
        // GET: Buyer
        [HttpGet]
        [HttpPost]
        public ActionResult Login(User admin)
        {
            using (var entities = new B2BDbEntities())
            {
                if (string.IsNullOrEmpty(admin.Name) || string.IsNullOrEmpty(admin.Password))
                {
                    ViewBag.error = "Username and Password cannot be left blank!";
                    return View();
                }

                var ad = entities.Users
                    .FirstOrDefault(a => a.Name == admin.Name && a.Password == admin.Password);

                if (ad != null)
                {
                    FormsAuthentication.SetAuthCookie(admin.Name, false);
                    Session["Admin_id"] = ad.User_id.ToString();
                    Session["Admin_name"] = ad.Name.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Invalid Username or Password!";
                }
            }

            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear(); // Oturum verilerini temizle
            Session.Abandon(); // Oturumu sonlandır
            return RedirectToAction("Login");
        }


    }
}
