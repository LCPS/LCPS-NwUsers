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

        public List<HRStaffPosition> Staff()
        {
            try
            { 
                LcpsDbContext db = new LcpsDbContext();
                List<HRStaffPosition> positions = db.StaffPositions.Where(CurrentGroup.ToString()).ToList();
                positions = positions.OrderBy(x => x.StaffMember.LastName + x.StaffMember.FirstName + x.StaffMember.MiddleInitial).ToList();
                return positions;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get related staff", ex);
            }
        }
    }
}