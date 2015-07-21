using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Anvil.v2015.v001.Domain.Exceptions;
using Anvil.v2015.v001.Domain.Entities;
using LCPS.v2015.v001.NwUsers.Filters;

namespace LCPS.v2015.v001.WebUI.Areas.My.Models
{
    public class MemberFilterModel : MemberFilter
    {
        public MemberFilterModel(MemberFilter f)
        {
            AnvilEntity e = new AnvilEntity(f);
            e.CopyTo(this);
        }

        public AnvilExceptionModel Exception { get; set; }

        public List<StaffFilterClause> GetClauses()
        {
            LCPS.v2015.v001.NwUsers.Infrastructure.LcpsDbContext db = new NwUsers.Infrastructure.LcpsDbContext();
            List<StaffFilterClause> cc = db.StaffFilterClauses
                .Where(x => x.FilterId.Equals(FilterId))
                .OrderBy(x => x.SortIndex)
                .ToList();

            return cc;
        }
    }
}