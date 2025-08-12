using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Auth
{
    public class AdminLogged : AuthorizeAttribute
    {
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["User"] != null)
            {
                var user = (User)httpContext.Session["User"];
                if (user.Role == "admin")
                {
                    return true; // User is authenticated and is an admin
                }
                else
                {
                    return false; // User is authenticated but not an admin 
                }
            }
            return false; // User is not authenticated
        }
    }
}