using AutoMapper;
using EcommerceWeb.DTO;
using EcommerceWeb.Ef;
using EcommerceWeb.Helpers;
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
    public class SignupController : Controller
    {
        EcommerceMSEntities3 db = new EcommerceMSEntities3();

        
        // GET: Signup
        [HttpGet]
        public ActionResult Index()
        {

            return View(new RegisterUser());
        }


        [HttpPost]
        public ActionResult Index(RegisterUser newUser)
        {

            if(newUser.Password == newUser.ConfirmPassword)
            {
                var customerDTO = new Customer
                {
                    Name = newUser.Name,
                    Email = newUser.Email,
                    Phone = newUser.Phone,

                };

                var customerDb = MapperHelper.GetMapper().Map<Customer>(customerDTO);
                db.Customers.Add(customerDb);
                db.SaveChanges();

                // Hash the password using MD5
                string hashedPassword = HashHelper.GetMd5Hash(newUser.Password);

                //Insert to Users table
                var userDTO = new UserDTO
                {
                    Username = newUser.Name + customerDb.Id,
                    Email = newUser.Email,
                    Password = hashedPassword,
                    Role = "customer",
                    CustomerId = (int)customerDb.Id
                };

                var userDb = MapperHelper.GetMapper().Map<Ef.User>(userDTO);


                db.Users.Add(userDb);

                db.SaveChanges();

                TempData["Msg"] = "Signup successful. Please login to continue.";
                TempData["Class"] = "text-success";


                return RedirectToAction("Index", "Login");
            }
            else
            {
                               
                TempData["Msg"] = "Password and Confirm Password do not match.";
                TempData["Class"] = "text-danger";
                return View(newUser);
            }
        }

       
    }
}