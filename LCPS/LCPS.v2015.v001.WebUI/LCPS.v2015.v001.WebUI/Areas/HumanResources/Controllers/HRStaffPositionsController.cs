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
using System.IO;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;
using LCPS.v2015.v001.NwUsers.HumanResources.DynamicGroups;
using LCPS.v2015.v001.WebUI.Areas.Import.Models;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;


using PagedList;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    public class HRStaffPositionsController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();

        #region Import

        public ActionResult ImportFile()
        {
            ImportSession s = new HRStaffPositionSession().ToImportSession();

            return View("~/Areas/Import/Views/ImportFile.cshtml", s);
        }

        [HttpPost]
        public ActionResult Preview(ImportSession s)
        {
            try
            {


                HRStaffPositionSession jt = new HRStaffPositionSession();
                jt.SessionId = s.SessionId;

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
                HRStaffPositionSession jt = new HRStaffPositionSession(dbs);
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


        // GET: HumanResources/HRStaffPositions
        public ActionResult Index(int? page, int? pageSize)
        {

            if (pageSize == null)
                pageSize = 12;

            StaffPositionModel m = new StaffPositionModel(page, pageSize);

            ViewBag.Total = m.Positions.Count();


            int pageNumber = (page ?? 1);
            return View(m);
        }



        [HttpPost]
        public ActionResult Filter(HRStaffPositionFilter filter, int? page, int? pageSize)
        {
            Session["staffFilter"] = filter;

            if (pageSize == null)
                pageSize = 12;
            
            StaffPositionModel m = new StaffPositionModel(page, pageSize, filter);

            ViewBag.Total = m.Positions.Count();


            int pageNumber = (page ?? 1);
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Filter(int? page, int? pageSize)
        {

            HRStaffPositionFilter filter = (HRStaffPositionFilter)Session["StaffFilter"];

            if (pageSize == null)
                pageSize = 12;

            StaffPositionModel m = new StaffPositionModel(page, pageSize, filter);

            
            return View(m);

        }

        // GET: HumanResources/HRStaffPositions/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRStaffPosition hRStaffPosition = db.StaffPositions.Find(id);
            if (hRStaffPosition == null)
            {
                return HttpNotFound();
            }
            return View(hRStaffPosition);
        }

        // GET: HumanResources/HRStaffPositions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HumanResources/HRStaffPositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PositionKey,StaffMemberId,BuildingKey,EmployeeTypeKey,JobTitleKey,Active,FiscalYear")] HRStaffPosition hRStaffPosition)
        {
            if (ModelState.IsValid)
            {
                hRStaffPosition.PositionKey = Guid.NewGuid();
                db.StaffPositions.Add(hRStaffPosition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hRStaffPosition);
        }

        // GET: HumanResources/HRStaffPositions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRStaffPosition hRStaffPosition = db.StaffPositions.Find(id);
            if (hRStaffPosition == null)
            {
                return HttpNotFound();
            }
            return View(hRStaffPosition);
        }

        // POST: HumanResources/HRStaffPositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PositionKey,StaffMemberId,BuildingKey,EmployeeTypeKey,JobTitleKey,Active,FiscalYear")] HRStaffPosition hRStaffPosition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hRStaffPosition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hRStaffPosition);
        }

        // GET: HumanResources/HRStaffPositions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRStaffPosition hRStaffPosition = db.StaffPositions.Find(id);
            if (hRStaffPosition == null)
            {
                return HttpNotFound();
            }
            return View(hRStaffPosition);
        }

        // POST: HumanResources/HRStaffPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HRStaffPosition hRStaffPosition = db.StaffPositions.Find(id);
            db.StaffPositions.Remove(hRStaffPosition);
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
