using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Anvil.v2015.v001.Domain.Html;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates;
using LCPS.v2015.v001.NwUsers.LcpsLdap.Servers;
using LCPS.v2015.v001.WebUI.Infrastructure;
using System.Management;
using Anvil.v2015.v001.Domain.Exceptions;
using Anvil.v2015.v001.Domain.Entities;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.NwUsers.LcpsComputers;
using LCPS.v2015.v001.NwUsers.LcpsComputers.IO;
using LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;
using LCPS.v2015.v001.WebUI.Areas.Students.Models;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Controllers
{
    public class HomeFolderController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();

        // GET: /LcpsLdap/HomeFolder/
        public ActionResult Index()
        {
            return View(db.HomeFolderTemplates.ToList());
        }

        // GET: /LcpsLdap/HomeFolder/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeFolderTemplate homefoldertemplate = db.HomeFolderTemplates.Find(id);
            if (homefoldertemplate == null)
            {
                return HttpNotFound();
            }
            return View(homefoldertemplate);
        }

        // GET: /LcpsLdap/HomeFolder/Create
        public ActionResult Create(string server)
        {
            return View();
        }

        // POST: /LcpsLdap/HomeFolder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HomeFolderId,TemplateName,Description,HomeFoldePath,IdField")] HomeFolderTemplate homefoldertemplate)
        {
            if (ModelState.IsValid)
            {
                homefoldertemplate.HomeFolderId = Guid.NewGuid();
                db.HomeFolderTemplates.Add(homefoldertemplate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(homefoldertemplate);
        }

        // GET: /LcpsLdap/HomeFolder/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeFolderTemplate homefoldertemplate = db.HomeFolderTemplates.Find(id);
            if (homefoldertemplate == null)
            {
                return HttpNotFound();
            }
            return View(homefoldertemplate);
        }

        // POST: /LcpsLdap/HomeFolder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="HomeFolderId,TemplateName,Description,HomeFoldePath,IdField")] HomeFolderTemplate homefoldertemplate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(homefoldertemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(homefoldertemplate);
        }

        // GET: /LcpsLdap/HomeFolder/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeFolderTemplate homefoldertemplate = db.HomeFolderTemplates.Find(id);
            if (homefoldertemplate == null)
            {
                return HttpNotFound();
            }
            return View(homefoldertemplate);
        }

        // POST: /LcpsLdap/HomeFolder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HomeFolderTemplate homefoldertemplate = db.HomeFolderTemplates.Find(id);
            db.HomeFolderTemplates.Remove(homefoldertemplate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SelectHomeFolder(string server)
        {
            if(server != null)
            {
                return RedirectToAction("Create", new { server = server });
            }

            return View("Create");
        }

        [HttpGet]
        public ActionResult GetShares(string server)
        {
            string result = "";

            try
            {
                SharedFolderTree n = new SharedFolderTree(server);
                n.Recursive = false;

                n.Refresh();
                result = "<div class='tree'>" + n.ToUL() + "</div>";
            }
            catch(Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                result = ec.ToUL();
            }

            return Content(result);
        }

        public ActionResult StaffMembership(Guid id)
        {
            LcpsDbContext db = new LcpsDbContext();
            HomeFolderTemplateModel m = new HomeFolderTemplateModel(db, id);
            return View(m);
        }

        public ActionResult AddStaffClause(StaffFilterClauseModel m)
        {
            DynamicStaffFilter f = new DynamicStaffFilter(m.FilterId);
            f.CreateClause(m.ToFilterClause());
            return RedirectToAction("StaffMembership", new { id = m.FilterId });
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
