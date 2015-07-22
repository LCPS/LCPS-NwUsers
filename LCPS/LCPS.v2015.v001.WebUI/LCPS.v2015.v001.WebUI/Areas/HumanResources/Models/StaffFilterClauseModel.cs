using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Anvil.v2015.v001.Domain.Html;
using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Models
{
    public class StaffFilterClauseModel : StaffFilterClause, IAnvilFormHandler
    {
        #region Constructors

        public StaffFilterClauseModel()
        { }

        public StaffFilterClauseModel(StaffFilterClause c)
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

        public StaffFilterClause ToFilterClause()
        {
            AnvilEntity e = new AnvilEntity(this);
            StaffFilterClause c = new StaffFilterClause();
            e.CopyTo(c);
            return c;
        }

        public int GetClauseCount(Guid filterId)
        {
            LcpsDbContext db = new LcpsDbContext();
            List<StaffFilterClause> cc = db.StaffFilterClauses
                .Where(x => x.FilterId.Equals(filterId))
                .OrderBy(x => x.SortIndex)
                .ToList();

            int count = cc.Count();

            return count;
        }

        #endregion
    }
}