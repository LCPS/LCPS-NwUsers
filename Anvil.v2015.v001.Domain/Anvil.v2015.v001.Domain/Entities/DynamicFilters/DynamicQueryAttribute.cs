using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct)]
    public class DynamicQueryAttribute : Attribute
    {
        public bool IsField { get; set; }
    }
}
