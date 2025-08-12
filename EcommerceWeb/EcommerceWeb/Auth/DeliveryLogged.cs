using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Auth
{
    public class DeliveryLogged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["User"] != null)
            {
                var user = (User)httpContext.Session["User"];
                if (user.Role == "deliveryMan")
                {
                    return true; // User is authenticated and is a deliveryman
                }
                else
                {
                    return false; // User is authenticated but not a deliveryman   
                }
            }
            return false; // User is not authenticated  
        }
    }
}