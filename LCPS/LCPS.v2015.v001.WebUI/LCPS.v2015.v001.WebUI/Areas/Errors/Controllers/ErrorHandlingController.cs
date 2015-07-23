using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Anvil.v2015.v001.Domain.Exceptions;

namespace LCPS.v2015.v001.WebUI.Areas.Errors.Controllers
{
    public class ErrorHandlingController : Controller
    {
        //
        // GET: /Errors/ErrorHandling/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AppStart()
        {

            System.Exception ex = Server.GetLastError();
            Server.ClearError();

            AnvilExceptionModel ec = null;
            if (ex == null)
            {
                ec = new AnvilExceptionModel(new Exception("An unknown error prevented this application from starting"), "Application Start Failure", null, null, null);
            }
            else
            {
                ec = new AnvilExceptionModel(ex, "Application Start Failure", null, null, null);
            }

            return View(ec);
        }
	}
}