using System.Web.Mvc;
using System.Web.Routing;

namespace LCPS.v2015.v001.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var route = routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            route.DataTokens["Area"] = "Public";

        }
    }
}
