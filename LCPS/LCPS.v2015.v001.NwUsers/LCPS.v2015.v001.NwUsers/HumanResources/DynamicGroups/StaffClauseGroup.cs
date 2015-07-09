using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Mvc;

using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;

using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPS.v2015.v001.NwUsers.HumanResources.DynamicGroups
{
    [Serializable]
    [Table("StaffClauseGroup", Schema = "HumanResources")]
    public class StaffClauseGroup
    {
        [Key]
        public Guid RecordId { get; set; }

        [Index("IX_StaffClause", IsUnique = true, Order = 1)]
        public Guid StaffGroupId { get; set; }

        public int SortIndex { get; set; }

        public DynamicQueryConjunctions GroupConjunction { get; set; }

        public DynamicQueryConjunctions BuildingConjunction { get; set; }
        public DynamicQueryOperators BuildingOperator { get; set; }

        [Index("IX_StaffClause", IsUnique = true, Order = 2)]
        public Guid Building { get; set; }

        public DynamicQueryConjunctions EmployeeTypeConjunction { get; set; }
        public DynamicQueryOperators EmployeeTypeOperator { get; set; }

        [Index("IX_StaffClause", IsUnique = true, Order = 3)]
        public Guid EmployeeType { get; set; }

        public DynamicQueryConjunctions JobTitleConjunction { get; set; }
        public DynamicQueryOperators JobTitleOperator { get; set; }

        [Index("IX_StaffClause", IsUnique = true, Order = 4)]
        public Guid JobTitle { get; set; }

        public DynamicQueryConjunctions StaffConjunction { get; set; }
        public DynamicQueryOperators StaffOperator { get; set; }
        public Guid Staff { get; set; }

        public DynamicQueryConjunctions StatusConjunction { get; set; }
        public DynamicQueryOperators StatusOperator { get; set; }
        public HRStaffPositionQualifier Status { get; set; }

        public DynamicQueryConjunctions YearConjunction { get; set; }
        public DynamicQueryOperators YearOperator { get; set; }
        public String Year { get; set; }

        [ForeignKey("StaffGroupId")]
        public virtual DynamicStaffGroup StaffGroup { get; set; }

        public override string ToString()
        {
            LcpsDbContext db = new LcpsDbContext();

            HRBuilding b = db.Buildings.First(x => x.BuildingKey.Equals(Building));
            HREmployeeType e = db.EmployeeTypes.First(x => x.EmployeeTypeLinkId.Equals(EmployeeType));
            HRJobTitle j = db.JobTitles.First(x => x.JobTitleKey.Equals(JobTitle));

            List<string> items = new List<string>();

            DynamicQueryOperatorLibrary lib = new DynamicQueryOperatorLibrary();


            if (BuildingConjunction != DynamicQueryConjunctions.None)
            {
                if (items.Count == 0)
                    items.Add("Building " + lib.GetOperator(BuildingOperator) + " '" + b.Name + "'");
                else
                    items.Add(BuildingConjunction.ToString() + " Building " + lib.GetOperator(BuildingOperator) + " '" + b.Name + "'");
            }

            if (EmployeeTypeConjunction != DynamicQueryConjunctions.None)
            {
                if (items.Count == 0)
                    items.Add("EmployeeType " + lib.GetOperator(EmployeeTypeOperator) + " '" + e.EmployeeTypeName + "'");
                else
                    items.Add(EmployeeTypeConjunction.ToString() + " EmployeeType " + lib.GetOperator(EmployeeTypeOperator) + " '" + e.EmployeeTypeName + "'");
            }

            if (JobTitleConjunction != DynamicQueryConjunctions.None)
            {
                if (items.Count == 0)
                    items.Add("JobTitle " + lib.GetOperator(JobTitleOperator) + " '" + j.JobTitleName + "'");
                else
                    items.Add(JobTitleConjunction.ToString() + " JobTitle " + lib.GetOperator(JobTitleOperator) + " '" + j.JobTitleName + "'");
            }

            if (StatusConjunction != DynamicQueryConjunctions.None)
            {
                if (items.Count == 0)
                    items.Add("Active " + lib.GetOperator(StatusOperator) + " '" + Status.ToString() + "'");
                else
                    items.Add(StatusConjunction.ToString() + " Status " + lib.GetOperator(StatusOperator) + " '" + Status.ToString() + "'");
            }

            if (YearConjunction != DynamicQueryConjunctions.None)
            {
                if (items.Count == 0)
                    items.Add("FiscalYear " + lib.GetOperator(YearOperator) + " '" + Year + "'");
                else
                    items.Add(YearConjunction.ToString() + " FiscalYear " + lib.GetOperator(YearOperator) + " '" + Year + "'");
            }

            return "( " + string.Join(" ", items.ToArray()) + " )";

        }

        public Dictionary<string, object> GetClausesForQuery()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            DynamicQueryOperatorLibrary lib = new DynamicQueryOperatorLibrary();

            if (BuildingConjunction != DynamicQueryConjunctions.None)
            {
                if (dic.Count == 0)
                    dic.Add("BuildingKey " + lib.GetOperator(BuildingOperator) + " @{0} ", Building);
                else
                    dic.Add(BuildingConjunction.ToString() + " BuildingKey " + lib.GetOperator(BuildingOperator) + " @{0} ", Building);
            }

            if (EmployeeTypeConjunction != DynamicQueryConjunctions.None)
            {
                if (dic.Count == 0)
                    dic.Add("EmployeeTypeKey " + lib.GetOperator(EmployeeTypeOperator) + " @{0} ", EmployeeType);
                else
                    dic.Add(EmployeeTypeConjunction.ToString() + " EmployeeTypeKey " + lib.GetOperator(EmployeeTypeOperator) + " @{0} ", EmployeeType);
            }

            if (JobTitleConjunction != DynamicQueryConjunctions.None)
            {
                if (dic.Count == 0)
                    dic.Add("JobTitleKey " + lib.GetOperator(JobTitleOperator) + " @{0} ", JobTitle);
                else
                    dic.Add(JobTitleConjunction.ToString() + " JobTitleKey " + lib.GetOperator(JobTitleOperator) + " @{0} ", JobTitle);
            }

            if (StatusConjunction != DynamicQueryConjunctions.None)
            {
                if (dic.Count == 0)
                    dic.Add("Status " + lib.GetOperator(StatusOperator) + " @{0} ", Status);
                else
                    dic.Add(StatusConjunction.ToString() + " Status " + lib.GetOperator(StatusOperator) + " @{0}", Status);
            }

            if (YearConjunction != DynamicQueryConjunctions.None)
            {
                if (dic.Count == 0)
                    dic.Add("FiscalYear " + lib.GetOperator(YearOperator) + " @{0} ", Year);
                else
                    dic.Add(YearConjunction.ToString() + " FiscalYear " + lib.GetOperator(YearOperator) + " @{0} ", Year);
            }

            return dic;
        }

    }
}
