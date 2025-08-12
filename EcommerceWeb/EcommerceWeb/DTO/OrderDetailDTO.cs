using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWeb.DTO
{
    public class OrderDetailDTO
    {
        public OrderDetailDTO(int orderId,int productId,int Qty,decimal price) 
        {
            this.OrderId = orderId;
            this.ProductId = productId;
            this.Qty = Qty;
            this.Price = price;

        }
        
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public virtual ProductDTO Product { get; set; } 
        public virtual OrderDTO Order { get; set; } 

        

    }
}