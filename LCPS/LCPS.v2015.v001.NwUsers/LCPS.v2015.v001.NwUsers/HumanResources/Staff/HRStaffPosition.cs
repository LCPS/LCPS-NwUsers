#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Serialization;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;


#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    [Table("HRStaffPosition", Schema = "HumanResources")]
    public class HRStaffPosition
    {

        HRStaff _staff = new HRStaff();
        HRBuilding _building = new HRBuilding();
        HREmployeeType _employeeType = new HREmployeeType();
        HRJobTitle _jobTitle = new HRJobTitle();
        LcpsDbContext db = new LcpsDbContext();

        public HRStaffPosition()
        { }



        public HRStaffPosition(string staffId, string buildingId, string employeeTypeId, string jobTitleId, bool active)
        {
            StaffPositionLinkId = Guid.NewGuid();


            /*
            _staff = db.StaffMembers.FirstOrDefault(x => x.StaffId == staffId);
            if (_staff == null)
                throw new Exception(string.Format("Staff member with Id {0} wasn't found", staffId));

            _building = db.Buildings.FirstOrDefault(x => x.BuildingId == buildingId);
            if (_building == null)
                throw new Exception(string.Format("Building with Id {0} wasn't found", staffId));

            _employeeType = db.EmployeeTypes.FirstOrDefault(x => x.EmployeeTypeId == employeeTypeId);
            if (_employeeType == null)
                throw new Exception(string.Format("Employee Type with Id {0} wasn't found", staffId));

            _jobTitle = db.JobTitles.FirstOrDefault(x => x.JobTitleId == jobTitleId);
            if (_jobTitle == null)
                throw new Exception(string.Format("Job Title with Id {0} wasn't found", staffId));
            */



            StaffLinkId = _staff.StaffLinkId;
            BuildingId = _building.BuildingKey;
            EmployeeTypeId = _employeeType.EmployeeTypeLinkId;
            JobTitleId = _jobTitle.JobTitleKey;
            Active = active;
        }



        [Key]
        public Guid StaffPositionLinkId { get; set; }

        public Guid StaffLinkId { get; set; }

        public Guid BuildingId { get; set; }

        public Guid EmployeeTypeId { get; set; }

        public Guid JobTitleId { get; set; }

        public bool Active { get; set; }

        [NotMapped]
        public string Status
        {
            get { if (Active) return "Active"; else return "Inactive"; }
        }


        [NotMapped]
        [Display(Name = "Staff Id")]
        public string StaffId
        {
            get { return StaffMember.StaffId; }
            set { StaffMember.StaffId = value; }
        }


        [NotMapped]
        [Display(Name = "Building")]
        public string BuildingName
        {
            get { return Building.Name; }
            set { Building.Name = value; }
        }

        [NotMapped]
        [Display(Name = "EmployeeType")]
        public string EmployeeTypeName
        {
            get { return EmployeeType.EmployeeTypeName; }
            set { EmployeeType.EmployeeTypeName = value; }
        }

        [NotMapped]
        [Display(Name = "JobTitle")]
        public string JobTitleName
        {
            get { return JobTitle.JobTitleName; }
            set { JobTitle.JobTitleName = value; }
        }

        [Required]
        [ForeignKey("StaffLinkId")]
        [XmlIgnore]
        public virtual HRStaff StaffMember
        {
            get { return _staff;  }
            set { _staff = value; }
        }

        [Required]
        [ForeignKey("BuildingId")]
        [XmlIgnore]
        public virtual HRBuilding Building
        {
            get { return _building; }
            set { _building = value; }
        }

        [Required]
        [ForeignKey("EmployeeTypeId")]
        [XmlIgnore]
        public virtual HREmployeeType EmployeeType
        {
            get { return _employeeType; }
            set { _employeeType = value; }
        }

        [Required]
        [ForeignKey("JobTitleId")]
        [XmlIgnore]
        public virtual HRJobTitle JobTitle
        {
            get { return _jobTitle; }
            set { _jobTitle = value; }
        }
    }
}
