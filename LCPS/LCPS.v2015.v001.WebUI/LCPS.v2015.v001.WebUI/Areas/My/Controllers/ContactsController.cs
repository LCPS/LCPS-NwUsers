﻿using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Exceptions;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Students;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;
using LCPS.v2015.v001.WebUI.Areas.My.Models;
using LCPS.v2015.v001.WebUI.Areas.Students.Models;
using LCPS.v2015.v001.WebUI.Infrastructure;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LCPS.v2015.v001.WebUI.Areas.My.Controllers 
{
    [LcpsControllerAuthorization(Area = "My", Controller = "Contacts")]
    public class ContactsController : Controller
    {

        #region Fields

        private LcpsDbContext _dbContext;
        private ApplicationUser _user;

        #endregion

        #region Helpers

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();

                return _dbContext;
            }
        }

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_user == null)
                    _user = DbContext.Users.First(x => x.UserName.ToLower() == User.Identity.Name.ToLower());
                return _user;
            }
        }

        public MyContactModel Model
        {
            get
            {
                if (Session["MyContactModel"] == null)
                {
                    MyContactModel m = new MyContactModel();

                    Session["MyContactModel"] = m;

                }

                return (MyContactModel)Session["MyContactModel"];
            }
            set
            {
                Session["MyContactModel"] = value;
            }
        }

        #endregion

        #region Index

        // GET: My/Contacts
        public ActionResult Index(MemberFilterClass? c)
        {
            List<MemberFilter> ff = DbContext.MemberFilters.OrderBy(x => x.Title).ToList();

            if (c != null)
            {
                ff = ff
                    .Where(x => x.FilterClass == c.Value)
                    .OrderBy(x => x.Title)
                    .ToList();
            }


            return View(ff);
        }

        #endregion

        #region Filter

        public ActionResult CreateFilter(MemberFilterClass c)
        {
            MemberFilter f = new MemberFilter()
            {
                FilterClass = c,
                Category = FilterCategories.Contacts,
                AntecedentId = new Guid(CurrentUser.Id),
                FilterId = Guid.NewGuid()
            };

            return View(f);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFilter(MemberFilter m)
        {

            if (ModelState.IsValid)
            {
                DbContext.MemberFilters.Add(m);
                DbContext.SaveChanges();

                MyContactModel cm = this.Model;
                cm.CurrentFilter = m;
                this.Model = cm;

                return View("Index", this.Model.GetFilters());
            }
            return View(m);
        }

        public ActionResult EditFilter(Guid id)
        {
            MemberFilter f = DbContext.MemberFilters.Find(id);

            MemberFilterModel m = new MemberFilterModel(f);
            

            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFilter(MemberFilter f)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Entry(f).State = System.Data.Entity.EntityState.Modified;
                    DbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    AnvilExceptionModel em = new AnvilExceptionModel(ex, "Edit Filter", "My", "Contacts", "Index");
                    return View("Error", em);
                }
            }

            return RedirectToAction("EditFilter", new { id = f.FilterId });
        }

        #endregion

        #region Staff

        public ActionResult DeleteFilter(Guid id)
        {
            MemberFilter mf = DbContext.MemberFilters.Find(id);

            List<StudentFilterClause> sfc = DbContext.StudentFilterClauses.Where(x => x.FilterId.Equals(id)).ToList();

            List<StaffFilterClause> stf = DbContext.StaffFilterClauses.Where(x => x.FilterId.Equals(id)).ToList();

            DbContext.StaffFilterClauses.RemoveRange(stf);
            DbContext.SaveChanges();

            DbContext.StudentFilterClauses.RemoveRange(sfc);

            DbContext.MemberFilters.Remove(mf);
            DbContext.SaveChanges();


            return RedirectToAction("Index", new { c = MemberFilterClass.Staff });
        }

        public ActionResult Preview(Guid id, int? page, int? pageSize)
        {
            MemberFilter f = DbContext.MemberFilters.Find(id);

            ViewBag.FilterId = id;
            ViewBag.FilterTitle = f.Title;

            if (page == null)
                page = 1;

            if (pageSize == null)
                pageSize = 12;


            FilterPreviewModel m = new FilterPreviewModel();

            m.Filter = f;

            if (f.FilterClass == MemberFilterClass.Staff)
            {

                DynamicStaffFilter dsf = new DynamicStaffFilter(id);
                dsf.Refresh();
                List<HRStaffRecord> rr = dsf.Execute();
                ViewBag.Total = rr.Count().ToString();
                PagedList<HRStaffRecord> pl = new PagedList<HRStaffRecord>(rr, page.Value, pageSize.Value);

                m.StaffList = pl;
            }
            else
            {

                DynamicStudentFilter stu = new DynamicStudentFilter(id);
                stu.Refresh();
                List<StudentRecord> ss = stu.Execute();
                ViewBag.Total = ss.Count().ToString();
                PagedList<StudentRecord> pl = new PagedList<StudentRecord>(ss, page.Value, pageSize.Value);

                m.StudentList = pl;

            }

            return View(m);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStaffClause(StaffFilterClauseModel m)
        {
            if (!m.BuildingInclude & !m.EmployeeTypeInclude & !m.JobTitleInclude & !m.StatusInclude & !m.StaffIdInclude & !m.LastNameInclude)
                ModelState.AddModelError("", "Please include at least one field in the filter");

            MemberFilter f = null;

            try
            {
                f = DbContext.MemberFilters.First(x => x.FilterId.Equals(m.FilterId));
            }
            catch
            {
                ModelState.AddModelError("", "Invalid filter id");
            }


            int count = DbContext.StaffFilterClauses.Where(x => x.FilterId.Equals(m.FilterId)).Count();

            bool hasErrors = ViewData.ModelState.Values.Any(x => x.Errors.Count > 1);
            List<ModelState> errors = ViewData.ModelState.Values.Where(x => x.Errors.Count() > 0).ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    StaffFilterClause c = new StaffFilterClause();

                    AnvilEntity e = new AnvilEntity(m);
                    e.CopyTo(c);
                    c.StaffFilterClauseId = Guid.NewGuid();
                    c.SortIndex = count;
                    DbContext.StaffFilterClauses.Add(c);
                    DbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return View("~/Views/Shared/Error.cshtml", new AnvilExceptionModel(ex, "Create Staff Clause", "My", "Contacts", "EditStaffFilter"));
                }
            }
            else
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector("The staff filter clause could not be validated");
                foreach (ModelState ms in errors)
                {
                    foreach (var x in ms.Errors)
                    {
                        ec.Add(x.ErrorMessage);
                    }
                }

                AnvilExceptionModel em = new AnvilExceptionModel(ec.ToException(), "Create Staff Filter Clause", null, null, null);

                MemberFilterModel fm = new MemberFilterModel(f);

                fm.Exception = em;

                return View("EditFilter", fm);
            }

            return RedirectToAction("EditFilter", new { id = m.FilterId });

        }

        public ActionResult DeleteStaffClause(Guid id)
        {
            StaffFilterClause c = DbContext.StaffFilterClauses.Find(id);
            Guid fid = c.FilterId;
            DbContext.StaffFilterClauses.Remove(c);
            DbContext.SaveChanges();

            StaffFilterClause cf = DbContext.StaffFilterClauses.Where(x => x.FilterId.Equals(fid)).FirstOrDefault();
            if (cf != null)
            {
                cf.SortIndex = 0;
                DbContext.Entry(cf).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }

            int idx = 1;
            List<StaffFilterClause> cc = DbContext.StaffFilterClauses.Where(x => x.FilterId.Equals(fid) & !x.StaffFilterClauseId.Equals(cf.StaffFilterClauseId)).OrderBy(x => x.SortIndex).ToList();
            foreach (StaffFilterClause ci in cc)
            {
                ci.SortIndex = idx;
                DbContext.Entry(ci).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
                idx++;
            }


            return RedirectToAction("EditFilter", new { id = fid });
        }

        #endregion

        #region Students


        public ActionResult AddStudentClause(StudentFilterClauseModel m)
        {
            try
            {
                DynamicStudentFilter f = new DynamicStudentFilter(m.FilterId);
                f.CreateClause(m.ToFilterClause());
                f.Refresh();

                return RedirectToAction("EditFilter", new { id = m.FilterId });
            }
            catch (Exception ex)
            {
                AnvilExceptionModel em = new AnvilExceptionModel(ex, "Create Student Clause", m.FormArea, m.FormController, "Index");

                return View("Error", em);
            }
        }

        public ActionResult DeleteStudentClause(Guid id)
        {
            try
            {
                StudentFilterClause c = DbContext.StudentFilterClauses.Find(id);
                Guid fid = c.FilterId;
                DbContext.StudentFilterClauses.Remove(c);
                DbContext.SaveChanges();
                return RedirectToAction("EditFilter", new { id = fid });
            }
            catch (Exception ex)
            {
                AnvilExceptionModel em = new AnvilExceptionModel(ex, "Create Student Clause", "Index", "Contacts", "Index");

                return View("Error", em);
            }
        }

        public ActionResult DeleteStudentFilter(Guid id)
        {
            try
            {
                MemberFilter f = DbContext.MemberFilters.Find(id);

                MemberFilterClass fc = f.FilterClass;

                List<StudentFilterClause> cc = DbContext.StudentFilterClauses
                    .Where(x => x.FilterId.Equals(id))
                    .ToList();

                DbContext.StudentFilterClauses.RemoveRange(cc);
                DbContext.SaveChanges();

                DbContext.MemberFilters.Remove(f);
                DbContext.SaveChanges();

                return RedirectToRoute("Index", new { c = fc });
            }
            catch (Exception ex)
            {
                AnvilExceptionModel em = new AnvilExceptionModel(ex, "Delete Student Filter", "My", "Contacts", "Index");
                return View("Error", em);
            }
        }

        #endregion

        #region StaffQuery

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetEmployeeTypes(Guid? buildingId)
        {

            List<HREmployeeType> ii = new List<HREmployeeType>();
            if (buildingId == null)
                ii = DbContext.EmployeeTypes.Distinct().OrderBy(x => x.EmployeeTypeName).ToList();
            else
                ii = (from HREmployeeType et in DbContext.EmployeeTypes
                      join HRStaffPosition s in DbContext.StaffPositions on et.EmployeeTypeLinkId equals s.EmployeeTypeKey
                      where s.BuildingKey.Equals(buildingId.Value)
                      select et).Distinct().OrderBy(x => x.EmployeeTypeName).ToList();


            var result = (from i in ii
                          select new
                          {
                              id = i.EmployeeTypeLinkId.ToString(),
                              name = i.EmployeeTypeName
                          }).ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetJobTitles(Guid? buildingId, Guid? employeeTypeId)
        {

            List<HRJobTitle> jtt = new List<HRJobTitle>();

            if (buildingId == null & employeeTypeId == null)
                jtt = DbContext.JobTitles.Distinct().OrderBy(x => x.JobTitleName).ToList();

            if (buildingId != null & employeeTypeId == null)
                jtt = (from HRJobTitle jt in DbContext.JobTitles
                       join HRStaffPosition s in DbContext.StaffPositions on jt.JobTitleKey equals s.JobTitleKey
                       where s.BuildingKey.Equals(buildingId.Value)
                       select jt).Distinct().OrderBy(x => x.JobTitleName).ToList();

            if (buildingId == null & employeeTypeId != null)
                jtt = (from HRJobTitle jt in DbContext.JobTitles
                       join HRStaffPosition s in DbContext.StaffPositions on jt.JobTitleKey equals s.JobTitleKey
                       where s.EmployeeTypeKey.Equals(employeeTypeId.Value)
                       select jt).Distinct().OrderBy(x => x.JobTitleName).ToList();

            if (buildingId != null & employeeTypeId != null)
                jtt = (from HRJobTitle jt in DbContext.JobTitles
                       join HRStaffPosition s in DbContext.StaffPositions on jt.JobTitleKey equals s.JobTitleKey
                       where s.EmployeeTypeKey.Equals(employeeTypeId.Value) &
                         s.BuildingKey.Equals(buildingId.Value)
                       select jt).Distinct().OrderBy(x => x.JobTitleName).ToList();


            var result = (from i in jtt
                          select new
                          {
                              id = i.JobTitleKey.ToString(),
                              name = i.JobTitleName
                          }).ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Server

        public ActionResult ServerVars()
        {
            Dictionary<string, string> vars = new Dictionary<string, string>();

            var keys = Request.ServerVariables.Keys;
            foreach(var k in keys)
            {
                vars.Add(k.ToString(), Request.ServerVariables[k.ToString()]);
            }


            return View(vars);
        }

        #endregion

    }
}