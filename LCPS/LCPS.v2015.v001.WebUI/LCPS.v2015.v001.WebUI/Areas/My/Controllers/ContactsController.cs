using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using Anvil.v2015.v001.Domain.Exceptions;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.NwUsers.Students;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.WebUI.Areas.My.Models;
using LCPS.v2015.v001.WebUI.Areas.Students.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using PagedList;

namespace LCPS.v2015.v001.WebUI.Areas.My.Controllers
{
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
                    AnvilExceptionModel em = new AnvilExceptionModel(ex, "Edit Staff Filter", "My", "Contacts", "Index");
                    return View("Error", em);
                }
            }

            if (f.FilterClass == MemberFilterClass.Staff)
                return RedirectToAction("EditStaffFilter", new { id = f.FilterId });
            else
                return RedirectToAction("EditStudentFilter", new { id = f.FilterId });
        }

        #endregion

        #region Staff

        public ActionResult DeleteStaffFilter(Guid id)
        {
            MemberFilter mf = DbContext.MemberFilters.Find(id);
            DbContext.MemberFilters.Remove(mf);
            DbContext.SaveChanges();
            return RedirectToAction("Index", new { c = MemberFilterClass.Staff });
        }

        public ActionResult StaffList(Guid id, int? page, int? pageSize)
        {
            try
            {
                MemberFilter f = DbContext.MemberFilters.Find(id);

                ViewBag.FilterId = id;
                ViewBag.FilterTitle = f.Title;

                List<StaffFilterClause> cc = DbContext.StaffFilterClauses
                    .Where(x => x.FilterId.Equals(id))
                    .OrderBy(x => x.SortIndex)
                    .ToList();

                if (page == null)
                    page = 1;

                if (pageSize == null)
                    pageSize = 12;

                DynamicQueryStatement dqs = f.ToDynamicQueryStatement();

                List<HRStaffRecord> rr = HRStaffRecord.GetList(dqs);

                PagedList<HRStaffRecord> pl = new PagedList<HRStaffRecord>(rr, page.Value, pageSize.Value);

                return View(pl);
            }
            catch (Exception ex)
            {
                AnvilExceptionModel errM = new AnvilExceptionModel(ex, "Get Staff Filter List", "My", "Contacts", "Index");
                return View("Error", errM);
            }



        }

        public ActionResult EditStaffFilter(Guid id)
        {
            MemberFilter f = DbContext.MemberFilters.Find(id);
            MyContactModel m = this.Model;
            m.CurrentFilter = f;
            this.Model = m;
            return View(new MemberFilterModel(f));
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
                m.Filter = f;
            }
            catch
            {
                ModelState.AddModelError("", "Invalid filter id");
            }


            int count = DbContext.StaffFilterClauses.Where(x => x.FilterId.Equals(m.FilterId)).Count();
            if (count == 0)
            {
                DynamicQueryClause dqc = m.ToDynamicQueryClause();

                int idx = dqc.Where(x => x.Conjunction != Anvil.v2015.v001.Domain.Entities.DynamicFilters.DynamicQueryConjunctions.None).Count();

                if (dqc.Count() > 1 & idx == 0)
                    ModelState.AddModelError("", "At least one item must have a valid conjunction");
            }
            else
            {
                if (m.ClauseConjunction == DynamicQueryConjunctions.None)
                    ModelState.AddModelError("", "Please select a clause conjunction");
            }

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

                return View("EditStaffFilter", fm);
            }

            return RedirectToAction("EditStaffFilter", new { id = m.FilterId });

        }

        public ActionResult DeleteStaffClause(Guid id)
        {
            StaffFilterClause c = DbContext.StaffFilterClauses.Find(id);
            Guid fid = c.FilterId;
            DbContext.StaffFilterClauses.Remove(c);
            DbContext.SaveChanges();

            StaffFilterClause cf = DbContext.StaffFilterClauses.Where(x => x.FilterId.Equals(fid)).First();
            cf.SortIndex = 0;
            cf.ClauseConjunction = DynamicQueryConjunctions.None;
            DbContext.Entry(cf).State = System.Data.Entity.EntityState.Modified;
            DbContext.SaveChanges();

            int idx = 1;
            List<StaffFilterClause> cc = DbContext.StaffFilterClauses.Where(x => x.FilterId.Equals(fid) & !x.StaffFilterClauseId.Equals(cf.StaffFilterClauseId)).OrderBy(x => x.SortIndex).ToList();
            foreach (StaffFilterClause ci in cc)
            {
                ci.SortIndex = idx;
                DbContext.Entry(ci).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
                idx++;
            }


            return RedirectToAction("EditStaffFilter", new { id = fid });
        }

        #endregion

        #region Students

        public ActionResult EditStudentFilter(Guid id)
        {
            StudentFilterModel m = new StudentFilterModel(id, "My", "Contacts", "CreateStudentClause", "Add Clause", "Index");
            return View(m);
        }

        public ActionResult CreateStudentClause(StudentClauseFilterModel m)
        {
            try
            {
                DynamicStudentFilter f = new DynamicStudentFilter(m.FilterId);
                f.CreateClause(m.ToFilterClause());
                f.Refresh();

                return RedirectToAction("EditStudentFilter", new { id = m.FilterId });
            }
            catch (Exception ex)
            {
                AnvilExceptionModel em = new AnvilExceptionModel(ex, "Create Student Clause", m.FormArea, m.FormController, "Index");

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

    }
}