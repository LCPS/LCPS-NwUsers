using System.Web.Mvc;

namespace LCPS.v2015.v001.WebUI.Areas.Google
{
    public class GoogleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Google";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Google_default",
                "Google/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}