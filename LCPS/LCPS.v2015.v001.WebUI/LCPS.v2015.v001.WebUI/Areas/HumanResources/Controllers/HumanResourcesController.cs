using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using Anvil.v2015.v001.Domain.Exceptions;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    public class HumanResourcesController : Controller
    {
        // GET: HumanResources/HumanResources
        public ActionResult Backup(string u)
        {
            try
            {
                HRStaffBackup b = new HRStaffBackup();
                b.Backup("Lcps-HumanResources.bck");
                return Redirect(u);
            }
            catch (Exception ex)
            {
                return View("Error", new AnvilExceptionModel(ex, "backup Human Resources", "HumanResources", "HREmployeeTypes", "Index"));
            }
        }
    }
}