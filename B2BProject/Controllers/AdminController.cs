<<<<<<< HEAD
﻿using B2BProject.Models;
=======
using B2BProject.Models;
>>>>>>> f86ced1f07240feb457de30763d1968e29fb9844
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
                    Session["Role_id"] = ad.Rol_id.ToString();
                    Session["Email"] = ad.Email.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Invalid Email or Password!";
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
