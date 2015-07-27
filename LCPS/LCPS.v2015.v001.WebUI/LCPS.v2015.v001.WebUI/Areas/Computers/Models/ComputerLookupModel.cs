using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace LCPS.v2015.v001.WebUI.Areas.Computers.Models
{
    public class ComputerLookupModel
    {
        [Display(Name = "Computer Name")]
        public string ComputerName { get; set; }


    }
}