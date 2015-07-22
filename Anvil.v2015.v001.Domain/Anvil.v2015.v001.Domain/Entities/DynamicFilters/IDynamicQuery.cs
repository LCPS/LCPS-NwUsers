using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    public interface IDynamicQuery: IList<IDynamicQueryClause>
    {
         #region Properties

        Guid FilterId { get; set; }

        List<object> Parms { get; }
        
        List<String> Elements { get; }

        #endregion

        #region Conversions

        DynamicQueryStatement ToDynamicQueryStatement();

        #endregion
    }
}
