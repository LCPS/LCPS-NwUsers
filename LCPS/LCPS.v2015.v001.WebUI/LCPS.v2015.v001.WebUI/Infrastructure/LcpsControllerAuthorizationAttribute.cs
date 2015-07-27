using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Diagnostics;


namespace LCPS.v2015.v001.WebUI.Infrastructure
{
    public class LcpsControllerAuthorizationAttribute : AuthorizeAttribute
    {
        private LcpsMvcControllerACLManager manager = new LcpsMvcControllerACLManager();

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            if(Area == null & Controller == null & Action == null)
                return httpContext.User.Identity.IsAuthenticated;

            if(Action == null & Controller != null)
                return manager.UserCanAccess(HttpContext.Current.User, Area, Controller);
            else
            {
                if (Action != null)
                    return manager.UserCanAccess(HttpContext.Current.User, Action);
            }

            return true;

        }
    }
}