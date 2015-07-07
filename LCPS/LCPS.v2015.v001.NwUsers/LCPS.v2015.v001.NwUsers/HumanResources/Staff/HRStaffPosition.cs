using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.Security;
using LCPS.v2015.v001.NwUsers.HumanResources;



namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    [Serializable]
    public class HRStaffPosition : IStaffPosition
    {
        #region Fields

        [NonSerialized]
        LcpsDbContext db;

        [NonSerialized]
        HRBuilding _building;

        [NonSerialized]
        HREmployeeType _employeeType;

        [NonSerialized]
        HRJobTitle _jobTitle;

        [NonSerialized]
        HRStaff _staff;

        #endregion

        [Key]
        public Guid PositionKey { get; set; }

        [Index("IX_Position", IsUnique = true, Order = 1)]
        public Guid StaffMemberId { get; set; }

        public LcpsDbContext Db
        {
            get
            {
                if (db == null)
                    db = new LcpsDbContext();

                return db;
            }
        }

        public IStaff StaffMember
        {
            get 
            {
                if (_staff == null)
                    _staff = Db.StaffMembers.FirstOrDefault(x => x.StaffKey.Equals(this.StaffMemberId));

                return _staff;
            }
        }

        [Index("IX_Position", IsUnique = true, Order = 2)]
        public Guid BuildingKey { get; set; }

        public IBuilding Building
        {
            get 
            {
                if (_building == null)
                    _building = Db.Buildings.First(x => x.BuildingKey.Equals(this.BuildingKey));

                return _building;
            }
        }

        [Index("IX_Position", IsUnique = true, Order = 3)]
        public Guid EmployeeTypeKey { get; set; }

        public IEmployeeType EmployeeType
        {
            get 
            {
                if (_employeeType == null)
                    _employeeType = Db.EmployeeTypes.First(x => x.EmployeeTypeId.Equals(EmployeeTypeKey));

                return _employeeType;
            }
        }

        [Index("IX_Position", IsUnique = true, Order = 4)]
        public Guid JobTitleKey { get; set; }

        public IJobTitle JobTitle
        {
            get 
            {
                if (_jobTitle == null)
                    _jobTitle = Db.JobTitles.First(x => x.JobTitleKey.Equals(JobTitleKey));

                return _jobTitle;
            }
        }

        public bool Active { get; set; }

        public string FiscalYear { get; set; }

        public void Load(string staffId, string buildingId, string employeeTypeId, string jobTitleId)
        {
            try
            {

                _staff = Db.StaffMembers.FirstOrDefault(x => x.StaffId == staffId);
                
                _building = Db.Buildings.FirstOrDefault(x => x.BuildingId == buildingId);
                
                _employeeType = Db.EmployeeTypes.FirstOrDefault(x => x.EmployeeTypeId == employeeTypeId);
                
                _jobTitle = Db.JobTitles.FirstOrDefault(x => x.JobTitleId == jobTitleId);

                if (_staff == null)
                    throw new Exception(string.Format("{0} is an invalid staff Id", staffId));

                if (_building == null)
                    throw new Exception(string.Format("{0} is an invalid building Id", buildingId));

                if (_employeeType == null)
                    throw new Exception(String.Format("{0} is an invalid employee type Id", employeeTypeId));

                if (_jobTitle == null)
                    throw new Exception(string.Format("{0} is an invalid job title Id", jobTitleId));

                HRStaffPosition p = Db.StaffPositions.FirstOrDefault(x => x.StaffMemberId.Equals(StaffMember.StaffKey) &
                    x.BuildingKey.Equals(x.Building.BuildingKey) & 
                    x.EmployeeTypeKey.Equals(x.EmployeeType.EmployeeTypeId) & 
                    x.JobTitleKey.Equals(x.JobTitle.JobTitleKey));

                if (p != null)
                {
                    Active = p.Active;
                    FiscalYear = p.FiscalYear;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not load position", ex);
            }
        }
    }
}