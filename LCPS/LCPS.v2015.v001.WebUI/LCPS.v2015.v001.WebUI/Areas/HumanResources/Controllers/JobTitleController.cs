using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.WebUI.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    [Authorize(Roles = "APP-Admins,HR-Managers")]
    public class JobTitleController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();

        // GET: /HumanResources/JobTitle/
        public ActionResult Index()
        {
            var jobtitles = db.JobTitles.Include(h => h.HREmployeeType);
            return View(jobtitles.ToList().OrderBy(x => x.JobTitleId));
        }

        // GET: /HumanResources/JobTitle/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRJobTitle hrjobtitle = db.JobTitles.Find(id);
            if (hrjobtitle == null)
            {
                return HttpNotFound();
            }
            return View(hrjobtitle);
        }

        // GET: /HumanResources/JobTitle/Create
        public ActionResult Create()
        {
            ViewBag.RecordId = new SelectList(db.EmployeeTypes, "RecordId", "EmployeeTypeId");
            return View();
        }

        // POST: /HumanResources/JobTitle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="RecordId,JobTitleId,JobTitleName")] HRJobTitle hrjobtitle)
        {
            if (ModelState.IsValid)
            {
                hrjobtitle.RecordId = Guid.NewGuid();
                db.JobTitles.Add(hrjobtitle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RecordId = new SelectList(db.EmployeeTypes, "RecordId", "EmployeeTypeId", hrjobtitle.RecordId);
            return View(hrjobtitle);
        }

        // GET: /HumanResources/JobTitle/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRJobTitle hrjobtitle = db.JobTitles.Find(id);
            if (hrjobtitle == null)
            {
                return HttpNotFound();
            }
            ViewBag.RecordId = new SelectList(db.EmployeeTypes, "RecordId", "EmployeeTypeId", hrjobtitle.RecordId);
            return View(hrjobtitle);
        }

        // POST: /HumanResources/JobTitle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="RecordId,JobTitleId,JobTitleName")] HRJobTitle hrjobtitle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hrjobtitle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RecordId = new SelectList(db.EmployeeTypes, "RecordId", "EmployeeTypeId", hrjobtitle.RecordId);
            return View(hrjobtitle);
        }

        // GET: /HumanResources/JobTitle/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRJobTitle hrjobtitle = db.JobTitles.Find(id);
            if (hrjobtitle == null)
            {
                return HttpNotFound();
            }
            return View(hrjobtitle);
        }

        // POST: /HumanResources/JobTitle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HRJobTitle hrjobtitle = db.JobTitles.Find(id);
            db.JobTitles.Remove(hrjobtitle);
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
