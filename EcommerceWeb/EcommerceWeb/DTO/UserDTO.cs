using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWeb.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public Nullable<int> CustomerId { get; set; }
    }
}