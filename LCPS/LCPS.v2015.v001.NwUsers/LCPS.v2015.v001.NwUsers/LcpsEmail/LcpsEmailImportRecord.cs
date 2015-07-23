using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.LcpsEmail
{
    public class LcpsEmailImportRecord
    {
        public string EntityID { get; set; }

        public string Classification { get; set; }

        public string EmailAddress { get; set; }

        public string InitialPassword { get; set; }
    }
}
