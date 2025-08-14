using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWeb.DTO
{
    public class ProductGroupByCategoryDTO
    {
        public string CategoryName { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}