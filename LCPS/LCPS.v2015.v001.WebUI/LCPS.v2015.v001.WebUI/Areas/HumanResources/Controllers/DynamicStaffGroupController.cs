using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.NwUsers.HumanResources.DynamicGroups;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;

using Anvil.v2015.v001.Domain.Exceptions;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    public class DynamicStaffGroupController : Controller
    {

        LcpsDbContext db = new LcpsDbContext();

        //
        // GET: /HumanResources/DynamicStaffGroup/
        public ActionResult Index()
        {
            List<DynamicStaffGroup> Groups = db.DynamicStaffGroups.OrderBy(x => x.GroupName).ToList();

            DynamicStaffGroupModel m = new DynamicStaffGroupModel()
            {
                StaffGroups = Groups,
                CurrentGroup = null
            };

            return View(m);
        }

        public ActionResult DynamicGroup(Guid id)
        {
            try
            {
                DynamicStaffGroup g = db.DynamicStaffGroups.First(x => x.DynamicGroupId.Equals(id));
                DynamicStaffGroupModel m = new DynamicStaffGroupModel();
                m.StaffGroups = db.DynamicStaffGroups.OrderBy(x => x.GroupName).ToList();
                m.Clauses = this.GetClauses(id);
                m.CurrentGroup = g;

                return View("Index", m);
            }
            catch (Exception ex)
            {
                return View("Error", new AnvilExceptionModel(ex, "Create Staff Group", "HumanResources", "DynamicStaffGroup", "Index"));
            }
        }

        [HttpPost]
        public ActionResult CreateStaffGroup(DynamicStaffGroup g)
        {
            try
            {
                DynamicStaffGroupModel m = new DynamicStaffGroupModel();

                if (ModelState.IsValid)
                {
                    g.DynamicGroupId = Guid.NewGuid();
                    db.DynamicStaffGroups.Add(g);
                    db.SaveChanges();

                    m = new DynamicStaffGroupModel(g.DynamicGroupId);
                }
                else
                    m.CurrentGroup = null;

                return View("Index", m);
            }
            catch (Exception ex)
            {
                return View("Error", new AnvilExceptionModel(ex, "Create Staff Group", "HumanResources", "DynamicStaffGroup", "Index"));
            }
        }

        [HttpPost]
        public ActionResult CreateStaffClause(StaffClauseGroup g)
        {
            try
            {
                
                DynamicStaffGroupModel m = new DynamicStaffGroupModel();
                m.StaffGroups = db.DynamicStaffGroups.OrderBy(x => x.GroupName).ToList();
                m.CurrentGroup = db.DynamicStaffGroups.First(x => x.DynamicGroupId.Equals(g.StaffGroupId));

                /*
                if (g.GroupConjunction == Anvil.v2015.v001.Domain.Entities.DynamicFilters.DynamicQueryConjunctions.None)
                    ModelState.AddModelError("", "The Group Conjunction must be 'AND' or 'OR'");

                if (g.BuildingConjunction == Anvil.v2015.v001.Domain.Entities.DynamicFilters.DynamicQueryConjunctions.None &
                    g.EmployeeTypeConjunction == Anvil.v2015.v001.Domain.Entities.DynamicFilters.DynamicQueryConjunctions.None &
                    g.JobTitleConjunction == Anvil.v2015.v001.Domain.Entities.DynamicFilters.DynamicQueryConjunctions.None &
                    g.StatusConjunction == Anvil.v2015.v001.Domain.Entities.DynamicFilters.DynamicQueryConjunctions.None &
                    g.YearConjunction == Anvil.v2015.v001.Domain.Entities.DynamicFilters.DynamicQueryConjunctions.None)

                    ModelState.AddModelError("", "At least one item must have a conjuction that is not set to 'None'");

                */
                if (ModelState.IsValid)
                {

                    g.SortIndex = db.DynamicStaffClauses.Where(x => x.StaffGroupId.Equals(g.StaffGroupId)).Count();
                    g.RecordId = Guid.NewGuid();
                    db.DynamicStaffClauses.Add(g);
                    db.SaveChanges();
                }

                m.Clauses = GetClauses(g.StaffGroupId);
                return View("Index", m);
            }
            catch (Exception ex)
            { return View("Error", new AnvilExceptionModel(ex, "Create Staff Group", "HumanResources", "DynamicStaffGroup", "Index")); }
        }

        public ActionResult RemoveStaffClause(Guid id)
        {
            try
            {
                StaffClauseGroup g = db.DynamicStaffClauses.First(x => x.RecordId.Equals(id));
                db.DynamicStaffClauses.Remove(g);
                db.SaveChanges();

                DynamicStaffGroupModel m = new DynamicStaffGroupModel(g.StaffGroupId);
                return View("Index", m);
            }
            catch(Exception ex)
            { 
                return View("Error", new AnvilExceptionModel(ex, "Create Staff Group", "HumanResources", "DynamicStaffGroup", "Index")); 
            }
        }

        private List<StaffClauseGroup> GetClauses(Guid id)
        {
           return  db.DynamicStaffClauses.Where(x => x.StaffGroupId.Equals(id)).OrderBy(x => x.SortIndex).ToList();
        }
    }
}