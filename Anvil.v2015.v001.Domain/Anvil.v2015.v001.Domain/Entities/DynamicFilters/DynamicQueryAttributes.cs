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

    [System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct)
]
    public class DQComparer : Attribute
    {
        public DQComparer(string fieldName)
        {
            this.FieldName = FieldName;
        }

        public string FieldName { get; set; }
        public DQElementTypes ElementType { get; set; }
    }

    public enum DQElementTypes
    {
        Comparer,
        Field,
        Operator,
        Value
    }

}
