using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWeb.Models
{
    public class NewUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Phone { get; set; }
       
    }
}