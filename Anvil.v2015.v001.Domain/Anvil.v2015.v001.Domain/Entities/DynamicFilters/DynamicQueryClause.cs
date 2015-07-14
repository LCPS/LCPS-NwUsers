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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    [Table("DynamicQueryClause", Schema = "Filtering")]
    public class DynamicQueryClause
    {
        [Key]
        [Index("IX_QueryClause", IsUnique = true, Order = 1)]
        public Guid ClauseId { get; set; }

        [Index("IX_QueryClause", IsUnique = true, Order = 2)]
        public Guid QueryId { get; set; }

        public DynamicQueryConjunctions ClauseConjunction { get; set; }
        
        
        [ForeignKey("QueryId")]
        [Required]
        public virtual DynamicQuery Query { get; set; }
        
    }
}
