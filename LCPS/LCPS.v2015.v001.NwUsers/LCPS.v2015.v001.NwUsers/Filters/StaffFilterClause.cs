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

        public DynamicQueryConjunctions ClauseConjunction { get; set; }

        public bool BuildingInclude { get; set; }
        public DynamicQueryConjunctions BuildingConjunction { get; set; }
        public DynamicQueryOperators BuildingOperator { get; set; }
        [Display(Name = "Building")]
        public Guid BuildingValue { get; set; }

        public bool EmployeeTypeInclude { get; set; }
        public DynamicQueryConjunctions EmployeeTypeConjunction { get; set; }
        public DynamicQueryOperators EmployeeTypeOperator { get; set; }
        [Display(Name = "Type")]
        public Guid EmployeeTypeValue { get; set; }

        public bool JobTitleInclude { get; set; }
        public DynamicQueryConjunctions JobTitleConjunction { get; set; }
        public DynamicQueryOperators JobTitleOperator { get; set; }
        [Display(Name = "Title")]
        public Guid JobTitleValue { get; set; }

        public bool StatusInclude { get; set; }
        public DynamicQueryConjunctions StatusConjunction { get; set; }
        public DynamicQueryOperators StatusOperator { get; set; }
        [Display(Name = "Status")]
        public HRStaffPositionQualifier StatusValue { get; set; }

        public bool LastNameInclude { get; set; }
        public DynamicQueryConjunctions LastNameConjunction { get; set; }
        public DynamicQueryOperators LastNameOperator { get; set; }
        [Display(Name = "Surname")]
        public string LastNameValue { get; set; }

        public bool StaffIdInclude { get; set; }
        public DynamicQueryConjunctions StaffIdConjunction { get; set; }
        public DynamicQueryOperators StaffIdOperator { get; set; }
        [Display(Name = "ID")]
        public string StaffIdValue { get; set; }

        public bool FiscalYearInclude { get; set; }
        public DynamicQueryConjunctions FiscalYearConjunction { get; set; }
        public DynamicQueryOperators FiscalYearOperator { get; set; }
        [Display(Name = "Year")]
        public string FiscalYearValue { get; set; }

    }
}
