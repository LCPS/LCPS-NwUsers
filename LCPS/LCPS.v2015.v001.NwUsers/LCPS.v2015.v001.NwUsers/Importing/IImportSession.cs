#region Using

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
using System.Web.Mvc;
using System.Web;

#endregion

namespace LCPS.v2015.v001.NwUsers.Importing
{
    public  interface IImportSession
    {
        //-- ---------- Info

         Guid SessionId { get; set; }

         DateTime SessionDate { get; set; }

         string Author { get; set; }

         string TypeName { get; }

         string FullAssemblyTypeName { get; set; }

         System.Type ItemType { get; }

         ImportSession ToImportSession();

         //-- ---------- Options 

         bool AddIfNotExist { get; set; }
         
         bool UpdateIfExists { get; set; }

         //-- ---------- Items

         List<IImportStatus> Candidates { get; }

         //-- ---------- File Contents

         string Delimiter { get; set; }

         void ParseItems(StreamReader reader);

         string[] FieldNames { get; set; }

         byte[] ImportFileContents { get; set; }


         //-- ---------- UI
         string ViewTitle { get; set; }

         string Area { get; set; }

         string Controller { get; set; }

         string Action { get; set; }

         string ViewLayoutPath { get; set; }

         //-- ---------- UI
         void Record();


         //-- ---------- Import

         void Import();

         void DetectConflicts();
        
         
     
         


    }
}
