using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.NwUsers.Filters
{
    [Table("MemberFilter", Schema = "Filters")]
    public class MemberFilter
    {
        [Key]
        public Guid FilterId { get; set; }

        [Index("IX_FilterName", IsUnique = true, Order = 1)]
        public Guid AntecedentId { get; set; }

        [Display(Name = "Filter Name", Description = "A descriptive title for the filter")]
        [MaxLength(128)]
        [Index("IX_FilterName", IsUnique = true, Order = 2)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Category")]
        public FilterCategories Category { get; set; }

        [Display(Name = "Class")]
        public MemberFilterClass FilterClass { get; set; }


        public override string ToString()
        {
            return ToDynamicQuery().ToString();
        }

        public DynamicQueryStatement ToDynamicQueryStatement()
        {
            return ToDynamicQuery().ToDynamicQueryStatement();
        }

        public DynamicQuery ToDynamicQuery()
        {
            if (FilterClass == MemberFilterClass.Student)
                //return ToStudentQuery();
                return null;
            else
                return ToStaffQuery();
        }

        private DynamicQuery ToStaffQuery()
        {
            DynamicQuery q = new DynamicQuery();

            LcpsDbContext db = new LcpsDbContext();

            List<StaffFilterClause> scc = db.StaffFilterClauses
                .Where(x => x.FilterId.Equals(FilterId))
                .OrderBy(x => x.SortIndex)
                .ToList();

            foreach (StaffFilterClause sc in scc)
            {
                q.Add(sc.ToDynamicQueryClause());
            }

            return q;
        }

        /*
        private DynamicQuery ToStudentQuery()
        {
            DynamicQuery q = new DynamicQuery();

            LcpsDbContext db = new LcpsDbContext();

            List<StudentFilterClause> scc = db.StudentFilterClauses
                .Where(x => x.FilterId.Equals(FilterId))
                .OrderBy(x => x.SortIndex)
                .ToList();

            foreach (StudentFilterClause sc in scc)
            {
                q.Add(sc.ToDynamicQueryClause());
            }

            return q;
        }
         * */
    }
}
