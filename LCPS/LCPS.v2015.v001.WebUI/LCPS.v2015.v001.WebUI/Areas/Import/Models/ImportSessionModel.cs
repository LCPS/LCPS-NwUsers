using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LCPS.v2015.v001.NwUsers.Importing;

namespace LCPS.v2015.v001.WebUI.Areas.Import.Models
{
    public class ImportSessionModel : ImportSession
    {
        [Display(Name = "Input File")]
        public HttpPostedFileBase ImportFilePost { get; set; }
    }
}