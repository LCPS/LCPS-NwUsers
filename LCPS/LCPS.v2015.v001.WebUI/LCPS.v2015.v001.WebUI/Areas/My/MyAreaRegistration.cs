using System.Web.Mvc;

namespace LCPS.v2015.v001.WebUI.Areas.My
{
    public class MyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "My";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "My_default",
                "My/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}