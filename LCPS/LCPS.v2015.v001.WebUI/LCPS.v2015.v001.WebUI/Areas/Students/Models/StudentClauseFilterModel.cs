using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using Anvil.v2015.v001.Domain.Html;
using LCPS.v2015.v001.NwUsers.Students;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using System.ComponentModel.DataAnnotations;
using LCPS.v2015.v001.NwUsers.Filters;

namespace LCPS.v2015.v001.WebUI.Areas.Students.Models
{
    public class StudentClauseFilterModel : StudentFilterClause, IAnvilFormHandler
    {
        #region Constructors

        public StudentClauseFilterModel()
        {
        }

        public StudentClauseFilterModel(StudentFilterClause c)
        {
            AnvilEntity e = new AnvilEntity(c);
            e.CopyTo(this);
        }

        #endregion

        #region Properties

        public string FormArea { get; set; }
        public string FormController { get; set; }
        public string FormAction { get; set; }
        public string SubmitText { get; set; }
        public string OnErrorActionName { get; set; }



        #endregion

        #region Conversions

        public StudentFilterClause ToFilterClause()
        {
            AnvilEntity e = new AnvilEntity(this);
            StudentFilterClause c = new StudentFilterClause();
            e.CopyTo(c);
            return c;
        }

        public int GetClauseCount(Guid filterId)
        {
            LcpsDbContext db = new LcpsDbContext();
            List<StudentFilterClause> cc = db.StudentFilterClauses
                .Where(x => x.FilterId.Equals(filterId))
                .OrderBy(x => x.SortIndex)
                .ToList();

            int count = cc.Count();

            return count;
        }

        #endregion


        
    }
}