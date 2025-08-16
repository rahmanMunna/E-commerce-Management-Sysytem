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
        EcommerceMSEntities3 db = new EcommerceMSEntities3();
        
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
        public ActionResult ReceivedOrders()
        {
            //need to implement DTO
            var user = GetUser();
            var orderTrackerDb = (from ot in db.OrderTarckers // not OrderTarckers
                                  where ot.Order.CustomerId == user.CustomerId
                                  && ot.StatusId == 6 // Delivered
                                  select ot).ToList();


            return View(orderTrackerDb);
        }
    }
}