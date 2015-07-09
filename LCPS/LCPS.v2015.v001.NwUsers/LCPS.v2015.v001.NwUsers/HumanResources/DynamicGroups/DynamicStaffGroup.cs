using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

using Anvil.v2015.v001.Domain.Exceptions;

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

        public List<HRStaffPosition> GetStaff()
        {
            LcpsDbContext db = new LcpsDbContext();
            List<StaffClauseGroup> clauses = db.DynamicStaffClauses.Where(x => x.StaffGroupId.Equals(DynamicGroupId)).OrderBy(x => x.SortIndex).ToList();

            if (clauses.Count() == 0)
                return new List<HRStaffPosition>();

            if (clauses.Count() > 0)
            {
                clauses[0].GroupConjunction = Anvil.v2015.v001.Domain.Entities.DynamicFilters.DynamicQueryConjunctions.None;
            }

            string query = "(";
            List<object> parms = new List<object>();

            foreach(StaffClauseGroup c in clauses)
            {
                if (c.GroupConjunction != Anvil.v2015.v001.Domain.Entities.DynamicFilters.DynamicQueryConjunctions.None)
                    query += c.GroupConjunction.ToString() + " (";

                Dictionary<string, object> dic = c.GetClausesForQuery();
                foreach (string k in dic.Keys)
                {
                    query += string.Format(k, parms.Count().ToString());
                    parms.Add(dic[k]);
                }

                query += ") ";
            }

            try
            {
                List<HRStaffPosition> positions = db.StaffPositions.Where(query, parms.ToArray()).ToList();

                return positions.OrderBy(x => x.StaffMember.LastName + x.StaffMember.FirstName + x.StaffMember.MiddleInitial).ToList();
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                ec.Insert(0, "Could not execute dynamic query");
                ec.Insert(1, query);
                throw ec.ToException();
            }

        }
    }
}
