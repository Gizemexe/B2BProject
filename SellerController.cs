using projeb2b.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projeb2b.Controllers
{
    public class SellerController : Controller
    {
        // GET: Buyer
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(int d)
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User seller)
        {
            var entities = new B2BDbEntities();
            var Seller = new User
            {
                Name = seller.Name,
                Surname = seller.Surname,
                Company_name = seller.Company_name,
                Email = seller.Email,
                Phone = seller.Phone,
                Password = seller.Password,
                Rol_id = 2
            };

            entities.Users.Add(seller);
            entities.SaveChanges();
            return View();
        }

    }
}