using B2BProject.Models;
using projeb2b.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceProject2.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            using (var entities = new B2BDbEntities())
            {
                var productList = entities.Products.ToList();
                TempData["products"] = productList;
                return View();
            }
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

                product.Category_id = entities.Categories.FirstOrDefault(i => i.Category_id == product.Category_id);

                if (Session["Admin_id"] != null)
                {
                    product.User_id = Convert.ToInt32(Session["Admin_id"]);
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
                var categoryList = entities.Categories
                    .Select(i => new SelectListItem
                    {
                        Text = i.Category_name,
                        Value = i.Category_id.ToString()
                    })
                    .ToList();

                ViewBag.CategoryList = categoryList;

                var product = entities.Products.Find(productID);
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
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Image = path;
                    existingProduct.Category_id = product.Category_id;
                    existingProduct.Stock = product.Stock;

                    if (Session["Admin_id"] != null)
                    {
                        existingProduct.User_id = Convert.ToInt32(Session["User_id"]);
                    }

                    entities.Entry(existingProduct).State = EntityState.Modified;
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



        private int GetCustomerId()
        {
            int customerId = 0;
            if (Session["Customer_id"] != null && int.TryParse(Session["Customer_id"].ToString(), out customerId))
            {
                return customerId;
            }
            return 0;
        }

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
                    Response.Write("<script>alert('Only .JPG, .JPEG, .PNG formats are allowed')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file')</script>");
                path = "-1";
            }

            return path;
        }
    }
}