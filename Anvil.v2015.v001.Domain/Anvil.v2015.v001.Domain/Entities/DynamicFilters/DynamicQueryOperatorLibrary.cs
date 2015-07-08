using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    public class DynamicQueryOperatorLibrary
    {
        private Dictionary<DynamicQueryOperators, string> _lib = new Dictionary<DynamicQueryOperators, string>();


        public DynamicQueryOperatorLibrary()
        {
            _lib.Add(DynamicQueryOperators.Equals, "=");
            _lib.Add(DynamicQueryOperators.NotEqual, "<>");
            _lib.Add(DynamicQueryOperators.Less, "<");
            _lib.Add(DynamicQueryOperators.LessOrEqual, "<=");
            _lib.Add(DynamicQueryOperators.Greater, ">");
            _lib.Add(DynamicQueryOperators.GreaterOrEqual, ">=");
        }


        public string GetOperator(DynamicQueryOperators @operator)
        {
            return this._lib[@operator];
        }
    }
}
