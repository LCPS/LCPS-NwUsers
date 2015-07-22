using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Anvil.v2015.v001.Domain.Exceptions;
using Anvil.v2015.v001.Domain.Entities;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.WebUI.Areas.Students.Models;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;


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

        public DynamicStudentFilter GetStudentFilter()
        {
            DynamicStudentFilter f = new DynamicStudentFilter(FilterId);
            f.Refresh();

            return f;
        }

        public DynamicStaffFilter GetStaffFilter()
        {
            DynamicStaffFilter f = new DynamicStaffFilter(FilterId);
            f.Refresh();

            return f;
        }

        public StudentFilterClauseModel GetDefaultStudentClause()
        {
            StudentFilterClauseModel m = new StudentFilterClauseModel(DynamicStudentClause.GetDefaultStudentClause(FilterId));
            m.FormArea = "My";
            m.FormController = "Contacts";
            m.FormAction = "AddStudentClause";
            m.SubmitText = "Add Clause";

            return m;
        }

        public StaffFilterClauseModel GetDefaultStaffClause()
        {
            StaffFilterClauseModel m = new StaffFilterClauseModel(DynamicStaffClause.GetDefault(FilterId));

            m.FormArea = "My";
            m.FormController = "Contacts";
            m.FormAction = "AddStaffClause";
            m.SubmitText = "Add Clause";

            return m;
        }



    }
}