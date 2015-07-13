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

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    public class DynamicQueryClause : IDynamicQueryClause
    {

        #region "Fields"

        private DynamicQueryOperatorLibrary lib = new DynamicQueryOperatorLibrary();

        #endregion


        public bool Include { get; set; }

        public DynamicQueryConjunctions Conjunction { get; set; }

        public string FieldName { get; set; }

        public DynamicQueryOperators Operator { get; set; }

        public string Value { get; set; }

    }
}
