using B2B_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace B2B_Project.Controllers
{
    public class SellerController : Controller
    {
        // GET: Seller
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User seller)
        {
            using (var entities = new B2BDbEntities())
            {
                if (string.IsNullOrEmpty(seller.Name) || string.IsNullOrEmpty(seller.Password))
                {
                    ViewBag.error = "Username and Password cannot be left blank!";
                    return View();
                }

                var ad = entities.Users
                    .FirstOrDefault(a => a.Name == seller.Name  && a.Password == seller.Password);

                if (ad != null)
                {
                    FormsAuthentication.SetAuthCookie(seller.Name, false);
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

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User seller)
        {
            var Entities = new B2BDbEntities();

            var Seller = new User
            {

                Name = seller.Name,
                Surname = seller.Surname,
                Email = seller.Email,
                Company_name = seller.Company_name,
                Phone = seller.Phone,
                Password = seller.Password,
                Rol_id = 3,

            };

            Entities.Users.Add(Seller);
            Entities.SaveChanges();

            return View();
        }
    }
}
