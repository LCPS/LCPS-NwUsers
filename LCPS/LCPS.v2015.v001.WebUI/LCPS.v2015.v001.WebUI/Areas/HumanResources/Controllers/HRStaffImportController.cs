using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.Importing2;
using LCPS.v2015.v001.WebUI.Areas.Import2.Models;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    public class HRStaffImportController : Controller
    {
        //
        // GET: /HumanResources/HRStaffImport/
        public ActionResult Import()
        {
            ImportViewModel m = new ImportViewModel();
            m.FormAction = "Parse";
            m.FormArea = "HumanResources";
            m.FormController = "HRStaffImport";
            m.SubmitText = "View File";
            m.ImportAction = "Process";
            m.ImportArea = "HumanResources";
            m.ImportController = "HRStaffImport";
            m.SessionVarName = "HRStaffImport";

            return View(m);
        }

        public ActionResult Parse(ImportViewModel m)
        {
            m.ProcessImportFile(typeof(HRStaffImportRecord));
            Session[m.SessionVarName] = m;
            return View("Import", m);
        }

        public ActionResult Process(string id)
        {
            ImportViewModel m = (ImportViewModel)Session[id];
            m.ImportFile.Import();
            return View("Import", m);
        }
	}
}