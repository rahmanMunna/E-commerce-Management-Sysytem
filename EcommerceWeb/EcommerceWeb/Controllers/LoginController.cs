using EcommerceWeb.Ef;
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
        EcommerceMSEntities db = new EcommerceMSEntities();
        // GET: Login


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.User user)
        {
            string hashedPassword = GetMd5Hash(user.Password);

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
        static string GetMd5Hash(string input)
        {
            if (input != null)
            {
                // Create MD5 instance
                using (MD5 md5 = MD5.Create())
                {
                    // Convert input string to byte array
                    byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                    // Compute the MD5 hash
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Convert byte array to hexadecimal string
                    StringBuilder sb = new StringBuilder();
                    foreach (var b in hashBytes)
                        sb.Append(b.ToString("x2"));  // x2 = lowercase hex format

                    return sb.ToString();
                }
            }
            return null;


        }


    }
}