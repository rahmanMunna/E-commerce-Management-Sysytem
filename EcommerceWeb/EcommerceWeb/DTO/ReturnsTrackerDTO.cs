using EcommerceWeb.Controllers;
using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWeb.DTO
{
    public class ReturnsTrackerDTO
    {
        public int Id { get; set; }
        public string ReasonToReturn { get; set; }
        public string Method { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> DeliverymanId { get; set; }
        public System.DateTime RequestTime { get; set; }
        public Nullable<System.DateTime> ApprovedTime { get; set; }
        public Nullable<System.DateTime> AssignedTime { get; set; }
        public Nullable<System.DateTime> PickupTime { get; set; }
        public Nullable<System.DateTime> RecievedTime { get; set; }
        public int OrderTrackerId { get; set; }

        public virtual DeliveryManDTO DeliveryMan { get; set; }
        public virtual OrderTrackerDTO OrderTarcker { get; set; }
        public virtual StatusDTO Status { get; set; }
    }
}