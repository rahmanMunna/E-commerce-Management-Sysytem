using EcommerceWeb.Ef;
using EcommerceWeb.Helpers;
using EcommerceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Controllers
{
    public class LoginController : Controller
    {
        EcommerceMSEntities3 db = new EcommerceMSEntities3();
        // GET: Login


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.LoginUser user)
        {
            string hashedPassword = HashHelper.GetMd5Hash(user.Password);

            var userDb = (from u in db.Users
                         where u.Username == user.Username && u.Password == hashedPassword
                         select u).SingleOrDefault();

            if(userDb == null)
            {
                TempData["Msg"] = "Invalid Username or Password";
                TempData["Class"] = "text-danger";
                return View();
            }
            //User credintial = true;
            else
            {
                Session["User"] = userDb;
                if (userDb.Role == "customer")
                {
                    return RedirectToAction("Index", "Customer");
                }
                else if(userDb.Role == "deliveryMan")
                {
                    return RedirectToAction("Index", "Delivery");
                }
                else
                {
                    return RedirectToAction("Index", "Admin");  
                }
                
            }

            
        }
        

    }
}