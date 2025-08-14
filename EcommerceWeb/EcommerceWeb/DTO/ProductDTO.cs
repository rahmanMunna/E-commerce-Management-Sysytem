using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWeb.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Pruduct must have a name")]
        public string Name { get; set; }

        [Required]
        //[RegularExpression(@"^\d+$", ErrorMessage = "Qty must be a valid Integer Number")]
        public int Qty { get; set; }


        [Required]
        //[RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Qty must be a valid number")]
        public decimal Price { get; set; }
    }
}