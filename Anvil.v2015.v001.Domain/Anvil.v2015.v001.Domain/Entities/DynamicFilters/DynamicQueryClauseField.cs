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
    [Table("DynamicQueryClauseField", Schema = "Filtering")]
    public class DynamicQueryClauseField
    {
        [Key]
        [Index("IX_QueryClauseField", IsUnique = true, Order = 1)]
        public Guid FieldId { get; set; }

        [Index("IX_QueryClauseField", IsUnique = true, Order = 2)]
        public Guid QueryId { get; set; }

        
        public Guid ClauseId { get; set; }

        public bool Include { get; set; }

        public DynamicQueryConjunctions Conjunction { get; set; }

        [Index("IX_QueryClauseField", IsUnique = true, Order = 3)]
        [Display(Name = "Field", Description = "The name of the field to query")]
        [Required]
        [MaxLength(75)]
        public string FieldName { get; set; }

        public DynamicQueryOperators Operator { get; set; }

        public object Value { get; set; }

    }
}
