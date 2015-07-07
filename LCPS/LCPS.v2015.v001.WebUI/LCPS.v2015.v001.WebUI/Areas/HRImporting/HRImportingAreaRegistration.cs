using System.Web.Mvc;

namespace LCPS.v2015.v001.WebUI.Areas.HRImporting
{
    public class HRImportingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HRImporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HRImporting_default",
                "HRImporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}