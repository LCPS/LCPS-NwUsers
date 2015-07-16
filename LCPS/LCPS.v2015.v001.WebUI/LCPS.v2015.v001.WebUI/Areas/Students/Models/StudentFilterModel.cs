using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.Students;
using System.ComponentModel.DataAnnotations;
using LCPS.v2015.v001.NwUsers.Filters;

namespace LCPS.v2015.v001.WebUI.Areas.Students.Models
{
    public class StudentFilterModel : StudentFilterClause
    {
        public string FormArea { get; set; }
        public string FormController { get; set; }
        public string FormAction { get; set; }

        public List<SelectListItem> Buildings { get; set; }
        public List<SelectListItem> InstructionalLevels { get; set; }
    }
}