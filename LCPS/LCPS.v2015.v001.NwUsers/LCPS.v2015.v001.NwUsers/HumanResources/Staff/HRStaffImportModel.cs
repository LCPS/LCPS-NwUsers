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


#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public class HRStaffImportModel
    {
        [Display(Name = "Staff ID")]
        [Required(ErrorMessage = "Staff ID is required", AllowEmptyStrings = false)]
        [MaxLength(25, ErrorMessage = "The Staff IF field must not excede 25 characters")]
        public string StaffId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required", AllowEmptyStrings = false)]
        [MaxLength(75)]
        public string FirstName { get; set; }

        [Display(Name = "MI")]
        [MaxLength(6, ErrorMessage = "The Middle Initials must not excede 6 characters")]
        public string MiddleInitial { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required", AllowEmptyStrings = false)]
        [MaxLength(75)]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        public HRPerson.HRGenders GenderQualifier { get; set; }

        [Display(Name = "Birthdate")]
        [Required(ErrorMessage = "Birthdate is a required field", AllowEmptyStrings = false)]
        [MaxLength(10)]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Building")]
        [Required(ErrorMessage = "Building ID is a required field", AllowEmptyStrings = false)]
        [MaxLength(25)]
        public string BuildingId { get; set; }

        [Display(Name = "Employee Type")]
        [Required(ErrorMessage = "Employee Type is a required field", AllowEmptyStrings = false)]
        [MaxLength(25)]
        public string EmployeeTypeId { get; set; }

        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "Job Title is a required field", AllowEmptyStrings = false)]
        [MaxLength(25)]
        public string JobTitleId { get; set; }

        [Display(Name = "Active")]
        public Boolean Active { get; set; }


        public override string ToString()
        {
            return "(" + StaffId + ")" + LastName + ", " + FirstName + " - " + BuildingId + "," + EmployeeTypeId + "," + JobTitleId + " - Active: " + Active.ToString() + " - BD: " + Birthdate.ToShortDateString() + " - Gender: " + GenderQualifier.ToString();
        }
    }
}
