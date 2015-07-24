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
    public interface IImportFile
    {
        string[] Columns { get; set; }

        bool Overwrite { get; set; }

        List<IImportFileRecord> Lines { get; set; }

        StreamReader Contents { get; set; }

        void ParseFile();

        System.Type ItemType { get; set; }

        char Delimiter { get; set; }

        void Import();

        List<object> Items { get; set; }

        void ParseLine(string line);
    }
}
