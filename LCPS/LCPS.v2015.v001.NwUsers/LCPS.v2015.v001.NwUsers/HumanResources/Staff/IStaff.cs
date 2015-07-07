using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public interface IStaff
    {
        Guid StaffKey { get; set; }

        string StaffId { get; set; }

        string StaffEmail { get; set; }
    }
}
