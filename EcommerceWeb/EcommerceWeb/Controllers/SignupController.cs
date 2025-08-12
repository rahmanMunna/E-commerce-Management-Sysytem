using AutoMapper;
using EcommerceWeb.DTO;
using EcommerceWeb.Ef;
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
        EcommerceMSEntities db = new EcommerceMSEntities();

        static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer,CustomerDTO>().ReverseMap();
                cfg.CreateMap<Ef.User,UserDTO>().ReverseMap();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
        // GET: Signup
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Index(NewUser newUser)
        {

            var customerDTO = new Customer
            {
                Name = newUser.Name,
                Email = newUser.Email,
                Phone = newUser.Phone,
                
            };

            var customerDb = GetMapper().Map<Customer>(customerDTO);
            db.Customers.Add(customerDb);

            db.SaveChanges();


            var userDTO = new UserDTO
            {
                Username = newUser.Name + customerDb.Id,
                Email = newUser.Email,
                Password = newUser.Password,
                Role = "customer",
                CustomerId = (int)customerDb.Id
            };

            var userDb = GetMapper().Map<Ef.User>(userDTO);  
              

            db.Users.Add(userDb);

            db.SaveChanges();

            TempData["Msg"] = "Signup successful. Please login to continue.";
            TempData["Class"] = "text-success";


            return RedirectToAction("Index", "Login");
        }

        static string GetMd5Hash(string input)
        {
            // Create MD5 instance
            using (MD5 md5 = MD5.Create())
            {
                // Convert input string to byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Compute the MD5 hash
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("x2"));  // x2 = lowercase hex format

                return sb.ToString();
            }
        }
    }
}