using AutoMapper;
using EcommerceWeb.Auth;
using EcommerceWeb.DTO;
using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Controllers
{
    public class DeliveryController : Controller
    {
        EcommerceMSEntities db = new EcommerceMSEntities();
        static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderTarcker, OrderTrackerDTO>().ReverseMap();
               
            });

            var mapper = new Mapper(config);
            return mapper;

      
        }
        // GET: Delivery

        User GetUser()
        {
            var user = (User)Session["User"];
            return user;
        }

        [Logged]
        [DeliveryLogged]
        public ActionResult Index()
        {
            var user = GetUser();
            var orderTrackerDb = (from ot in db.OrderTarckers
                                  where ot.DeliveryManId == user.DeliveryManId
                                  && ot.StatusId == 1002
                                  select ot).ToList();
            if (orderTrackerDb == null)
            {
                TempData["Msg"] = "No orders assigned to you";
                TempData["Class"] = "text-danger";
            }
            //var orderTrackerDTO = GetMapper().Map<List<OrderTrackerDTO>>(orderTrackerDb);
            return View(orderTrackerDb);
        }
        [Logged]
        [DeliveryLogged]
        public ActionResult Received(int Id)
        {
            var orderTrackerDb = db.OrderTarckers.Find(Id);
            orderTrackerDb.Order.StatusId = 1003; // 1003 = Received    
            orderTrackerDb.RecievedTime = DateTime.Now;
            orderTrackerDb.StatusId = 1003; // 1003 = Received
            db.SaveChanges();
            TempData["Msg"] = "Order-" + orderTrackerDb.OrderId + " is received";

            return RedirectToAction("Index");
            
        }

        [Logged]
        [DeliveryLogged]
        public ActionResult OnGoingOrders()
        {
            var user = GetUser();
            var orderTrackerDb = (from ot in db.OrderTarckers
                                         where ot.DeliveryManId == user.DeliveryManId
                                         && ot.StatusId == 1003
                                         select ot).ToList();
            return View(orderTrackerDb);
        }

        [Logged]
        [DeliveryLogged]
        public ActionResult Delivered(int Id)
        {
            var orderTrackerDb = db.OrderTarckers.Find(Id);

            if(orderTrackerDb != null)
            {
                orderTrackerDb.StatusId = 6; // 6 = delivered
                orderTrackerDb.DeliveredTime = DateTime.Now;    
                orderTrackerDb.Order.StatusId = 6;
                db.SaveChanges();

                TempData["Msg"] = "Order-" + orderTrackerDb.OrderId + " is been delivered";  
            }
            return RedirectToAction("OnGoingOrders");
        }

        [Logged]
        [DeliveryLogged]
        public ActionResult DeliveredOrders()
        {
            var user = GetUser();   
            var orderTrackerDb = (from ot in db.OrderTarckers
                                  where ot.StatusId == 6 && ot.DeliveryManId == user.DeliveryManId  
                                  select ot).ToList();  

            return View(orderTrackerDb);

        }
    }
        
}