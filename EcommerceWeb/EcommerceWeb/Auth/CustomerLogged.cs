using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Auth
{
    public class CustomerLogged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["User"] != null)
            {
                var user = (User)httpContext.Session["User"];

                if (user.Role == "customer")
                {
                    return true; // User is authenticated and is a customer
                }
                else
                {
                    return false; // User is authenticated but not a customer   
                }
            }
            return false; // User is not authenticated  
        }
    }
}