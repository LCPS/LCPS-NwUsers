﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.WebUI.Infrastructure;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;
using LCPS.v2015.v001.WebUI.Areas.Import.Models;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    [Authorize(Roles = "APP-Admins,HR-Admins")]
    public class HRStaffController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();



        #region Importing

        public ActionResult ImportFile()
        {
            ImportSession s = new ImportSession
                (
                    area: "HumanResources",
                    controller: "HRStaff",
                    action: "Preview",
                    itemType: typeof(HRStaffCandidate),
                    viewLayoutPath: "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml",
                    addIfNotExist: true,
                    updateIfExists: false,
                    viewTitle: "Import Staff Members"
                );

            return View("~/Areas/Import/Views/ImportFile.cshtml", s);
        }

        [HttpPost]
        public ActionResult Preview(ImportSession s)
        {
            try
            {
                HRStaffSession ss = new HRStaffSession();

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
                HRStaffSession jt = new HRStaffSession(dbs);
                jt.Import();
                ImportPreviewModel m = new ImportPreviewModel(s.SessionId);
                return View("~/Areas/Import/Views/Import.cshtml", m);
            }
            catch (Exception ex)
            {
                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Import Job Titles", "HumanResources", "HRStaff", "Preview"));
            }
        }

        #endregion


        #region Crud

        // GET: /HumanResources/HRStaff/
        public ActionResult Index()
        {
            return View(db.StaffMembers.OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial).ToList());
        }

        [HttpPost]
        public ActionResult StaffSearch(StaffSearchModel m)
        {
            List<HRStaff> staff = db.StaffMembers.Where
                (
                    x => x.LastName.Contains(m.SearchString) |
                        x.FirstName.Contains(m.SearchString) |
                        x.StaffEmail.Contains(m.SearchString) |
                        x.StaffId.Contains(m.SearchString)
                )
                .OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial).ToList();

            return View("Index", staff);
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
        public ActionResult Create([Bind(Include = "StaffKey,StaffId,FirstName,MiddleInitial,LastName,StaffEmail,Gender,Birthdate")] HRStaff hrstaff)
        {
            if (ModelState.IsValid)
            {
                hrstaff.StaffKey = Guid.NewGuid();
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
        public ActionResult Edit([Bind(Include = "StaffKey,StaffId,FirstName,MiddleInitial,LastName,StaffEmail,Gender,Birthdate")] HRStaff hrstaff)
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
