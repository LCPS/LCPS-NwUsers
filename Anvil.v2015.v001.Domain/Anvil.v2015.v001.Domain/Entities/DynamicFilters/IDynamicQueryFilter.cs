using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    public interface IDynamicQueryFilter
    {
        string Name { get; set; }

        List<IDynamicQueryClause> Clauses { get; set;}
    }
}
