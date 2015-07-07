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
    public class HREmployeeTypesController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();

        // GET: HumanResources/HREmployeeTypes
        public ActionResult Index()
        {
            return View(db.EmployeeTypes.ToList());
        }

        // GET: HumanResources/HREmployeeTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HREmployeeType hREmployeeType = db.EmployeeTypes.Find(id);
            if (hREmployeeType == null)
            {
                return HttpNotFound();
            }
            return View(hREmployeeType);
        }

        // GET: HumanResources/HREmployeeTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HumanResources/HREmployeeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeTypeLinkId,EmployeeTypeId,EmployeeTypeName,Category")] HREmployeeType hREmployeeType)
        {
            if (ModelState.IsValid)
            {
                hREmployeeType.EmployeeTypeLinkId = Guid.NewGuid();
                db.EmployeeTypes.Add(hREmployeeType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hREmployeeType);
        }

        // GET: HumanResources/HREmployeeTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HREmployeeType hREmployeeType = db.EmployeeTypes.Find(id);
            if (hREmployeeType == null)
            {
                return HttpNotFound();
            }
            return View(hREmployeeType);
        }

        // POST: HumanResources/HREmployeeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeTypeLinkId,EmployeeTypeId,EmployeeTypeName,Category")] HREmployeeType hREmployeeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hREmployeeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hREmployeeType);
        }

        // GET: HumanResources/HREmployeeTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HREmployeeType hREmployeeType = db.EmployeeTypes.Find(id);
            if (hREmployeeType == null)
            {
                return HttpNotFound();
            }
            return View(hREmployeeType);
        }

        // POST: HumanResources/HREmployeeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HREmployeeType hREmployeeType = db.EmployeeTypes.Find(id);
            db.EmployeeTypes.Remove(hREmployeeType);
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
