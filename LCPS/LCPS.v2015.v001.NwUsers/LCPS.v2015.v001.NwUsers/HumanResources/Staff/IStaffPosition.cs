using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public interface IStaffPosition
    {
        Guid PositionKey { get; set; }

        Guid StaffMemberId { get; set; }

        IStaff StaffMember { get; }

        Guid BuildingKey { get; set; }
        IBuilding Building { get; }

        Guid EmployeeTypeKey { get; set; }
        IEmployeeType EmployeeType { get; }
        
        Guid JobTitleKey { get; set; }
        IJobTitle JobTitle { get; }

        bool Active { get; set; }

        string FiscalYear { get; set; }
    }
}
