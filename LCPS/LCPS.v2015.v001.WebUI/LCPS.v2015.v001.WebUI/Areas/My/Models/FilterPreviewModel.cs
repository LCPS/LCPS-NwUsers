

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Students;
using LCPS.v2015.v001.NwUsers.Filters;
using PagedList;

namespace LCPS.v2015.v001.WebUI.Areas.My.Models
{
    public class FilterPreviewModel
    {
        public MemberFilter Filter { get; set; }

        public PagedList<HRStaffRecord> StaffList { get; set; }

        public PagedList<StudentRecord> StudentList { get; set; }
    }
}