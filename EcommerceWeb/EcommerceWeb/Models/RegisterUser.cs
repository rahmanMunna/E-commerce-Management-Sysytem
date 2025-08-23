using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWeb.Models
{
    public class RegisterUser
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 30 characters.")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-z][a-z0-9]*@[a-z]+\.[a-z]{3}$", ErrorMessage = "Invald Email Address")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&()_+\-}{\]\[])[A-Za-z\d!@#$%^&()_+\-}{\]\[]]{8,16}$", ErrorMessage = "Password Structure is not Correct")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 characters.")]
        public string Password { get; set; }


        [Required]
        [PasswordValidationAttribute]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"^01[3456789]\d{11}$", ErrorMessage = "Phone(11 digit) number format : 01XXXXXXXXX ")]
        public string Phone { get; set; }
    }
}