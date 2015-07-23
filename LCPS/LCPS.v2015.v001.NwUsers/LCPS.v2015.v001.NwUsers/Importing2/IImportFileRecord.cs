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
    public interface IImportFileRecord
    {
        int Index { get; set; }

        string Content { get; set; }

        string[] Fields { get; set; }

        ImportRecordStatus ValidationStatus { get; set; }

        string ValidationReport { get; set; }

        ImportRecordStatus ImportStatus { get; set; }

        string ImportReport { get; set; }

        ImportCrudStatus CrudStatus { get; set; }

        void Validate();

        void GetCrudStatus();

        void Import();

        void Create();

        void Update();
        

    }
}
