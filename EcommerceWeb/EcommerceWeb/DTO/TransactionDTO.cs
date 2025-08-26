using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWeb.DTO
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int Status { get; set; }
        public int Method { get; set; }
        public System.DateTime Date { get; set; }

        public virtual OrderDTO Order { get; set; }
        public virtual PaymentMethodDTO PaymentMethod { get; set; }
        public virtual TransactionStatuDTO TransactionStatu { get; set; }
    }
}