using System.Web.Mvc;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsEmail
{
    public class LcpsEmailAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LcpsEmail";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LcpsEmail_default",
                "LcpsEmail/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}