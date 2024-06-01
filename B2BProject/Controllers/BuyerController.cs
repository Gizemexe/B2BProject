using B2BProject.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace B2BProject.Controllers
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
        public ActionResult Login(Users buyer)
        {
            using (var entities = new B2BDbEntities())
            {
                if (string.IsNullOrEmpty(buyer.Email) || string.IsNullOrEmpty(buyer.Password))
                {
                    ViewBag.error = "Email and Password cannot be left blank!";
                    return View();
                }

                var bu = entities.Users.FirstOrDefault(b => b.Email == buyer.Email &&
                                                            b.Password == buyer.Password &&
                                                            b.Rol_id == 3);

                if (bu != null)
                {
                    FormsAuthentication.SetAuthCookie(buyer.Email, false);
                    Session["UserId"] = bu.User_id.ToString();
                    Session["Email"] = bu.Email.ToString();
                    Session["RoleId"] = bu.Rol_id.ToString();
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

        [HttpPost]
        public ActionResult Register(Users buyer)
        {
            if (ModelState.IsValid)
            {
                using (var entities = new B2BDbEntities())
                {
                    var newBuyer = new Users
                    {
                        Name = buyer.Name,
                        Surname = buyer.Surname,
                        Email = buyer.Email,
                        Company_name = buyer.Company_name,
                        Phone = buyer.Phone,
                        Password = buyer.Password,
                        Rol_id = 3
                    };

                    entities.Users.Add(newBuyer);
                    entities.SaveChanges();

                    return RedirectToAction("Login");
                }
            }
            return View(buyer);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear(); // Oturum verilerini temizle
            Session.Abandon(); // Oturumu sonlandır
            return RedirectToAction("Index", "Home");
        }
    }
}
