using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.DynamicGroups;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Models
{
    public class DynamicStaffGroupModel
    {
        public DynamicStaffGroupModel()
        { }

        public DynamicStaffGroupModel(Guid id)
        {
            LcpsDbContext db = new LcpsDbContext();
            this.StaffGroups = db.DynamicStaffGroups.OrderBy(x => x.GroupName).ToList();
            this.CurrentGroup = db.DynamicStaffGroups.First(x => x.DynamicGroupId.Equals(id));
            this.Clauses = db.DynamicStaffClauses.Where(x => x.StaffGroupId.Equals(id)).OrderBy(x => x.SortIndex).ToList();
        }



        public List<DynamicStaffGroup> StaffGroups { get; set; }

        public DynamicStaffGroup CurrentGroup { get; set;}

        public List<StaffClauseGroup> Clauses { get; set; }

    }
}