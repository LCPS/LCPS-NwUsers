using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.WebUI.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    [Authorize(Roles = "APP-Admins,HR-Managers")]
    public class BuildingController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();

        // GET: /HumanResources/Building/
        public ActionResult Index()
        {
            return View(db.Buildings.ToList());
        }

        // GET: /HumanResources/Building/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRBuilding hrbuilding = db.Buildings.Find(id);
            if (hrbuilding == null)
            {
                return HttpNotFound();
            }
            return View(hrbuilding);
        }

        // GET: /HumanResources/Building/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /HumanResources/Building/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="BuildingKey,BuildingId,Name")] HRBuilding hrbuilding)
        {
            if (ModelState.IsValid)
            {
                hrbuilding.BuildingKey = Guid.NewGuid();
                db.Buildings.Add(hrbuilding);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hrbuilding);
        }

        // GET: /HumanResources/Building/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRBuilding hrbuilding = db.Buildings.Find(id);
            if (hrbuilding == null)
            {
                return HttpNotFound();
            }
            return View(hrbuilding);
        }

        // POST: /HumanResources/Building/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="BuildingKey,BuildingId,Name")] HRBuilding hrbuilding)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hrbuilding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hrbuilding);
        }

        // GET: /HumanResources/Building/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRBuilding hrbuilding = db.Buildings.Find(id);
            if (hrbuilding == null)
            {
                return HttpNotFound();
            }
            return View(hrbuilding);
        }

        // POST: /HumanResources/Building/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HRBuilding hrbuilding = db.Buildings.Find(id);
            db.Buildings.Remove(hrbuilding);
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
