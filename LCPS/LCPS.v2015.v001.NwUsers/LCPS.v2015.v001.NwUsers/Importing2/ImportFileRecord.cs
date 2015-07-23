using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.Importing2
{
    public class ImportFileRecord : IImportFileRecord
    {
        public int Index { get; set; }

        public string Content { get; set; }

        public string[] Fields { get; set; }

        public ImportRecordStatus ValidationStatus { get; set; }

        public string ValidationReport { get; set; }

        public ImportRecordStatus ImportStatus { get; set; }

        public string ImportReport { get; set; }

        public ImportCrudStatus CrudStatus { get; set; }

        public virtual void Validate(){}

        public virtual void GetCrudStatus(){}

        public virtual void Import() { }

        public virtual void Create() { }

        public virtual void Update() { }

    }
}
