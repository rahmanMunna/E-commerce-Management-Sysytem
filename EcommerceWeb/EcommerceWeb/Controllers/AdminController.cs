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
    public class AdminController : Controller
    {
        EcommerceMSEntities db = new EcommerceMSEntities();

        static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>().ReverseMap();
                cfg.CreateMap<Order, OrderDTO>().ReverseMap();
                cfg.CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
                cfg.CreateMap<Status, StatusDTO>().ReverseMap();
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
        // GET: Admin
        [Logged]
        [AdminLogged]
        public ActionResult Index()
        {
            //OrderDb
            var placedOrderDb = (from order in db.Orders
                              where order.StatusId == 1 
                              select order).ToList();
            var orderDTO = GetMapper().Map<List<OrderDTO>>(placedOrderDb);
            

            return View(orderDTO);
        }
        public ActionResult AllOrder()
        {
            var placedOrderDb = db.Orders.ToList();
            var orderDTO = GetMapper().Map<List<OrderDTO>>(placedOrderDb);


            return View(orderDTO);
        }

        [HttpGet]
        [Logged]
        [AdminLogged]
        public ActionResult Processing()
        {
            var orderDb = (from order in db.Orders
                                 where order.StatusId == 2
                                 select order).ToList();
            var orderDTO = GetMapper().Map<List<OrderDTO>>(orderDb);

            //Load DeliveryMan and attach to ViewBag    
            ViewBag.DeliveryMen = db.DeliveryMen.ToList();


            return View(orderDTO);
        }

        //After assigning a delivery man from processing
        [Logged]
        [AdminLogged]
        [HttpPost]
        public ActionResult Processing(int DeliveryManId, int OrderId)
        {
            if (DeliveryManId > 0)
            {

                var orderTracker = new OrderTarcker
                {
                    OrderId = OrderId,
                    DeliveryManId = DeliveryManId,
                    AssignedTime = DateTime.Now,
                    StatusId = 1002, // 1002 = Assigned

                };

                db.OrderTarckers.Add(orderTracker);

                //Find the order and change status to asigned
                var o = db.Orders.Find(OrderId);
                o.StatusId = 1002;

                db.SaveChanges();


                TempData["Msg"] = "Order-" + OrderId + " is being assigned";
            }
            else
            {
                TempData["Msg"] = "Please select a Delivery Man";
            }

            return RedirectToAction("Processing");
        }


        [Logged]
        [AdminLogged]
        public ActionResult Cancelled()
        {
            var placedOrderDb = (from order in db.Orders
                                 where order.StatusId == 5
                                 select order).ToList();
            var orderDTO = GetMapper().Map<List<OrderDTO>>(placedOrderDb);


            return View(orderDTO);
        }
        [HttpGet]
        [Logged]
        [AdminLogged]
        public ActionResult AddProduct()
        {
            return View();
        }

        [Logged]
        [AdminLogged]
        [HttpPost]
        public ActionResult AddProduct(ProductDTO prd)
        {
            var existingProduct = (from pr in db.Products
                     where pr.Name == prd.Name
                     select pr).SingleOrDefault();

            

            if (existingProduct == null)
            {
                var productDb = GetMapper().Map<Product>(prd);
                db.Products.Add(productDb);
            }
            else
            {
                existingProduct.Qty += prd.Qty;
            }

            db.SaveChanges();
            TempData["Msg"] = prd.Name+" added successfully"; 
            return RedirectToAction("Index");
        }

        [Logged]
        [AdminLogged]
        public ActionResult AssignedQueue()
        {
            var orderTrackerDb = (from ot in db.OrderTarckers
                                 where ot.StatusId == 1002 // Assigned
                                 select ot).ToList();
            return View(orderTrackerDb);
        }


    }
}