using B2BProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace B2BProject.Controllers
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
        public ActionResult Login(Users seller)
        {
            using (var entities = new B2BDbEntities())
            {
                if (string.IsNullOrEmpty(seller.Email) || string.IsNullOrEmpty(seller.Password))
                {
                    ViewBag.error = "Email and Password cannot be left blank!";
                    return View();
                }

                var bu = entities.Users.FirstOrDefault(b => b.Email == seller.Email &&
                                                            b.Password == seller.Password &&
                                                            b.Rol_id == 2);

                if (bu != null)
                {
                    FormsAuthentication.SetAuthCookie(seller.Email, false);
                    Session["Role_id"] = bu.Rol_id.ToString();
                    Session["Email"] = bu.Email.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Invalid Email or Password!";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Register(Users seller)
        {
            var entities = new B2BDbEntities();

            if (ModelState.IsValid)
            {

                var Seller = new Users
                {
                    Name = seller.Name,
                    Surname = seller.Surname,
                    Email = seller.Email,
                    Company_name = seller.Company_name,
                    Phone = seller.Phone,
                    Password = seller.Password,
                    Rol_id = 2

                };

                entities.Users.Add(Seller);
                entities.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return View(seller);
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