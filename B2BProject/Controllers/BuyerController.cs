using projeb2b.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projeb2b.Controllers
{
    public class BuyerController : Controller
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
        public ActionResult Register(User buyer)
        {
            var entities = new B2BDbEntities();
            var Buyer = new User
            {
             Name=buyer.Name,
             Surname=buyer.Surname,
             Company_name=buyer.Company_name,
             Email=buyer.Email,
             Phone=buyer.Phone,
             Password=buyer.Password,
             Rol_id=3
            };

            entities.Users.Add(Buyer);
            entities.SaveChanges();
            return View();
        }
       
    }
}
