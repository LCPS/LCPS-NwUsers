using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LcpsLdapSync
{
    public enum OUSyncAction
    {
        None = 0,
        Create = 1,
        Move = 2
    }
}
