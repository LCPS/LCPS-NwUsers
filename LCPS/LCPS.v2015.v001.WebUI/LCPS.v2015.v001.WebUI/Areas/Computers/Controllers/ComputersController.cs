using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.WebUI.Areas.Computers.Models;
using LCPS.v2015.v001.NwUsers.LcpsComputers;

namespace LCPS.v2015.v001.WebUI.Areas.Computers.Controllers
{
    public class ComputersController : Controller
    {
        //
        // GET: /Computers/Computers/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RemoteComputer(string name)
        {
            if(name != null)
            {
                RemoteComputer c = new RemoteComputer(name, @"lcps\earlyms", "Lcp$-pw1");
                c.Refresh();
                return View(c);
            }
            return View();
        }

        public ActionResult Lookup(ComputerLookupModel m)
        {
            return RedirectToAction("RemoteComputer", new { name = m.ComputerName });
        }
	}
}