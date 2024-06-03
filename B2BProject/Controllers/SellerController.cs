using B2BProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace B2BProject.Controllers
{
    public class SellerController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            using (var entities = new B2BDbEntities())
            {
                var productList = entities.Products.Where(p => p.User_id == userId).Include("Categories").ToList();
                TempData["products"] = productList;
                return View();
            }
        }

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
        public ActionResult Register(Users seller)
        {
            if (ModelState.IsValid)
            {
                using (var entities = new B2BDbEntities())
                {
                    var newSeller = new Users
                    {
                        Name = seller.Name,
                        Surname = seller.Surname,
                        Email = seller.Email,
                        Company_name = seller.Company_name,
                        Phone = seller.Phone,
                        Password = seller.Password,
                        Rol_id = 2
                    };

                    entities.Users.Add(newSeller);
                    entities.SaveChanges();

                    return RedirectToAction("Login");
                }
            }
            return View(seller);
        }
        public ActionResult Coupons()
        {
            using (var entities = new B2BDbEntities())
            {
                var couponList = entities.Coupons.Include("Users").ToList();

                TempData["Coupon"] = couponList;
                return View();
            }
        }
        public ActionResult ActivateCoupon(int couponId)
        {
            using (var entities = new B2BDbEntities())
            {
                var coupon = entities.Coupons.Find(couponId);

                if (coupon != null)
                {
                    coupon.IsActive = true;
                    entities.SaveChanges();
                }

                return RedirectToAction("Coupons");
            }
        }

        public ActionResult DeactivateCoupon(int couponId)
        {
            using (var entities = new B2BDbEntities())
            {
                var coupon = entities.Coupons.Find(couponId);

                if (coupon != null)
                {
                    coupon.IsActive = false;
                    entities.SaveChanges();
                }

                return RedirectToAction("Coupons");
            }
        }

        [HttpGet]
        public ActionResult AddCoupons()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        // POST: Seller/AddCoupon
        [HttpPost]
        public ActionResult AddCoupons(Coupons coupon, string productCode)
        {
            var currentUserId = Convert.ToInt32(Session["UserId"]);
            using (var entities = new B2BDbEntities())
            {
                var product = entities.Products.FirstOrDefault(p => p.Product_code == productCode);
                if (product != null)
                {
                    coupon.Product_id = product.Product_id; // Ürün koduna göre ilgili ürün bulundu, Product_id'yi ayarla
                    coupon.Start_date = DateTime.Now;
                    coupon.End_date = DateTime.ParseExact(Request.Form["End_date"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    coupon.User_id = currentUserId;
                    entities.Coupons.Add(coupon);
                    entities.SaveChanges();
                    return RedirectToAction("Coupons", "Seller"); // veya başka bir yönlendirme yapabilirsiniz
                }
                else
                {
                    ModelState.AddModelError("", "Product with the given code does not exist."); // Ürün koduyla eşleşen bir ürün bulunamadı hatası
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult CheckStock()
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            using (var entities = new B2BDbEntities())
            {
                var products = entities.Products.Where(p => p.User_id == userId).ToList();
                if (products.Any())
                {
                    TempData["Inventory"] = products;
                }
                else
                {
                    TempData["Inventory"] = new List<Products>(); // Boş liste döndür
                    ViewBag.Message = "No products found for this user.";
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Products()
        {
            using (var entities = new B2BDbEntities())
            {
                var categoryList = entities.Categories
                    .Select(i => new SelectListItem
                    {
                        Text = i.Category_name,
                        Value = i.Category_id.ToString()
                    })
                    .ToList();

                ViewBag.CategoryList = categoryList;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Products(Products product, HttpPostedFileBase img)
        {
            using (var entities = new B2BDbEntities())
            {
                string path = uploadimage(img);
                product.Image = path;

                product.Categories = entities.Categories.Where(i => i.Category_id == product.Categories.Category_id).FirstOrDefault();

                if (Session["UserId"] != null)
                {
                    product.User_id = Convert.ToInt32(Session["UserId"]);
                }

                entities.Products.Add(product);
                entities.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult DeleteProducts()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteProducts(int productID)
        {
            using (var entities = new B2BDbEntities())
            {
                var product = entities.Products.Find(productID);
                if (product != null)
                {
                    entities.Products.Remove(product);
                    entities.SaveChanges();
                }

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult UpdateProducts(int productID)
        {
            using (var entities = new B2BDbEntities())
            {
                var product = entities.Products
                    .Include(p => p.Categories) // Eager load the Categories property
                    .FirstOrDefault(p => p.Product_id == productID);

                if (product == null)
                {
                    return HttpNotFound(); // If the product is not found, return 404 Not Found
                }

                var categoryList = entities.Categories
                    .Select(i => new SelectListItem
                    {
                        Text = i.Category_name,
                        Value = i.Category_id.ToString()
                    })
                    .ToList();

                ViewBag.CategoryList = categoryList;

                return View(product);
            }
        }

        [HttpPost]
        public ActionResult UpdateProducts(Products product, HttpPostedFileBase img)
        {
            using (var entities = new B2BDbEntities())
            {
                var existingProduct = entities.Products.Find(product.Product_id);
                if (existingProduct != null)
                {
                    string path = uploadimage(img);
                    existingProduct.Product_name = product.Product_name;
                    existingProduct.Product_code = product.Product_code;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Image = path;
                    existingProduct.Category_id = product.Categories.Category_id;
                    existingProduct.Stock = product.Stock;

                    if (Session["UserId"] != null)
                    {
                        existingProduct.User_id = Convert.ToInt32(Session["UserId"]);
                    }

                   
                    entities.SaveChanges();

                    ViewBag.Message = "Product updated successfully.";
                }
                else
                {
                    ViewBag.Message = "Failed to update product.";
                }

                return RedirectToAction("Index");
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear(); // Oturum verilerini temizle
            Session.Abandon(); // Oturumu sonlandır
            return RedirectToAction("Index", "Home");
        }

        //UPLOAD IMAGE CODE//
        public string uploadimage(HttpPostedFileBase file)
        {
            Random r = new Random();

            string path = "-1";

            int random = r.Next();

            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        string filename = random + Path.GetFileName(file.FileName);
                        string serverPath = Server.MapPath("~/Content/Images/");
                        path = Path.Combine(serverPath, filename);
                        file.SaveAs(path);
                        path = "Content/Images/" + filename;
                    }
                    catch (Exception ex)
                    {
                        path = "-1";

                        throw;
                    }
                }

                else
                {
                    Response.Write("<script>alert('Only .JPG , .JPEG , .PNG Formats Are Allowed')</script>");
                }

            }

            else
            {
                Response.Write("<script>alert('Please Select a File')</script>");

                path = "-1";
            }

            return path;
        }

    }
}
