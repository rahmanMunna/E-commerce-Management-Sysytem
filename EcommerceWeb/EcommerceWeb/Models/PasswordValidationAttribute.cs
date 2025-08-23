using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWeb.Models
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var registerUser = (RegisterUser)validationContext.ObjectInstance;
            if (registerUser.Password.ToString() != value.ToString())
            {
                return new ValidationResult("Password and Confirm Password do not match.");
            }
            return ValidationResult.Success;
        }
    }
}