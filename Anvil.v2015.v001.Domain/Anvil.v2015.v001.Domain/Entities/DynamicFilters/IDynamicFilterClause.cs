using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    public interface IDynamicFilterClause : IList<DynamicQueryClauseField>
    {
        DynamicQueryConjunctions ClauseConjunction { get; set; }

        Guid FilterId { get; set; }

        Guid ClauseId { get; set; }

        List<object> Parms { get; }

        List<string> Elements { get; }

        DynamicQueryStatement ToDynamicQueryStatement();

        String ToFriendlyString();

        void AddElement(DynamicQueryConjunctions conjunction, string fieldName, DynamicQueryOperators op, object value);
    }
}
