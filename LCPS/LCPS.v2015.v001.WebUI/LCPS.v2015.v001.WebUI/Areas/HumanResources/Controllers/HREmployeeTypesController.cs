﻿using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.WebUI.Areas.Import.Models;
using LCPS.v2015.v001.WebUI.Infrastructure;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    [LcpsControllerAuthorization(Area = "HumanResources", Controller = "EmployeeTypes")]
    public class HREmployeeTypesController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();

        #region Import

        public ActionResult ImportFile()
        {
            ImportSession i = new HREmployeeTypeSession().ToImportSession();

            return View("~/Areas/Import/Views/ImportFile.cshtml", i);
        }

        [HttpPost]
        public ActionResult Preview(ImportSession s)
        {
            try
            {
                HREmployeeTypeSession ss = new HREmployeeTypeSession();
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
                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Parse Import File", "HumanResources", "HRStaff", "Index"));
            }
        }


        [HttpPost]
        public ActionResult Import(ImportSession s)
        {
            try
            {
                ImportSession dbs = db.ImportSessions.First(x => x.SessionId.Equals(s.SessionId));
                HREmployeeTypeSession jt = new HREmployeeTypeSession(dbs);
                jt.Import();
                ImportPreviewModel m = new ImportPreviewModel(s.SessionId);
                return View("~/Areas/Import/Views/Import.cshtml", m);
            }
            catch (Exception ex)
            {
                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Import Staff", "HumanResources", "HRStaff", "ImportFile"));
            }
        }

        #endregion


        #region Crud
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
