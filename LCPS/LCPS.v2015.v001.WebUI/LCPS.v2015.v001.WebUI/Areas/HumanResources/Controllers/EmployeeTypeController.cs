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
    public class EmployeeTypeController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();

        // GET: /HumanResources/EmployeeType/
        public ActionResult Index()
        {
            return View(db.EmployeeTypes.ToList().OrderBy(x => x.EmployeeTypeId));
        }

        // GET: /HumanResources/EmployeeType/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HREmployeeType hremployeetype = db.EmployeeTypes.Find(id);
            if (hremployeetype == null)
            {
                return HttpNotFound();
            }
            return View(hremployeetype);
        }

        // GET: /HumanResources/EmployeeType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /HumanResources/EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EmployeeTypeLinkId,EmployeeTypeId,EmployeeTypeName,Category")] HREmployeeType hremployeetype)
        {
            if (ModelState.IsValid)
            {
                hremployeetype.EmployeeTypeLinkId = Guid.NewGuid();
                db.EmployeeTypes.Add(hremployeetype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hremployeetype);
        }

        // GET: /HumanResources/EmployeeType/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HREmployeeType hremployeetype = db.EmployeeTypes.Find(id);
            if (hremployeetype == null)
            {
                return HttpNotFound();
            }
            return View(hremployeetype);
        }

        // POST: /HumanResources/EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="EmployeeTypeLinkId,EmployeeTypeId,EmployeeTypeName,Category")] HREmployeeType hremployeetype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hremployeetype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hremployeetype);
        }

        // GET: /HumanResources/EmployeeType/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HREmployeeType hremployeetype = db.EmployeeTypes.Find(id);
            if (hremployeetype == null)
            {
                return HttpNotFound();
            }
            return View(hremployeetype);
        }

        // POST: /HumanResources/EmployeeType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HREmployeeType hremployeetype = db.EmployeeTypes.Find(id);
            db.EmployeeTypes.Remove(hremployeetype);
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
