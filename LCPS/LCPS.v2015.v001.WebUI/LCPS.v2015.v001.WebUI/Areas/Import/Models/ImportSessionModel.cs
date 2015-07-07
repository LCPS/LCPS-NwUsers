using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LCPS.v2015.v001.NwUsers.Importing;

namespace LCPS.v2015.v001.WebUI.Areas.Import.Models
{
    public class ImportSessionModel
    {

        public string PageHeader { get; set; }
        public string Layout { get; set; }



        public List<ImportSession> Sessions { get; set; }

    }
}