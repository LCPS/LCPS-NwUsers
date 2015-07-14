using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.Students;
using System.ComponentModel.DataAnnotations;

namespace LCPS.v2015.v001.WebUI.Areas.Students.Models
{
    public class StudentFilterModel : StudentFilterClause
    {
        public List<SelectListItem> Buildings { get; set; }
        public List<SelectListItem> InstructionalLevels { get; set; }
    }
}