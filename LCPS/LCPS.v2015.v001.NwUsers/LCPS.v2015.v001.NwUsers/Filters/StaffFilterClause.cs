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

        [ForeignKey("FilterId")]
        [Required]
        public virtual MemberFilter Filter { get; set;}

        public Guid OUTemplateId { get; set; }

        [ForeignKey("OUTemplateId")]
        [Required]
        public virtual OUTemplate OUTemplate { get; set; }

        public DynamicQueryConjunctions ClauseConjunction { get; set; }

        public bool BuildingInclude { get; set; }
        public DynamicQueryConjunctions BuildingConjunction { get; set; }
        public DynamicQueryOperators BuildingOperator { get; set; }
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

        public DynamicQueryClauseFilterCollection ToFieldCollection()
        {
            DynamicQueryClauseFilterCollection _list = new DynamicQueryClauseFilterCollection();
            _list.Add(BuildingInclude, BuildingConjunction, "BuildingKey", BuildingOperator, BuildingValue);
            _list.Add(EmployeeTypeInclude, EmployeeTypeConjunction, "EmployeeTypeKey", EmployeeTypeOperator, EmployeeTypeValue);
            _list.Add(JobTitleInclude, JobTitleConjunction, "JobTitleKey", JobTitleOperator, JobTitleValue);
            _list.Add(StatusInclude, StatusConjunction, "Status", StatusOperator, StatusValue);
            _list.Add(LastNameInclude, LastNameConjunction, "LastName", LastNameOperator, LastNameValue);
            _list.Add(StaffIdInclude, StaffIdConjunction, "StaffId", StaffIdOperator, StaffIdValue);
            _list.Add(FiscalYearInclude, FiscalYearConjunction, "FiscalYear", FiscalYearOperator, FiscalYearValue);

            return _list;
        }

        public DynamicQueryStatement ToQueryStatement()
        {
            DynamicQueryClauseFilterCollection _filters = ToFieldCollection();
            DynamicQueryStatement _query = _filters.ToDynamicQuery();
            return _query;
        }

        


    }
}
