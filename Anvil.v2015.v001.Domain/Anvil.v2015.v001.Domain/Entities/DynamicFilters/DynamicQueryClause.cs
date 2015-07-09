using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    public class DynamicQueryClause
    {
        public DynamicQueryConjunctions Conjunction { get; set; }

        public string FieldName { get; set; }

        public DynamicQueryOperators Operator { get; set; }

        public Object Value { get; set; }
    }
}
