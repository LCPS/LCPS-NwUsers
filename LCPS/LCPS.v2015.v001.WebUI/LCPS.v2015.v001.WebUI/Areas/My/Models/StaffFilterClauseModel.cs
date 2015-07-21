using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Filters;



namespace LCPS.v2015.v001.WebUI.Areas.My.Models
{
    public class StaffFilterClauseModel: StaffFilterClause
    {
        private LcpsDbContext _dbContext;

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();
                return _dbContext;
            }
        }

        public Guid AntecedentId { get; set; }

        public List<SelectListItem> GetBuildingList()
        {
            try
            {
                List<HRBuilding> bb = DbContext.Buildings.OrderBy(x => x.Name).ToList();
                return bb.Select(x => new SelectListItem() { Text = x.Name, Value = x.BuildingKey.ToString() }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get buildings from database", ex);
            }
        }

        public List<SelectListItem> GetEmployeeTypeList(Guid? buildingId)
        {
            try
            { 
                List<HREmployeeType> ett;
                if (buildingId != null)
                    ett = DbContext.EmployeeTypes.OrderBy(x => x.EmployeeTypeName).ToList();
                else
                    ett = (from HREmployeeType et in DbContext.EmployeeTypes
                           join HRStaffPosition s in DbContext.StaffPositions on et.EmployeeTypeLinkId equals s.EmployeeTypeKey
                           where s.BuildingKey.Equals(buildingId.Value)
                           select et).OrderBy(x => x.EmployeeTypeName).ToList();

                return ett.Select(x => new SelectListItem() { Text = x.EmployeeTypeName, Value = x.EmployeeTypeLinkId.ToString() }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get employee types from the database", ex);
            }
        }

        public List<SelectListItem> GetJobTitles()
        {
            try 
            { 
                List<HRJobTitle> jtt = new List<HRJobTitle>();

                if (BuildingValue.Equals(Guid.Empty) & EmployeeTypeValue.Equals(Guid.Empty))
                    jtt = DbContext.JobTitles.OrderBy(x => x.JobTitleName).ToList();

                if (!BuildingValue.Equals(Guid.Empty) & EmployeeTypeValue.Equals(Guid.Empty))
                    jtt = (from HRJobTitle jt in DbContext.JobTitles
                           join HRStaffPosition s in DbContext.StaffPositions on jt.JobTitleKey equals s.JobTitleKey
                           where s.BuildingKey.Equals(BuildingValue)
                           select jt).OrderBy(x => x.JobTitleName).ToList();

                if (BuildingValue.Equals(Guid.Empty) & !EmployeeTypeValue.Equals(Guid.Empty))
                    jtt = (from HRJobTitle jt in DbContext.JobTitles
                           join HRStaffPosition s in DbContext.StaffPositions on jt.JobTitleKey equals s.JobTitleKey
                           where s.EmployeeTypeKey.Equals(EmployeeTypeValue)
                           select jt).OrderBy(x => x.JobTitleName).ToList();

                if (BuildingValue.Equals(Guid.Empty) & EmployeeTypeValue.Equals(Guid.Empty))
                    jtt = (from HRJobTitle jt in DbContext.JobTitles
                           join HRStaffPosition s in DbContext.StaffPositions on jt.JobTitleKey equals s.JobTitleKey
                           where s.EmployeeTypeKey.Equals(EmployeeTypeValue) &
                             s.BuildingKey.Equals(BuildingValue)
                           select jt).OrderBy(x => x.JobTitleName).ToList();


                return jtt.Select(x => new SelectListItem() { Text = x.JobTitleName, Value = x.JobTitleKey.ToString() }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get job titles from the database", ex);
            }
        }



    }
}