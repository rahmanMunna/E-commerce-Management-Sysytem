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
            var deliverymenListDb = db.DeliveryMen.ToList(); ;
            ViewBag.DeliveryMen = deliverymenListDb;


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
            var orderTrackerDTO = MapperHelper.GetMapper().Map<List<OrderTrackerDTO>>(orderTrackerDb);  
            return View(orderTrackerDb);
        }

       
        [Logged]
        [AdminLogged]
        public ActionResult ReturnRequestDetails()
        {
            var returnTrackerDb = (from rt in db.ReturnsTrackers
                                   where rt.StatusId == 1006
                                   select rt).ToList();

           var returnTrackerDTO = MapperHelper.GetMapper().Map<List<ReturnsTrackerDTO>>(returnTrackerDb);

            return View(returnTrackerDTO);
        }

        [Logged]
        [AdminLogged]
        public ActionResult ViewReturnDetails(int id)
        {
            var returnTrackerDb = db.ReturnsTrackers.Find(id);
            var returnTrackerDTO = MapperHelper.GetMapper().Map<ReturnsTrackerDTO>(returnTrackerDb);

            return View(returnTrackerDTO);
        }

        [HttpGet]
        [Logged]
        [AdminLogged]
        public ActionResult ApproveReturnRequest(int id)
        {
            var deliveryManListDb = db.DeliveryMen.ToList();
            var deliveryManDTO = MapperHelper.GetMapper().Map<List<DeliveryManDTO>>(deliveryManListDb);

            ViewBag.ReturnTrackerId = id;   


            return View(deliveryManDTO);
        }

        [HttpPost]
        [Logged]
        [AdminLogged]
        public ActionResult ApproveReturnRequest(int returnTrackerId, int deliverymanId = 0)
        {
            if(deliverymanId != 0)
            {
                var returnTrackerDb = db.ReturnsTrackers.Find(returnTrackerId);
                returnTrackerDb.OrderTarcker.StatusId = 1009; // Return Approved
                returnTrackerDb.StatusId = 1009; // Return Approved 
                returnTrackerDb.OrderTarcker.Order.StatusId = 1009; // Return Approved 
                returnTrackerDb.DeliverymanId = (int)deliverymanId;
                returnTrackerDb.ApprovedTime = (DateTime)DateTime.Now;
                returnTrackerDb.AssignedTime = (DateTime)DateTime.Now;

                db.SaveChanges();

                var deliverymanName = db.DeliveryMen.Find(deliverymanId).Name;

                TempData["Msg"] = "Return request approved and assigned to deliveryman Id : "+ deliverymanName;

                return RedirectToAction("ReturnRequestDetails");
            }
            else
            {
                TempData["Msg"] = "Please select a deliveryman";
                return View();
            }


            
        }





    }
}