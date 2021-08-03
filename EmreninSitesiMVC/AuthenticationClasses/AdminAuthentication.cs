using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmreninSitesiMVC.AuthenticationClasses
{
    public class AdminAuthentication:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["admin"] != null)
            {
                return true;
            }


            httpContext.Response.Redirect("/Admin/Login");
            return false;
            
        }
    }
}