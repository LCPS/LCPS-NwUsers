using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using LCPS.v2015.v001.NwUsers.Importing2;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapAccounts;

using LCPS.v2015.v001.WebUI.Areas.Import2.Models;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Controllers
{
    public class LdapAccountImportController : Controller
    {
        //
        // GET: /LcpsLdap/LdapAccountImport/
        // GET: LcpsEmail/LcpsEmail
        public ActionResult Import()
        {
            ImportViewModel m = new ImportViewModel();
            m.FormAction = "Parse";
            m.FormArea = "LcpsLdap";
            m.FormController = "LdapAccountImport";
            m.SubmitText = "View File";
            m.ImportAction = "Process";
            m.ImportArea = "LcpsLdap";
            m.ImportController = "LdapAccountImport";
            m.SessionVarName = "LcpsAccountImport";

            return View(m);
        }

        public ActionResult Parse(ImportViewModel m)
        {
            m.ProcessImportFile(typeof(LdapAccountImportRecord));
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