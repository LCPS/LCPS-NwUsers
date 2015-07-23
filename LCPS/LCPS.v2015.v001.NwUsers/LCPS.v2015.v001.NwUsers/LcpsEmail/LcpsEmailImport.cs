using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Anvil.v2015.v001.Domain.Exceptions;

using LCPS.v2015.v001.NwUsers.Importing2;

namespace LCPS.v2015.v001.NwUsers.LcpsEmail
{
    public class LcpsEmailImport : ImportFileRecord
    {
        public override void Validate()
        {
            try
            { 
                //-- Is the Enity Id a valid entity id

            }
            catch(Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(new Exception("Validation Error", ex));
                ValidationReport = string.Join("\n", ec.ToArray());
            }
        }

        
    }
}
