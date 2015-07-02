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


#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    [Table("HRStaffPosition", Schema = "HumanResources")]
    public class HRStaffPosition
    {
        
        HRStaff _staff;
        HRBuilding _building;
        HREmployeeType _employeeType;
        HRJobTitle _jobTitle;
        LcpsDbContext db = new LcpsDbContext();

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
        [Display(Name = "Building")]
        public string BuildingName
        {
            get { return Building.Name; }
            set { Building.Name = value ; }
        }

        [NotMapped]
        [Display(Name = "EmployeeType")]
        public string EmployeeTypeName
        {
            get { return EmployeeType.EmployeeTypeName;  }
            set { EmployeeType.EmployeeTypeName = value; }
        }

        [NotMapped]
        [Display(Name = "JobTitle")]
        public string JobTitleName
        {
            get { return JobTitle.JobTitleName;  }
            set { JobTitle.JobTitleName = value; }
        }

        [Required]
        [ForeignKey("StaffLinkId")]
        [XmlIgnore]
        public virtual HRStaff StaffMember
        {
            get { return db.StaffMembers.First(x => x.StaffLinkId.Equals(StaffLinkId)); }
            set{ _staff = value; }
        }

        [Required]
        [ForeignKey("BuildingId")]
        [XmlIgnore]
        public virtual HRBuilding Building 
        {
            get { return db.Buildings.First(x => x.BuildingKey.Equals(BuildingId)); }
            set { _building = value; }
        }

        [Required]
        [ForeignKey("EmployeeTypeId")]
        [XmlIgnore]
        public virtual HREmployeeType EmployeeType
        {
            get { return db.EmployeeTypes.First(x => x.EmployeeTypeLinkId.Equals(EmployeeTypeId)); }
            set { _employeeType = value; }
        }

        [Required]
        [ForeignKey("JobTitleId")]
        [XmlIgnore]
        public virtual HRJobTitle JobTitle
        {
            get {  return db.JobTitles.First(x => x.RecordId.Equals(JobTitleId)); }
            set { _jobTitle = value; }
        }
    }
}
