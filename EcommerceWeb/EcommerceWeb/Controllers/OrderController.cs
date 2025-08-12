using AutoMapper;
using EcommerceWeb.Auth;
using EcommerceWeb.DTO;
using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Controllers
{
    public class OrderController : Controller
    {
        EcommerceMSEntities db =  new EcommerceMSEntities();

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
        // GET: Order
        [Logged]
        [CustomerLogged]
        public ActionResult Index()
        {
            var p = db.Products.ToList();
            
            var product = GetMapper().Map<List<ProductDTO>>(p);

            ViewBag.TotalProductCount = product.Count; 
            
            return View(product);
        }

        [Logged]
        [CustomerLogged]
        public ActionResult AddtoCart(int id)
        {
            var p = db.Products.Find(id);
            var product = GetMapper().Map<ProductDTO>(p);
            product.Qty = 1;
            List<ProductDTO> cartProduct = null;

            if (Session["cart"] == null)
            {
                cartProduct = new List<ProductDTO>();
                
            }
            else
            {
                cartProduct = (List<ProductDTO>)Session["cart"];
               
            }
            var existProduct = (from pr in cartProduct where
                               pr.Id == id
                               select pr).SingleOrDefault();
            if(existProduct != null)
            {
                existProduct.Qty+=1;
            }
            else
            {
                cartProduct.Add(product);
            }

            Session["cart"] = cartProduct;
            TempData["Msg"] = p.Name+ " added to cart succesfully";

            return RedirectToAction("Index");
        }

        [Logged]
        [CustomerLogged]
        public ActionResult PlaceOrder(decimal total,int totalProduct)
        {
            var cart = (List<ProductDTO>)Session["cart"];

            if(cart != null)
            {                
                var order = new Order
                {
                    Date = DateTime.Now,
                    GTotal = total,
                    StatusId = 1,
                    NumberofProduct = totalProduct,
                    CustomerId = 1 // default for now
                };

                db.Orders.Add(order);
                db.SaveChanges();
                TempData["Msg"] = "Order Placed Successfully";



                //add to OrderDetails table
                foreach(var c in cart)
                {
                    //int orderId,int productId,int Qty,decimal price
                    var odDTo = new OrderDetailDTO(order.Id,c.Id,c.Qty,c.Price);
                    var odDb = GetMapper().Map<OrderDetail>(odDTo);

                    db.OrderDetails.Add(odDb);
                    
                }
                db.SaveChanges();

                Session["cart"] = null;

                return RedirectToAction("Index");   
            }
            else
            {
                TempData["Msg"] = "Cart is Empty";
            }

            return RedirectToAction("Cart");
        }

        [Logged]
        [CustomerLogged]
        public ActionResult ViewPlacesOrder()
        { 
            var ordersDb = (from o in db.Orders
                           where o.StatusId != 6
                           select o).ToList();
            var orderDTO = GetMapper().Map<List<OrderDTO>>(ordersDb); 
           
            return View(orderDTO);
        }

        [Logged]
        [CustomerLogged]
        public ActionResult Cart()
        {
            var cart = (List<ProductDTO>)Session["cart"];

            return View(cart);
        }

        //See Order Details
        [Logged]
        [CustomerLogged]
        public ActionResult ViewOrderDetails(int id)
        {
            var orderDetailsDb = (from od in db.OrderDetails
                                  where od.OrderId == id
                                  select od).ToList();
            var orderDetailDTO = GetMapper().Map<List<OrderDetailDTO>>(orderDetailsDb);
            return View(orderDetailDTO);
        }


        //Confim Order 
        [Logged]
        [AdminLogged]
        public ActionResult ConfirmOrder(int id)
        {
            //id is pk of orders Table

            var order = db.Orders.Find(id);
            if(order != null && AdjustProductQuantity(order.Id))
            {
                
                order.StatusId = 2;
                db.SaveChanges();
                TempData["Msg"] = "Order has been Confirmed";
                TempData["Class"] = "text-bg-success";
                
            }
            else
            {
                TempData["Msg"] = "Product Quantity not Available in Inventory";
                TempData["Class"] = "text-bg-danger";
            }

            return RedirectToAction("Index", "Admin");
        }

        //Handle For QTY   = 0 ---- need to implment
        public bool AdjustProductQuantity(int oId)
        {
            //OId = order id
            List<OrderDetail> odDbList = (from od in db.OrderDetails
                            where od.OrderId == oId
                            select od).ToList();

            var odDb = (from od in odDbList
                        where od.Product.Qty < od.Qty || od.Product.Qty == 0
                        select od).SingleOrDefault();

            if(odDb == null)
            {
                foreach(var od in odDbList)
                {
                    od.Product.Qty-= od.Qty;
                }
                db.SaveChanges();
                return true;
            }
            return false;
        }


        //Cancel Order
        [Logged]
        [AdminLogged]
        public ActionResult CancelOrder(int id)
        {

            var order = db.Orders.Find(id);
            if (order != null)
            {
                order.StatusId = 5; // 5  = cancelled  
                db.SaveChanges();
                TempData["Msg"] = "Order has been Cancelled"; 
                TempData["Class"] = "text-bg-danger";
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}