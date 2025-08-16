using AutoMapper;
using EcommerceWeb.Auth;
using EcommerceWeb.DTO;
using EcommerceWeb.Ef;
using EcommerceWeb.Helpers;
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
        EcommerceMSEntities3 db = new EcommerceMSEntities3();

        // GET: Order
        [Logged]
        [CustomerLogged]
        public ActionResult Index()
        {
            var productDb = db.Products.ToList();
            
            var productDTO = MapperHelper.GetMapper().Map<List<ProductDTO>>(productDb);

            ViewBag.TotalProductCount = productDTO.Count;


            // Grouping Products by Category
            var productGroupByCategory = (from p in productDTO
                                          group p by p.Category.Name into g
                                          select new ProductGroupByCategoryDTO
                                          {
                                              CategoryName = g.Key,
                                              Products = g.ToList()
                                          }).ToList();


            return View(productGroupByCategory);
        }

        [Logged]
        [CustomerLogged]
        public ActionResult AddtoCart(int id)
        {
            var p = db.Products.Find(id);
            var product = MapperHelper.GetMapper().Map<ProductDTO>(p);
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
            var user = (User)Session["User"];

            if(cart != null)
            {                
                var order = new Order
                {
                    Date = DateTime.Now,
                    GTotal = total,
                    StatusId = 1,
                    NumberofProduct = totalProduct,
                    CustomerId = (int)user.CustomerId // default for now
                };

                db.Orders.Add(order);
                db.SaveChanges();
                TempData["Msg"] = "Order Placed Successfully";



                //add to OrderDetails table
                foreach(var c in cart)
                {
                    //int orderId,int productId,int Qty,decimal price
                    var odDTo = new OrderDetailDTO(order.Id,c.Id,c.Qty,c.Price);
                    var odDb = MapperHelper.GetMapper().Map<OrderDetail>(odDTo);

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
        public ActionResult ViewPlacedOrders()
        { 
            var ordersDb = (from o in db.Orders
                           where o.StatusId == 1 // 1 = Placed
                            select o).ToList();
            var orderDTO = MapperHelper.GetMapper().Map<List<OrderDTO>>(ordersDb); 
           
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
            var orderDetailDTO = MapperHelper.GetMapper().Map<List<OrderDetailDTO>>(orderDetailsDb);
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
                order.StatusId = 5; // 5  = cancelled by Admin
                db.SaveChanges();
                TempData["Msg"] = "Order has been Cancelled"; 
                TempData["Class"] = "text-bg-danger";
            }

            return RedirectToAction("Index", "Admin");
        }

        [Logged]
        [CustomerLogged]
        public ActionResult CancelOrderByCustomer(int id)
        {
            var orderDb = db.Orders.Find(id);
            if(orderDb != null)
            {
                orderDb.StatusId = 1004;
                db.SaveChanges();
                TempData["Msg"] = "Order has been Cancelled";
                TempData["Class"] = "text-bg-danger";
            }
            return RedirectToAction("ViewPlacedOrders");
        }

        [Logged]
        [CustomerLogged]
        public ActionResult CancelledOrders()
        {
            var orderDb = (from o in db.Orders
                           where o.StatusId == 5 // 5 = cancelled by Admin
                           || o.StatusId == 1004 // 1004 = cancelled by customer
                           select o).ToList();
            var orderDTO = MapperHelper.GetMapper().Map<List<OrderDTO>>(orderDb);
            return View(orderDTO);
        }


        [Logged]
        [CustomerLogged]
        public ActionResult InProgress()
        {
            var orderDb = (from o in db.Orders
                           where o.StatusId != 6 // 6 = Delivered
                            && o.StatusId != 5 // 5 = cancelled by Admin
                            && o.StatusId != 1004 // 1004 = cancelled by customer
                           select o).ToList();
            var orderDTO = MapperHelper.GetMapper().Map<List<OrderDTO>>(orderDb);
            return View(orderDTO);
        }
    }
}