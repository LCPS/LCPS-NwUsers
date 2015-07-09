using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.HumanResources.Students;
using LCPS.v2015.v001.WebUI.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources
{
    public class InstructionalLevelController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();

        // GET: HumanResources/InstructionalLevel
        public ActionResult Index()
        {
            return View(db.InstructionalLevels.ToList());
        }

        // GET: HumanResources/InstructionalLevel/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructionalLevel instructionalLevel = db.InstructionalLevels.Find(id);
            if (instructionalLevel == null)
            {
                return HttpNotFound();
            }
            return View(instructionalLevel);
        }

        // GET: HumanResources/InstructionalLevel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HumanResources/InstructionalLevel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstructionalLevelKey,InstructionalLevelId,InstructionalLevelName")] InstructionalLevel instructionalLevel)
        {
            if (ModelState.IsValid)
            {
                instructionalLevel.InstructionalLevelKey = Guid.NewGuid();
                db.InstructionalLevels.Add(instructionalLevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instructionalLevel);
        }

        // GET: HumanResources/InstructionalLevel/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructionalLevel instructionalLevel = db.InstructionalLevels.Find(id);
            if (instructionalLevel == null)
            {
                return HttpNotFound();
            }
            return View(instructionalLevel);
        }

        // POST: HumanResources/InstructionalLevel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstructionalLevelKey,InstructionalLevelId,InstructionalLevelName")] InstructionalLevel instructionalLevel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructionalLevel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructionalLevel);
        }

        // GET: HumanResources/InstructionalLevel/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructionalLevel instructionalLevel = db.InstructionalLevels.Find(id);
            if (instructionalLevel == null)
            {
                return HttpNotFound();
            }
            return View(instructionalLevel);
        }

        // POST: HumanResources/InstructionalLevel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            InstructionalLevel instructionalLevel = db.InstructionalLevels.Find(id);
            db.InstructionalLevels.Remove(instructionalLevel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
