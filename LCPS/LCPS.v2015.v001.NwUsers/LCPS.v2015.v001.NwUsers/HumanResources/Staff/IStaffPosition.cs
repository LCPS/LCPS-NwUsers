using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public enum HRStaffPositionQualifier
    {
        Inactive = 0,
        Active = 1
    }

    public interface IStaffPosition
    {
        Guid PositionKey { get; set; }

        Guid StaffMemberId { get; set; }

        HRStaff StaffMember { get; }

        Guid BuildingKey { get; set; }

        
        HRBuilding Building { get; }

        Guid EmployeeTypeKey { get; set; }

        
        HREmployeeType EmployeeType { get; }
        
        Guid JobTitleKey { get; set; }

        
        HRJobTitle JobTitle { get; }

        HRStaffPositionQualifier Status { get; set; }

        string FiscalYear { get; set; }
    }
}
