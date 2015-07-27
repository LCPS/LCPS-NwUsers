using LCPS.v2015.v001.WebUI.Infrastructure;
using System.Web.Mvc;

namespace LCPS.v2015.v001.WebUI.Areas.My.Controllers
{
    [LcpsControllerAuthorization()]
    public class ProfileController : Controller
    {
        // GET: My/Profile
        public ActionResult Index()
        {
            return View();
        }
    }
}