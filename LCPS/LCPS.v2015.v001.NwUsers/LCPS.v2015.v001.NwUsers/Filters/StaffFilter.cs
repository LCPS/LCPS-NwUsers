using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPS.v2015.v001.NwUsers.Filters
{
    [Table("StaffFilter", Schema = "HumanResources")]
    public class StaffFilter
    {
        [Key]
        public Guid StaffFilterId { get; set; }

        [Index("IX_FilterName", IsUnique = true, Order = 1)]
        public Guid AntecedentId { get; set; }

        [Display(Name = "Filter Name", Description = "A descriptive title for the name")]
        [MaxLength(128)]
        [Index("IX_FilterName", IsUnique = true, Order = 2)]
        public string Title { get; set; }

        [Display(Name = "Description", Description = "A detailed description for the filter")]
        [MaxLength(128)]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public FilterCategories Category { get; set; }



    }
}
