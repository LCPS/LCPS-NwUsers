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

using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    [Table("DynamicQuery", Schema = "Filters")]
    public class DynamicQuery 
    {
        [Key]
        public Guid QueryId { get; set; }

        [Index("IX_QueryName", IsUnique = true, Order = 1)]
        public Guid AntecedentId { get; set; }

        [Index("IX_QueryName", IsUnique = true, Order = 2)]
        [Display(Name = "Name", Description = "A descriptive title for the query")]
        [MaxLength(75)]
        [Required]
        public string Name { get; set; }


        [Display(Name = "Description", Description = "A detailed description for the query")]
        [MaxLength(2048)]
        [Required]
        public string Description { get; set; }
    }
}
