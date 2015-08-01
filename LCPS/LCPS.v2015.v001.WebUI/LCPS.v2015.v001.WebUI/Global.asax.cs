using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Anvil.v2015.v001.Domain.Exceptions;
using Anvil.v2015.v001.Domain.Entities;
using LCPS.v2015.v001.WebUI.Infrastructure;

using LCPS.v2015.v001.WebUI.Areas.Errors.Controllers;

namespace LCPS.v2015.v001.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            

            LCPS.v2015.v001.WebUI.Infrastructure.LcpsDbContext db = new WebUI.Infrastructure.LcpsDbContext();
            LCPS.v2015.v001.NwUsers.Properties.Settings.Default.ConnectionString = db.Database.Connection.ConnectionString;

            
            
        }

       
    }
}
