using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Anvil.v2015.v001.Domain.Entities;

using LCPS.v2015.v001.WebUI.Areas.Computers.Models;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.LcpsComputers;
using LCPS.v2015.v001.NwUsers.LcpsComputers.Peripherals;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using Anvil.v2015.v001.Domain.Exceptions;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using System.DirectoryServices;

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
            if (name != null)
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

        [HttpGet]
        public ActionResult LookupComputer(string name)
        {
            string result = "Success";

           try
           {
               if (String.IsNullOrEmpty(name))
                   throw new Exception("Please supply a computer name to search for");

               ApplicationBase app = LcpsDbContext.DefaultApp;
               string un = string.Format("{0}\\{1}", app.LDAPDomain, app.LDAPUserName);

               RemoteComputer c = new RemoteComputer(name, un, app.LDAPPassword);

               c.Refresh();
           }
            catch(Exception ex)
           {
               AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
               result = ec.ToUL();
           }

           return Content(result);
        }

        public ActionResult ArchiveComputer(string name)
        {
            if (name != null)
            {
                RemoteComputer c = new RemoteComputer(name, @"lcps\earlyms", "Lcp$-pw1");
                c.Refresh();
                return View(c);
            }
            return View();
        }

        [HttpGet]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RelocateComputer(string b, string r, string u, string n)
        {
            string result = "Success";
            

            try
            {
                if (String.IsNullOrEmpty(b))
                    throw new Exception("Building ID is required");

                if (String.IsNullOrEmpty(r))
                    throw new Exception("Room ID is required");

                if (String.IsNullOrEmpty(u))
                    throw new Exception("Unit Number is required");

                if (String.IsNullOrEmpty(n))
                    throw new Exception("Computer Name is required");

                ApplicationBase app = LcpsDbContext.DefaultApp;
                string un = string.Format("{0}\\{1}", app.LDAPDomain, app.LDAPUserName);

                RemoteComputer c = new RemoteComputer(n, un, app.LDAPPassword);

                c.Refresh();

                ComputerInfo info = c.ToComputerInfo();
                info.BuildingId = new Guid(b);
                info.RoomId = new Guid(r);
                info.UnitNumber = u.PadLeft(3, '0');

                c.DBAcrchive(info, User.Identity.Name);

                info.UpdateLDAP();
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                result = ec.ToUL();
            }

            return Content(result);
        }

        [HttpPost]
        public ActionResult ArchiveComputer(ComputerInfo m)
        {
            try
            {
                RemoteComputer c = new RemoteComputer(m.ComputerName, @"lcps\earlyms", "Lcp$-pw1");

                c.Refresh();

                c.DBAcrchive(m, User.Identity.Name);

                return View("Index", new { name = m.ComputerName });
            }
            catch (Exception ex)
            {
                AnvilExceptionModel em = new AnvilExceptionModel(ex, "Archive Computer", "Computers", "Computers", "Index");
                return View("Error", em);
            }
        }

        #region Drop Downs

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetBuildings()
        {
            LcpsDbContext db = new LcpsDbContext();

            List<HRBuilding> bb = db.Buildings.OrderBy(x => x.Name).ToList();

            var result = (from b in bb
                          select new
                          {
                              id = b.BuildingKey.ToString(),
                              name = b.Name
                          }).ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetRooms(Guid buildingId)
        {
            LcpsDbContext db = new LcpsDbContext();

            List<HRRoom> bb = db.Rooms.Where(x => x.BuildingId.Equals(buildingId)).OrderBy(x => x.RoomNumber).ToList();

            var result = (from b in bb
                          select new
                          {
                              id = b.RoomKey.ToString(),
                              name = (b.PrimaryOccupant == null) ? b.RoomNumber : b.RoomNumber + " " + b.PrimaryOccupant
                          }).ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}