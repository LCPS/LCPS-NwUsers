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
    [Flags]
    public enum ImportCrudStatus
    {
        None = 0,
        InsertMember = 1,
        UpdateMember = 2,
        CreateMembership = 4,
        UpdateMembership = 8
    }
}
