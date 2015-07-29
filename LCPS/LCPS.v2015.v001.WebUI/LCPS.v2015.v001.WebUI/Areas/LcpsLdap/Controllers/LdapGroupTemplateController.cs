using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Anvil.v2015.v001.Domain.Exceptions;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates;
using LCPS.v2015.v001.NwUsers.Filters;

using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;
using LCPS.v2015.v001.WebUI.Areas.Students.Models;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Controllers
{
    public class LdapGroupTemplateController : Controller
    {

        #region Fields

        private LcpsDbContext _dbContext;

        #endregion

        #region Properties

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();
                return _dbContext;
            }
        }

        #endregion

        //
        // GET: /LcpsLdap/LdapOuTemplate/
        public ActionResult Index()
        {
            try
            {
                return View(new GroupTemplateViewModel(DbContext));
            }
            catch (Exception ex)
            {
                AnvilExceptionModel em = new AnvilExceptionModel(ex, "Group Templates", "Public", "Home", "Index");
                return View("Error", em);
            }
        }

        [HttpGet]
        public ActionResult CreateGroupTemplate(Guid id)
        {
            try
            {
                LcpsAdsGroup ou = new LcpsAdsGroup(id);
                GroupTemplate t = new GroupTemplate();
                t.GroupId = id;
                t.TemplateName = ou.Name;

                if (string.IsNullOrEmpty(ou.Description))
                    t.Description = "No aescription available";
                else
                    t.Description = ou.Description;

                DbContext.GroupTemplates.Add(t);
                DbContext.SaveChanges();

                return Content("Success", "text/html");
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                return Content(ec.ToUL(), "text/html");
            }
        }

        public ActionResult EditTemplate(Guid id)
        {
            GroupTemplateViewModel m = new GroupTemplateViewModel(DbContext, id);
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTemplate(GroupTemplate m)
        {
            GroupTemplateViewModel oum = new GroupTemplateViewModel(DbContext);

            try
            {
                if (ModelState.IsValid)
                {
                    DbContext.Entry(m).State = System.Data.Entity.EntityState.Modified;
                    DbContext.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                oum.Exception = ex;
                return View(oum);

            }
        }


        public ActionResult StudentMembership(Guid id)
        {
            GroupTemplateViewModel m = new GroupTemplateViewModel(DbContext, id);
            return View(m);
        }

        public ActionResult StaffMembership(Guid id)
        {
            GroupTemplateViewModel m = new GroupTemplateViewModel(DbContext, id);
            return View(m);
        }

        public ActionResult AddStudentClause(StudentFilterClauseModel m)
        {
            DynamicStudentFilter f = new DynamicStudentFilter(m.FilterId);
            f.CreateClause(m.ToFilterClause());
            return RedirectToAction("StudentMembership", new { id = m.FilterId });
        }

        public ActionResult AddStaffClause(StaffFilterClauseModel m)
        {
            DynamicStaffFilter f = new DynamicStaffFilter(m.FilterId);
            f.CreateClause(m.ToFilterClause());
            return RedirectToAction("StaffMembership", new { id = m.FilterId });
        }


        [HttpGet]
        public ActionResult DeleteGroupTemplate(Guid id)
        {
            string result = "The template was successfully deleted";

            try
            {
                
                GroupTemplate t = DbContext.GroupTemplates.Find(id);

                List<StaffFilterClause> stf = DbContext.StaffFilterClauses.Where(x => x.FilterId.Equals(t.GroupId)).ToList();

                List<StudentFilterClause> stu = DbContext.StudentFilterClauses.Where(x => x.FilterId.Equals(t.GroupId)).ToList();


                DbContext.StaffFilterClauses.RemoveRange(stf);
                DbContext.StudentFilterClauses.RemoveRange(stu);

                DbContext.GroupTemplates.Remove(t);

                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                result = ec.ToUL();
            }

            return Content(result, "text/html");

        }
	}
}