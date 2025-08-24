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
        EcommerceMSEntities3 db = new EcommerceMSEntities3();
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

            //group by category
            var productGroupByCategory = from p in productDTO
                                         group p by p.Category.Name into g
                                         select new ProductGroupByCategoryDTO
                                         {
                                             CategoryName = g.Key,
                                             Products = g.ToList()
                                         };

            return View(productGroupByCategory);
        }

        [HttpGet]
        [Logged]
        [AdminLogged]
        public ActionResult Update(int id)
        {
            var productDb = db.Products.Find(id);
            var allCategories = db.Categories.ToList(); 

            ViewBag.Categories = MapperHelper.GetMapper().Map<List<CategoryDTO>>(allCategories);

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