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
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;


#endregion

namespace LCPS.v2015.v001.NwUsers.Students
{
    public class StudentFilter
    {
       

        #region Properties

        public Guid AntecedentId { get; set; }

        public IDynamicQueryFilter Building { get; set; }



        #endregion

    }
}
