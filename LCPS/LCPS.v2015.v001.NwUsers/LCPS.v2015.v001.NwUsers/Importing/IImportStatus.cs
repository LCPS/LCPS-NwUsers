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


#endregion

namespace LCPS.v2015.v001.NwUsers.Importing
{
    public enum ImportResultStatus
    {
        @default = 0,
        success = 1,
        info = 2,
        warning = 3,
        danger = 4
    }

    public enum ImportEntityStatus
    {
        None = 0,
        Create = 1,
        Update = 2,
        Ignore = 3,
        Error = 4,
        Conflict = 5,
        Duplicate = 6
    }
   
    public interface IImportStatus
    {
        Guid ImportItemId { get; set; }

        Guid SessionId { get; set; }

        string Comment { get; set; }

        string Description { get; set;  }

        DateTime EntryDate { get; set; }

        ImportResultStatus ImportStatus { get; set; }

        byte[] SerializedData { get; set; }

        object SourceItem { get; set; }

        void Serialize();

        object Deserialize();

        object Deserialize(IImportSession session);

        ImportEntityStatus EntityStatus { get; set; }

        int LineIndex { get; set; }

        void Validate();

        void Record();

        ImportItem ToImportItem();
    }

}
