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
    [Authorize(Roles = "APP-Admins,HR-Admins")]
    public class HRBuildingsController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();


        #region CRUD

        // GET: HumanResources/HRBuildings
        public ActionResult Index()
        {
            return View(db.Buildings.ToList());
        }

        // GET: HumanResources/HRBuildings/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRBuilding hRBuilding = db.Buildings.Find(id);
            if (hRBuilding == null)
            {
                return HttpNotFound();
            }
            return View(hRBuilding);
        }

        // GET: HumanResources/HRBuildings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HumanResources/HRBuildings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BuildingKey,BuildingId,Name")] HRBuilding hRBuilding)
        {
            if (ModelState.IsValid)
            {
                hRBuilding.BuildingKey = Guid.NewGuid();
                db.Buildings.Add(hRBuilding);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hRBuilding);
        }

        // GET: HumanResources/HRBuildings/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRBuilding hRBuilding = db.Buildings.Find(id);
            if (hRBuilding == null)
            {
                return HttpNotFound();
            }
            return View(hRBuilding);
        }

        // POST: HumanResources/HRBuildings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BuildingKey,BuildingId,Name")] HRBuilding hRBuilding)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRBuilding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hRBuilding);
        }

        // GET: HumanResources/HRBuildings/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRBuilding hRBuilding = db.Buildings.Find(id);
            if (hRBuilding == null)
            {
                return HttpNotFound();
            }
            return View(hRBuilding);
        }

        // POST: HumanResources/HRBuildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HRBuilding hRBuilding = db.Buildings.Find(id);
            db.Buildings.Remove(hRBuilding);
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
