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
    public enum ImportRecordStatus
    {
        @default = 0,
        info = 1,
        warning = 2,
        danger = 4,
        success = 8
    }
}
