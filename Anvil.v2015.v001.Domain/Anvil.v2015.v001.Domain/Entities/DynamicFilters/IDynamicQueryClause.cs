using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    public interface IDynamicQueryClause
    {
        bool Include { get; set; }

        DynamicQueryConjunctions Conjunction { get; set; }

        string FieldName { get; set; }

        DynamicQueryOperators Operator { get; set; }

        string Value { get; set; }
    }
}
