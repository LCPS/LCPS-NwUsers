using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPS.v2015.v001.NwUsers.HumanResources.DynamicGroups
{
    [Serializable]
    [Table("DynamicStaffGroup", Schema = "HumanResources")]
    public class DynamicStaffGroup
    {
        [Key]
        public Guid DynamicGroupId { get; set; }

        [Index("IX_DynamicGroupName", IsUnique = true)]
        [Display(Name = "Group Name")]
        [Required]
        [MaxLength(128)]
        public string GroupName { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        public override string ToString()
        {
            string query = "";

            LcpsDbContext db = new  LcpsDbContext();
            List<StaffClauseGroup> clauses = db.DynamicStaffClauses.Where(x => x.StaffGroupId.Equals(DynamicGroupId)).OrderBy(x => x.SortIndex).ToList();
            
            if(clauses.Count() > 0)
            {
                clauses[0].GroupConjunction = Anvil.v2015.v001.Domain.Entities.DynamicFilters.DynamicQueryConjunctions.None;
            }

            query = "";
            foreach(StaffClauseGroup c in clauses)
            {
                query += c.ToString();
            }

            return query;
        }
    }
}
