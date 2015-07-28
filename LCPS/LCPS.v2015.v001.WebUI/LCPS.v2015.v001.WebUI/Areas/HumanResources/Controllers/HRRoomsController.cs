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
    public class HRRoomsController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();

        // GET: HumanResources/HRRooms
        public ActionResult Index(Guid? id)
        {
            if (id == null)
                return View(db.Rooms.ToList());
            else
            {
                List<HRRoom> rooms = db.Rooms.Where(x => x.BuildingId.Equals(id.Value)).ToList();
                return View(rooms);
            }
                
            
        }

        // GET: HumanResources/HRRooms/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRRoom hRRoom = db.Rooms.Find(id);
            if (hRRoom == null)
            {
                return HttpNotFound();
            }
            return View(hRRoom);
        }

        // GET: HumanResources/HRRooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HumanResources/HRRooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomKey,BuildingId,RoomNumber,RoomId,RoomType,PrimaryOccupant")] HRRoom hRRoom)
        {
            if (ModelState.IsValid)
            {
                hRRoom.RoomKey = Guid.NewGuid();
                db.Rooms.Add(hRRoom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hRRoom);
        }

        // GET: HumanResources/HRRooms/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRRoom hRRoom = db.Rooms.Find(id);
            if (hRRoom == null)
            {
                return HttpNotFound();
            }
            return View(hRRoom);
        }

        // POST: HumanResources/HRRooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomKey,BuildingId,RoomNumber,RoomId,RoomType,PrimaryOccupant")] HRRoom hRRoom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRRoom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hRRoom);
        }

        // GET: HumanResources/HRRooms/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRRoom hRRoom = db.Rooms.Find(id);
            if (hRRoom == null)
            {
                return HttpNotFound();
            }
            return View(hRRoom);
        }

        // POST: HumanResources/HRRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HRRoom hRRoom = db.Rooms.Find(id);
            db.Rooms.Remove(hRRoom);
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
