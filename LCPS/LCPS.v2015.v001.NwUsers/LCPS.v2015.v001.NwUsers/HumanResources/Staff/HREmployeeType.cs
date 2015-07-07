﻿#region Using

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
using CompareAttribute = System.Web.Mvc.CompareAttribute;
using System.Web.Mvc;

#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public enum HREmployeeTypeCategory
    {
        Unknown = 0,
        Professional = 1,
        Classified = 2,
        Outsource = 3
    }

    [Serializable]
    [Table("HREmployeeType", Schema = "HumanResources")]
    public class HREmployeeType: IEmployeeType
    {
      


        #region Properties

        [Key]
        public Guid EmployeeTypeLinkId { get; set; }


        [Display(Name = "Type ID", Description = "An id uniquely identifying the employee type in the division")]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(15, ErrorMessage = "The ID cannot be more then 15 characters")]
        [Remote("CheckEmployeeTypeIdAvailable", "Authors", HttpMethod = "POST")]
        [Index("EmployeeTypeId_IX", IsUnique = true)]
        public string EmployeeTypeId { get; set; }

        [Display(Name = "Name", Description = "A descriptive name for the employee type")]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(128)]
        [Index("EmployeeTypeName_IX", IsUnique = true)]
        public string EmployeeTypeName { get; set; }

        [Display(Name = "Category", Description = "Classified, Professional, or Outsource")]
        public HREmployeeTypeCategory Category { get; set; }

        #endregion


        public override string ToString()
        {
            return Category + " - " + EmployeeTypeId + " - " + EmployeeTypeName;
        } 
    }
}
