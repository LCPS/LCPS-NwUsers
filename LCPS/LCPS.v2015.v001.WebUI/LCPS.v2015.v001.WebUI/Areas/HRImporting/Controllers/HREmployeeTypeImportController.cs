using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;

using LCPS.v2015.v001.WebUI.Areas.HRImporting.Models;
using LCPS.v2015.v001.WebUI.Areas.Import.Models;
using System.Runtime.Serialization.Formatters.Binary;

using Anvil.v2015.v001.Domain.Exceptions;

namespace LCPS.v2015.v001.WebUI.Areas.HRImporting.Controllers
{
    public class HREmployeeTypeImportController : Controller
    {

        LcpsDbContext db = new LcpsDbContext();

        // GET: HRImporting/HREmployeeTypeImport
        public ActionResult Index()
        {
            HREmployeeTypeImportSession s = new HREmployeeTypeImportSession();
            return View(s);
        }

        [HttpPost]
        public ActionResult Preview(ImportSession s)
        {

            HREmployeeTypePreviewModel m = new HREmployeeTypePreviewModel(s.SessionId, s.ImportFile.InputStream);
            ImportPreviewModel p = new ImportPreviewModel(s.SessionId);

            return View("~/Areas/Import/Views/Preview.cshtml", p);
        }

        [HttpPost]
        public ActionResult Import(ImportSession s)
        {
            try
            {
                HREmployeeTypeImportModel m = new HREmployeeTypeImportModel(s.SessionId);

                ImportPreviewModel pm = new Import.Models.ImportPreviewModel(s.SessionId);

               
                return View("~/Areas/Import/Views/Import.cshtml", pm);
            }
            catch (Exception ex)
            {
                return View("Error", new AnvilExceptionModel(ex, "Import", "HRImporting", "HREmployeeTypeImport", "Import"));
            }
        }

        public ActionResult Sessions()
        {
            string n = typeof(HREmployeeTypeCandidate).AssemblyQualifiedName;
            List<ImportSession> s = db.ImportSessions.Where(x => x.FullAssemblyTypeName == n).ToList();

            ImportSessionModel m = new ImportSessionModel()
            {
                Sessions = s,
                Layout = "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml",
                PageHeader = "Employee Type Sessions"

            };

            return View("~/Areas/Import/Views/Sessions.cshtml", m);
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                //delete the Item from the DB using the id
                // and return Deleted as the status value in JSON
                ImportSession i = db.ImportSessions.First(x => x.SessionId.Equals(id));
                db.ImportSessions.Remove(i);
                db.SaveChanges();

                return RedirectToAction("Sessions", "HREmployeeTypeImport", new { area = "HRImporting" });
            }
            catch (Exception ex)
            {
                return View("Error", new AnvilExceptionModel(ex, "Delete Import Session", "HRImport", "HREmployeeTypeImport", "Sessions"));
            }
            
        }
    }
}