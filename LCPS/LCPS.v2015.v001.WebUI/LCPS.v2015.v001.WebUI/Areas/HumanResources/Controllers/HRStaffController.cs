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
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    public class HRStaffController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();

        // GET: /HumanResources/HRStaff/
        public ActionResult Index()
        {
            return View(db.StaffMembers.OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial));
        }

        // GET: /HumanResources/HRStaff/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRStaff hrstaff = db.StaffMembers.Find(id);
            if (hrstaff == null)
            {
                return HttpNotFound();
            }
            return View(hrstaff);
        }

        // GET: /HumanResources/HRStaff/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /HumanResources/HRStaff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="StaffLinkId,StaffId,PositionCaptions,FirstName,MiddleInitial,LastName,Gender,Birthdate")] HRStaff hrstaff)
        {
            if (ModelState.IsValid)
            {
                hrstaff.StaffLinkId = Guid.NewGuid();
                db.StaffMembers.Add(hrstaff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hrstaff);
        }

        // GET: /HumanResources/HRStaff/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRStaff hrstaff = db.StaffMembers.Find(id);
            if (hrstaff == null)
            {
                return HttpNotFound();
            }
            return View(hrstaff);
        }

        // POST: /HumanResources/HRStaff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="StaffLinkId,StaffId,PositionCaptions,FirstName,MiddleInitial,LastName,Gender,Birthdate")] HRStaff hrstaff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hrstaff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hrstaff);
        }

        // GET: /HumanResources/HRStaff/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRStaff hrstaff = db.StaffMembers.Find(id);
            if (hrstaff == null)
            {
                return HttpNotFound();
            }
            return View(hrstaff);
        }

        // POST: /HumanResources/HRStaff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HRStaff hrstaff = db.StaffMembers.Find(id);
            db.StaffMembers.Remove(hrstaff);
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
