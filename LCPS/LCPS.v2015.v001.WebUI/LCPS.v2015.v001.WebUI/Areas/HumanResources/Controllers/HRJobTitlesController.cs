using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.WebUI.Areas.Import.Models;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    [Authorize(Roles = "APP-Admins,HR-Admins")]
    public class HRJobTitlesController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();



        #region Importing

        public ActionResult ImportFile()
        {
            ImportSession s = new ImportSession
                (
                    area: "HumanResources",
                    controller: "HRJobTitles",
                    action: "Preview",
                    itemType: typeof(HRJobTitleCandidate),
                    viewLayoutPath: "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml",
                    addIfNotExist: true,
                    updateIfExists: false,
                    viewTitle: "Import Job Titles"
                );

            return View("~/Areas/Import/Views/ImportFile.cshtml", s);
        }

        [HttpPost]
        public ActionResult Preview(ImportSession s)
        {
            try
            {
                ImportSession dbs = db.ImportSessions.First(x => x.SessionId.Equals(s.SessionId));

                HRJobTitleSession jt = new HRJobTitleSession(dbs);
                using (StreamReader sr = new StreamReader(s.ImportFile.InputStream))
                {
                    jt.ParseItems(sr);
                }


                ImportPreviewModel m = new ImportPreviewModel(s.SessionId);
                return View("~/Areas/Import/Views/Preview.cshtml", m);
            }
            catch (Exception ex)
            {
                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Parse Import File", "HumanResources", "HRJobTitles", "Index"));
            }
        }

        [HttpPost]
        public ActionResult Import(ImportSession s)
        {
            try
            {
                ImportSession dbs = db.ImportSessions.First(x => x.SessionId.Equals(s.SessionId));
                HRJobTitleSession jt = new HRJobTitleSession(dbs);
                jt.Import();
                ImportPreviewModel m = new ImportPreviewModel(s.SessionId);
                return View("~/Areas/Import/Views/Import.cshtml", m);
            }
            catch (Exception ex)
            {
                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Import Job Titles", "HumanResources", "HRJobTitles", "Preview"));
            }
        }

        #endregion


        #region Crud

        // GET: HumanResources/HRJobTitles
        public ActionResult Index()
        {
            return View(db.JobTitles.OrderBy(x => x.JobTitleId).ToList());
        }

        [HttpPost]
        public ActionResult FilterByEmployeeType(HRJobTitleFilterModel f)
        {
            return View("Index", db.JobTitles.Where(x => x.EmployeeTypeLinkId.Equals(f.EmployeeTypeLinkId)).OrderBy(x => x.JobTitleId).ToList());
        }

        // GET: HumanResources/HRJobTitles/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRJobTitle hRJobTitle = db.JobTitles.Find(id);
            if (hRJobTitle == null)
            {
                return HttpNotFound();
            }
            return View(hRJobTitle);
        }

        // GET: HumanResources/HRJobTitles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HumanResources/HRJobTitles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobTitleKey,EmployeeTypeLinkId,JobTitleId,JobTitleName")] HRJobTitle hRJobTitle)
        {
            if (ModelState.IsValid)
            {
                hRJobTitle.JobTitleKey = Guid.NewGuid();
                db.JobTitles.Add(hRJobTitle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hRJobTitle);
        }

        // GET: HumanResources/HRJobTitles/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRJobTitle hRJobTitle = db.JobTitles.Find(id);
            if (hRJobTitle == null)
            {
                return HttpNotFound();
            }
            return View(hRJobTitle);
        }

        // POST: HumanResources/HRJobTitles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobTitleKey,EmployeeTypeLinkId,JobTitleId,JobTitleName")] HRJobTitle hRJobTitle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRJobTitle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hRJobTitle);
        }

        // GET: HumanResources/HRJobTitles/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRJobTitle hRJobTitle = db.JobTitles.Find(id);
            if (hRJobTitle == null)
            {
                return HttpNotFound();
            }
            return View(hRJobTitle);
        }

        // POST: HumanResources/HRJobTitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HRJobTitle hRJobTitle = db.JobTitles.Find(id);
            db.JobTitles.Remove(hRJobTitle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
