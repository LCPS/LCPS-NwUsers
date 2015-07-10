using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.Students;
using LCPS.v2015.v001.WebUI.Infrastructure;
using System.IO;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.WebUI.Areas.Import.Models;


namespace LCPS.v2015.v001.WebUI.Areas.Students.Controllers
{
    public class InstructionalLevelController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();


        #region Import

        public ActionResult ImportFile()
        {
            ImportSession i = new InstructionalLevelSession().ToImportSession();

            return View("~/Areas/Import/Views/ImportFile.cshtml", i);
        }

        [HttpPost]
        public ActionResult Preview(ImportSession s)
        {
            try
            {
                InstructionalLevelSession ss = new InstructionalLevelSession();
                ss.SessionId = s.SessionId;

                using (StreamReader sr = new StreamReader(s.ImportFile.InputStream))
                {
                    ss.ParseItems(sr);
                }


                ImportPreviewModel m = new ImportPreviewModel(ss.SessionId);
                return View("~/Areas/Import/Views/Preview.cshtml", m);
            }
            catch (Exception ex)
            {
                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Parse Import File", "Students", "InstructionalLevel", "Index"));
            }
        }


        [HttpPost]
        public ActionResult Import(ImportSession s)
        {
            try
            {
                ImportSession dbs = db.ImportSessions.First(x => x.SessionId.Equals(s.SessionId));
                InstructionalLevelSession jt = new InstructionalLevelSession(dbs);
                jt.Import();
                ImportPreviewModel m = new ImportPreviewModel(s.SessionId);
                return View("~/Areas/Import/Views/Import.cshtml", m);
            }
            catch (Exception ex)
            {
                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Import Staff", "Students", "InstructionalLevel", "ImportFile"));
            }
        }

        #endregion


        #region Crud
        // GET: /Students/InstructionalLevel/
        public ActionResult Index()
        {
            return View(db.InstructionalLevels.OrderBy(x => x.InstructionalLevelId).ToList());
        }

        // GET: /Students/InstructionalLevel/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructionalLevel instructionallevel = db.InstructionalLevels.Find(id);
            if (instructionallevel == null)
            {
                return HttpNotFound();
            }
            return View(instructionallevel);
        }

        // GET: /Students/InstructionalLevel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Students/InstructionalLevel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="InstructionalLevelKey,InstructionalLevelId,InstructionalLevelName")] InstructionalLevel instructionallevel)
        {
            if (ModelState.IsValid)
            {
                instructionallevel.InstructionalLevelKey = Guid.NewGuid();
                db.InstructionalLevels.Add(instructionallevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instructionallevel);
        }

        // GET: /Students/InstructionalLevel/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructionalLevel instructionallevel = db.InstructionalLevels.Find(id);
            if (instructionallevel == null)
            {
                return HttpNotFound();
            }
            return View(instructionallevel);
        }

        // POST: /Students/InstructionalLevel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="InstructionalLevelKey,InstructionalLevelId,InstructionalLevelName")] InstructionalLevel instructionallevel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructionallevel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructionallevel);
        }

        // GET: /Students/InstructionalLevel/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructionalLevel instructionallevel = db.InstructionalLevels.Find(id);
            if (instructionallevel == null)
            {
                return HttpNotFound();
            }
            return View(instructionallevel);
        }

        // POST: /Students/InstructionalLevel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            InstructionalLevel instructionallevel = db.InstructionalLevels.Find(id);
            db.InstructionalLevels.Remove(instructionallevel);
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
