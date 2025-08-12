using EcommerceWeb.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        [Logged]
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                Session["User"] = null; // Clear the session
                TempData["Msg"] = "You have been logged out successfully.";
                TempData["Class"] = "text-danger";
            }
            else
            {
                TempData["Msg"] = "You are not logged in.";
            }

            return RedirectToAction("Index","Login");
        }

        
    }
}