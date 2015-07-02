using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Models
{
    public class HRJobTitleImportModel 
    {
        public string EmployeeTypeId { get; set; }

        [Display(Name = "ID", Description = "An ID that uniquely identifies the job title in the division")]
        [Required]
        [MaxLength(15)]
        public string JobTitleId { get; set; }

        [Display(Name = "Name", Description = "A descriptive name for the job title")]
        [Required]
        [MaxLength(128)]
        public string JobTitleName { get; set; }

        public override string ToString()
        {
            return EmployeeTypeId + " - " + JobTitleId + " - " + JobTitleName;
        }
    }
}