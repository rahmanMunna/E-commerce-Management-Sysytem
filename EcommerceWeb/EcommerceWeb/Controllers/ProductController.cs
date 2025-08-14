using EcommerceWeb.Auth;
using EcommerceWeb.DTO;
using EcommerceWeb.Ef;
using EcommerceWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Controllers
{
    public class ProductController : Controller
    {
        EcommerceMSEntities db = new EcommerceMSEntities();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [Logged]
        [AdminLogged]
        public ActionResult AllProduct()
        {
            //get all product from db
            var productDb = db.Products.ToList();
            var productDTO = MapperHelper.GetMapper().Map<List<ProductDTO>>(productDb);

            return View(productDTO);
        }

        [HttpGet]
        [Logged]
        [AdminLogged]
        public ActionResult Update(int id)
        {
            var productDb = db.Products.Find(id);

            var productDTO = MapperHelper.GetMapper().Map<ProductDTO>(productDb);

            return View(productDTO);

        }
        [HttpPost]
        [Logged]
        [AdminLogged]
        public ActionResult Update(ProductDTO product)
        {
            var productDb = MapperHelper.GetMapper().Map<Product>(product);
            db.Products.AddOrUpdate(productDb); // upsert

            db.SaveChanges();
            TempData["Msg"] = product.Name + " has been updated";
            TempData["Class"] = "alert alert-success";

            return RedirectToAction("AllProduct");
        }
    }
}