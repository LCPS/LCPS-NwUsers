using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.Students;



namespace LCPS.v2015.v001.WebUI.Areas.Students.Models
{
    public class StudentListModel
    {

        public StudentListModel()
        {
            Buildings = new List<SelectListItem>();
            InstructionalLevels = new List<SelectListItem>();
        }

        [Display(Name = "Building")]
        public Guid BuildingId { get; set; }
        public List<SelectListItem> Buildings { get; set; }

        [Display(Name = "Level")]
        public Guid InstructionalLevelId { get; set; }
        public List<SelectListItem> InstructionalLevels { get; set; }

    }
}