using System;
using System.Collections.Generic;
using System.Linq;
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


        public string ToFriendlyString()
        {
            LcpsDbContext db = new LcpsDbContext();

            HRBuilding b = db.Buildings.First(x => x.BuildingKey.Equals(Building));
            HREmployeeType e = db.EmployeeTypes.First(x => x.EmployeeTypeLinkId.Equals(EmployeeType));
            HRJobTitle j = db.JobTitles.First(x => x.JobTitleKey.Equals(JobTitle));

            string item = "";
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

        public override string ToString()
        {
            string item = "";

            DynamicQueryOperatorLibrary lib = new DynamicQueryOperatorLibrary();

            List<string> items = new List<string>();

            if (BuildingConjunction != DynamicQueryConjunctions.None)
            {
                if (items.Count() == 0)
                    item += "BuildingKey " + lib.GetOperator(BuildingOperator) + " '" + Building.ToString() + "' ";
                else
                    item += BuildingConjunction.ToString() + " BuildingKey " + lib.GetOperator(BuildingOperator) + " '" + Building.ToString() + "' ";

                items.Add(item);
            }

            if (EmployeeTypeConjunction != DynamicQueryConjunctions.None)
            {
                if (items.Count() == 0)
                    item += "EmployeeTypeKey " + lib.GetOperator(EmployeeTypeOperator) + " '" + EmployeeType.ToString() + "' ";
                else
                    item += EmployeeTypeConjunction.ToString() + " EmployeeTypeKey " + lib.GetOperator(EmployeeTypeOperator) + " '" + EmployeeType.ToString() + "' ";

                items.Add(item);
            }

            if (JobTitleConjunction != DynamicQueryConjunctions.None)
            {
                if (items.Count() == 0)
                    item += "JobTitleKey " + lib.GetOperator(JobTitleOperator) + " '" + JobTitle.ToString() + "' ";
                else
                    item += JobTitleConjunction.ToString() + " JobTitleKey " + lib.GetOperator(JobTitleOperator) + " '" + JobTitle.ToString() + "' ";

                items.Add(item);
            }

            if (StatusConjunction != DynamicQueryConjunctions.None)
            {
                if (items.Count() == 0)
                    item += "Active " + lib.GetOperator(StatusOperator) + " '" + Status.ToString() + "' ";
                else
                    item += StatusConjunction.ToString() + " Status " + lib.GetOperator(StatusOperator) + " '" + Status.ToString() + "' ";

                items.Add(item);
            }

            if (YearConjunction != DynamicQueryConjunctions.None)
            {
                if (items.Count() == 0)
                    item += "FiscalYear " + lib.GetOperator(YearOperator) + " '" + Year + "' ";
                else
                    item += YearConjunction.ToString() + " FiscalYear " + lib.GetOperator(YearOperator) + " '" + Year + "' ";

                items.Add(item);
            }

            if (GroupConjunction == DynamicQueryConjunctions.None)
                return " (" + item + ")";
            else
                return " " + GroupConjunction.ToString() + " (" + item + ")";


        }
    }
}
