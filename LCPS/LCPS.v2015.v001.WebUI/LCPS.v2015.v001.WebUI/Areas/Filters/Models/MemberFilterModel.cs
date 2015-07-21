using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LCPS.v2015.v001.NwUsers.Filters;

namespace LCPS.v2015.v001.WebUI.Areas.Filters.Models
{
    public class MemberFilterModel
    {
        public string FormAction { get; set; }
        public string FormController { get; set; }
        public string FormArea { get; set; }

        public bool ForModal {  get; set;}

        public MemberFilter Filter { get; set; }
    }
}