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

using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#endregion

namespace LCPS.v2015.v001.NwUsers.Filters
{
    [Table("StaffFilterClause", Schema = "Filters")]
    public class StaffFilterClause
    {
        [Key]
        public Guid StaffFilterClauseId { get; set; }

        public Guid FilterId { get; set; }

        public int SortIndex { get; set; }

        [ForeignKey("FilterId")]
        public virtual MemberFilter Filter { get; set; }

        public DynamicQueryConjunctions ClauseConjunction { get; set; }

        public bool BuildingInclude { get; set; }
        public DynamicQueryConjunctions BuildingConjunction { get; set; }
        public DynamicQueryOperators BuildingOperator { get; set; }
        [Display(Name = "Building")]
        public Guid BuildingValue { get; set; }

        public bool EmployeeTypeInclude { get; set; }
        public DynamicQueryConjunctions EmployeeTypeConjunction { get; set; }
        public DynamicQueryOperators EmployeeTypeOperator { get; set; }
        public Guid EmployeeTypeValue { get; set; }

        public bool JobTitleInclude { get; set; }
        public DynamicQueryConjunctions JobTitleConjunction { get; set; }
        public DynamicQueryOperators JobTitleOperator { get; set; }
        public Guid JobTitleValue { get; set; }

        public bool StatusInclude { get; set; }
        public DynamicQueryConjunctions StatusConjunction { get; set; }
        public DynamicQueryOperators StatusOperator { get; set; }
        public HRStaffPositionQualifier StatusValue { get; set; }

        public bool LastNameInclude { get; set; }
        public DynamicQueryConjunctions LastNameConjunction { get; set; }
        public DynamicQueryOperators LastNameOperator { get; set; }
        public string LastNameValue { get; set; }

        public bool StaffIdInclude { get; set; }
        public DynamicQueryConjunctions StaffIdConjunction { get; set; }
        public DynamicQueryOperators StaffIdOperator { get; set; }
        public string StaffIdValue { get; set; }

        public bool FiscalYearInclude { get; set; }
        public DynamicQueryConjunctions FiscalYearConjunction { get; set; }
        public DynamicQueryOperators FiscalYearOperator { get; set; }
        public string FiscalYearValue { get; set; }

        public DynamicQueryClause ToDynamicQueryClause()
        {
            DynamicQueryClause _list = new DynamicQueryClause();

            _list.ClauseConjunction = ClauseConjunction;

            if (BuildingInclude)
                _list.Add(BuildingInclude, BuildingConjunction, "BuildingKey", BuildingOperator, BuildingValue);

            if (EmployeeTypeInclude)
                _list.Add(EmployeeTypeInclude, EmployeeTypeConjunction, "EmployeeTypeKey", EmployeeTypeOperator, EmployeeTypeValue);

            if (JobTitleInclude)
                _list.Add(JobTitleInclude, JobTitleConjunction, "JobTitleKey", JobTitleOperator, JobTitleValue);

            if (StatusInclude)
                _list.Add(StatusInclude, StatusConjunction, "Status", StatusOperator, StatusValue);

            if (LastNameInclude)
                _list.Add(LastNameInclude, LastNameConjunction, "LastName", LastNameOperator, LastNameValue);

            if (StaffIdInclude)
                _list.Add(StaffIdInclude, StaffIdConjunction, "StaffId", StaffIdOperator, StaffIdValue);

            if (FiscalYearInclude)
                _list.Add(FiscalYearInclude, FiscalYearConjunction, "FiscalYear", FiscalYearOperator, FiscalYearValue);

            return _list;
        }

        public DynamicQueryStatement ToQueryStatement()
        {
            DynamicQueryClause _filters = ToDynamicQueryClause();
            DynamicQueryStatement _query = _filters.ToDynamicQuery();
            return _query;
        }

        public override string ToString()
        {
            DynamicQueryClause c = ToDynamicQueryClause();
            LcpsDbContext db = new LcpsDbContext();

            DynamicQueryClauseField bf = c.FirstOrDefault(x => x.FieldName == "BuildingKey");
            if (bf != null)
            {
                int bx = c.IndexOf(bf);
                c[bx].FieldName = "Building";
                Guid bid = new Guid(bf.Value.ToString());
                c[bx].Value = db.Buildings.First(x => x.BuildingKey.Equals(bid)).Name;
            }

            DynamicQueryClauseField ef = c.FirstOrDefault(x => x.FieldName == "EmployeeTypeKey");
            if (ef != null)
            {
                int i = c.IndexOf(ef);
                c[i].FieldName = "EmployeeType";
                Guid id = new Guid(ef.Value.ToString());
                c[i].Value = db.EmployeeTypes.First(x => x.EmployeeTypeLinkId.Equals(id)).EmployeeTypeName;
            }

            DynamicQueryClauseField jf = c.FirstOrDefault(x => x.FieldName == "JobTitleKey");
            if (jf != null)
            {
                int i = c.IndexOf(jf);
                c[i].FieldName = "JobTitle";
                Guid id = new Guid(jf.Value.ToString());
                c[i].Value = db.JobTitles.First(x => x.JobTitleKey.Equals(id)).JobTitleName;
            }





            return c.ToString();
            
        }

        #region Get

        public static List<StaffFilterClause> GetClause(Guid filterId)
        {
            try
            {
                LcpsDbContext db = new LcpsDbContext();
                List<StaffFilterClause> items = db.StaffFilterClauses
                    .Where(x => x.FilterId.Equals(filterId))
                    .OrderBy(x => x.SortIndex)
                    .ToList();

                return items;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get clauses from database", ex);
            }
        }

        #endregion


    }
}
