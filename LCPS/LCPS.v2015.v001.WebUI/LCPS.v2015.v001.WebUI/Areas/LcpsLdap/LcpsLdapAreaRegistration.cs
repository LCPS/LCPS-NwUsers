using System.Web.Mvc;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap
{
    public class LcpsLdapAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LcpsLdap";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LcpsLdap_default",
                "LcpsLdap/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}