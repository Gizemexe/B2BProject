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
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Admin/Login
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

        // GET: Admin/Logout
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear(); // Oturum verilerini temizle
            Session.Abandon(); // Oturumu sonlandır
            return RedirectToAction("Login");
        }

        // GET: Admin/ViewUsers
        public ActionResult ViewUsers()
        {
            using (var entities = new B2BDbEntities())
            {
                var users = entities.Users.ToList();
                return View(users);
            }
        }

        // GET: Admin/AddCategory
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        // POST: Admin/AddCategory
        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            using (var entities = new B2BDbEntities())
            {
                if (string.IsNullOrEmpty(category.Category_name))
                {
                    ViewBag.error = "Category Name cannot be left blank!";
                    return View();
                }

                entities.Categories.Add(category);
                entities.SaveChanges();
                return RedirectToAction("ViewCategories");
            }
        }

        // GET: Admin/DeleteCategory
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            using (var entities = new B2BDbEntities())
            {
                var category = entities.Categories.FirstOrDefault(c => c.Category_id == id);
                if (category != null)
                {
                    entities.Categories.Remove(category);
                    entities.SaveChanges();
                }
                return RedirectToAction("ViewCategories");
            }
        }

        // GET: Admin/ViewCategories
        public ActionResult ViewCategories()
        {
            using (var entities = new B2BDbEntities())
            {
                var categories = entities.Categories.ToList();
                return View(categories);
            }
        }
    }
}
