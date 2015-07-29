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

using System.Web.Mvc;



namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    [Serializable]
    [Table("HRStaffPosition", Schema = "HumanResources")]
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


        #region Constructors

        public HRStaffPosition()
        { }

        public HRStaffPosition(HRStaff staff, HRBuilding building, HREmployeeType employeeType, HRJobTitle jobTitle)
        {
            _staff = staff;
            _building = building;
            _employeeType = employeeType;
            _jobTitle = jobTitle;
            StaffMemberId = staff.StaffKey;
            BuildingKey = building.BuildingKey;
            EmployeeTypeKey = employeeType.EmployeeTypeLinkId;
            JobTitleKey = jobTitle.JobTitleKey;
        }

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

        public HRStaff StaffMember
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


        public HRBuilding Building
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


        public HREmployeeType EmployeeType
        {
            get 
            {
                if (_employeeType == null)
                    _employeeType = Db.EmployeeTypes.First(x => x.EmployeeTypeLinkId.Equals(EmployeeTypeKey));

                return _employeeType;
            }
        }

        [Index("IX_Position", IsUnique = true, Order = 4)]
        public Guid JobTitleKey { get; set; }


        public HRJobTitle JobTitle
        {
            get 
            {
                if (_jobTitle == null)
                    _jobTitle = Db.JobTitles.First(x => x.JobTitleKey.Equals(JobTitleKey));

                return _jobTitle;
            }
        }

        public HRStaffPositionQualifier Status { get; set; }

        public string FiscalYear { get; set; }

        public string Caption 
        {
            get
            {
                return this.Building.Name + " - " + this.EmployeeType.EmployeeTypeName + " - " + this.JobTitle.JobTitleName + " (" + this.Status.ToString() + ")";
            }
        }

        public static HRStaffPosition Load(string staffId, string buildingId, string employeeTypeId, string jobTitleId, LcpsDbContext db)
        {
            try
            {
                HRStaff  _staff = db.StaffMembers.FirstOrDefault(x => x.StaffId == staffId);
                
                HRBuilding _building = db.Buildings.FirstOrDefault(x => x.BuildingId == buildingId);
                
                HREmployeeType _employeeType = db.EmployeeTypes.FirstOrDefault(x => x.EmployeeTypeId == employeeTypeId);
                
                HRJobTitle _jobTitle = db.JobTitles.FirstOrDefault(x => x.JobTitleId == jobTitleId);

                if (_staff == null)
                    throw new Exception(string.Format("{0} is an invalid staff Id", staffId));

                if (_building == null)
                    throw new Exception(string.Format("{0} is an invalid building Id", buildingId));

                if (_employeeType == null)
                    throw new Exception(String.Format("{0} is an invalid employee type Id", employeeTypeId));

                if (_jobTitle == null)
                    throw new Exception(string.Format("{0} is an invalid job title Id", jobTitleId));

                HRStaffPosition p = new HRStaffPosition(_staff, _building, _employeeType, _jobTitle);

                return p;

            }
            catch (Exception ex)
            {
                throw new Exception("Could not load position", ex);
            }

        }

        public static void ValidatePosition(string buildingId, string employeeTypeId, string jobTitleId, LcpsDbContext db)
        {
            HRBuilding _building = db.Buildings.FirstOrDefault(x => x.BuildingId == buildingId);

            HREmployeeType _employeeType = db.EmployeeTypes.FirstOrDefault(x => x.EmployeeTypeId == employeeTypeId);

            HRJobTitle _jobTitle = db.JobTitles.FirstOrDefault(x => x.JobTitleId == jobTitleId);


            if (_building == null)
                throw new Exception(string.Format("{0} is an invalid building Id", buildingId));

            if (_employeeType == null)
                throw new Exception(String.Format("{0} is an invalid employee type Id", employeeTypeId));

            if (_jobTitle == null)
                throw new Exception(string.Format("{0} is an invalid job title Id", jobTitleId));
        }

        public static IEnumerable<SelectListItem> StaffQualifierList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string[] names = System.Enum.GetNames(typeof(HRStaffPositionQualifier));
            foreach(string name in names)
            {
                int value = Convert.ToInt32(System.Enum.Parse(typeof(HRStaffPositionQualifier), name));
                SelectListItem si = new SelectListItem()
                {
                    Text = name,
                    Value = value.ToString()
                };
                items.Add(si);
            }
            return items;
        }
    }
}