using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public class HRStaffPositionDefinition
    {
        public HRStaffPositionDefinition(string buildingId, string employeeTypeId, string jobTitleId, LcpsDbContext db)
        {
            this.Building= db.Buildings.FirstOrDefault(x => x.BuildingId == buildingId);

            this.EmployeeType = db.EmployeeTypes.FirstOrDefault(x => x.EmployeeTypeId == employeeTypeId);

            this.JobTitle = db.JobTitles.FirstOrDefault(x => x.JobTitleId == jobTitleId);

            if (Building == null)
                throw new Exception(string.Format("{0} is an invalid building Id", buildingId));

            if (EmployeeType == null)
                throw new Exception(String.Format("{0} is an invalid employee type Id", employeeTypeId));

            if (JobTitle == null)
                throw new Exception(string.Format("{0} is an invalid job title Id", jobTitleId));
        }

        public HRBuilding Building { get; set; }
        public HREmployeeType EmployeeType { get; set; }
        public HRJobTitle JobTitle { get; set; }
    }
}
