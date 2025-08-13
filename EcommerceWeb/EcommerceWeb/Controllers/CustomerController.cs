using AutoMapper;
using EcommerceWeb.Auth;
using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        EcommerceMSEntities db = new EcommerceMSEntities();
        
        User GetUser()
        {
            var user = (User)Session["User"];
            return user;
        }

        [Logged]
        [CustomerLogged]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Order");   
            
        }


        [Logged]
        [CustomerLogged]
        public ActionResult RecievedOrders()
        {
            //need to implement DTO
            var user = GetUser();
            var orderTrackerDb = (from ot in db.OrderTarckers
                                  where ot.Order.CustomerId == user.CustomerId
                                  && ot.StatusId == 6 // 6 = delivered
                                  select ot).ToList();
                                                   
            return View(orderTrackerDb);
        }
    }
}