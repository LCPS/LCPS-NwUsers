using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.NwUsers.Importing2;
using LCPS.v2015.v001.NwUsers.LcpsEmail;

using LCPS.v2015.v001.WebUI.Areas.Import2.Models;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsEmail.Controllers
{
    public class LcpsEmailController : Controller
    {
        // GET: LcpsEmail/LcpsEmail
        public ActionResult Import()
        {
            ImportViewModel m = new ImportViewModel();
            m.FormAction = "Parse";
            m.FormArea = "LcpsEmail";
            m.FormController = "LcpsEmail";
            m.SubmitText = "View File";
            m.ImportAction = "Process";
            m.ImportArea = "LcpsEmail";
            m.ImportController = "LcpsEmail";
            m.SessionVarName = "LcpsEmailImport";

            return View(m);
        }

        public ActionResult Parse(ImportViewModel m)
        {
            m.ProcessImportFile(typeof(LcpsEmailImportRecord));
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