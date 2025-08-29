using EcommerceWeb.Controllers;
using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWeb.DTO
{
    public class OrderTrackerDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int DeliveryManId { get; set; }
        public System.DateTime AssignedTime { get; set; }
        public Nullable<System.DateTime> RecievedTime { get; set; }
        public Nullable<System.DateTime> DeliveredTime { get; set; }
        public int StatusId { get; set; }

        public virtual DeliveryManDTO DeliveryMan { get; set; }
        public virtual OrderDTO Order { get; set; }
        public virtual StatusDTO Status { get; set; }


       

    }
}