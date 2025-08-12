using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Auth
{
    public class Logged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Check if the user is authenticated
            if (httpContext.Session["User"] != null)
            {
                return true;
            }
            return false;
        }
    }
        
    
}