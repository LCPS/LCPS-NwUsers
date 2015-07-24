using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anvil.v2015.v001.Domain.Html;

using LCPS.v2015.v001.NwUsers.Importing2;

namespace LCPS.v2015.v001.WebUI.Areas.Import2.Models
{
    public class ImportViewModel : IAnvilFormHandler
    {
        [Display(Name = "Import File")]
        public HttpPostedFileBase InputFile { get; set; }

        

        public IImportFile ImportFile { get; set; }

        public void ProcessImportFile(System.Type t)
        {
            ImportFile iFile = new ImportFile(InputFile.InputStream, '\t');
            iFile.ItemType = t;
            iFile.Overwrite = true;
            iFile.ParseFile();
            this.ImportFile = iFile;
        }

        public string FormArea { get; set; }

        public string FormController { get; set; }

        public string FormAction { get; set; }

        public string SubmitText { get; set; }

        public string OnErrorActionName { get; set; }


        public string ImportController { get; set; }

        public string ImportAction { get; set; }
        
        public string ImportArea { get; set; }

        public string SessionVarName { get; set; }

    }
}