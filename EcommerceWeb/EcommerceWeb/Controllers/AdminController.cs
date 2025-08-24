using AutoMapper;
using EcommerceWeb.Auth;
using EcommerceWeb.DTO;
using EcommerceWeb.Ef;
using EcommerceWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Controllers
{
    public class AdminController : Controller
    {
        EcommerceMSEntities3 db = new EcommerceMSEntities3();

        // GET: Admin
        [Logged]
        [AdminLogged]
        public ActionResult Index()
        {
            //Placed Orders
            var OrderDb = (from order in db.Orders
                              where order.StatusId == 1 
                              select order).ToList();
            var orderDTO = MapperHelper.GetMapper().Map<List<OrderDTO>>(OrderDb);
            

            return View(orderDTO);
        }

        [Logged]
        [AdminLogged]
        public ActionResult AllOrder()
        {
            var placedOrderDb = db.Orders.ToList();
            var orderDTO = MapperHelper.GetMapper().Map<List<OrderDTO>>(placedOrderDb);


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
            var orderDTO = MapperHelper.GetMapper().Map<List<OrderDTO>>(orderDb);

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
                                 || order.StatusId == 1004
                                 select order).ToList();
            var orderDTO = MapperHelper.GetMapper().Map<List<OrderDTO>>(placedOrderDb);


            return View(orderDTO);
        }


        [HttpGet]
        [Logged]
        [AdminLogged]
        public ActionResult AddProduct()
        {

            var categoryDb = db.Categories.ToList();    
            var categoryDTO = MapperHelper.GetMapper().Map<List<CategoryDTO>>(categoryDb);  

            return View(categoryDTO);
        }

        [Logged]
        [AdminLogged]
        [HttpPost]
        public ActionResult AddProduct(ProductDTO productDTO)
        {
            var existingProduct = (from pr in db.Products
                     where pr.Name == productDTO.Name
                     select pr).SingleOrDefault();

            

            if (existingProduct == null)
            {
                var productDb = MapperHelper.GetMapper().Map<Product>(productDTO);
                db.Products.Add(productDb);
            }
            else
            {
                existingProduct.Qty += productDTO.Qty;
            }

            db.SaveChanges();
            TempData["Msg"] = productDTO.Name+" added successfully";
            TempData["Class"] = "alert alert-success";
            return RedirectToAction("AllProduct","Product");
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



        [HttpGet]
        [Logged]
        [AdminLogged]
        public ActionResult ApproveReturnRequest()
        {
            var returnTrackerDb = (from rt in db.ReturnsTrackers
                                   where rt.StatusId == 1006
                                   select rt).ToList();

            var deliveryMen = db.DeliveryMen.ToList();
            ViewBag.Deliverymen = deliveryMen;

            return View(returnTrackerDb);
        }


        [HttpPost]
        [Logged]
        [AdminLogged]
        public ActionResult ApproveReturnRequest(int requestTrackerId,int deliverymanId)
        {
            var requestTrackerdb = db.ReturnsTrackers.Find(requestTrackerId);
            if (requestTrackerdb != null)
            {
                requestTrackerdb.StatusId = 1009; // Approved
                requestTrackerdb.DeliverymanId = Convert.ToInt32(deliverymanId);
                requestTrackerdb.ApprovedTime = (DateTime)DateTime.Now;
                requestTrackerdb.ApprovedTime = (DateTime)DateTime.Now;
                db.SaveChanges();
                TempData["Msg"] = "Return request approved and assigned to delivery";
            }
            else
            {
                TempData["Msg"] = "Something went wrong";
            }

            return RedirectToAction("Index");
        }

    }
}