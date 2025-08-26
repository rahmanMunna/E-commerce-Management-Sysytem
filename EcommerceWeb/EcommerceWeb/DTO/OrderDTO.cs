using EcommerceWeb.Controllers;
using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWeb.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public decimal GTotal { get; set; }
        public int StatusId { get; set; }
        public int NumberofProduct { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }

        public virtual StatusDTO Status { get; set; }
        public virtual CustomerDTO Customer { get; set; }

        public virtual PaymentMethodDTO PaymentMethod1 { get; set; }
        public virtual TransactionStatuDTO TransactionStatu { get; set; }

    }
}