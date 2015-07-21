using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

using Anvil.v2015.v001.Domain.Entities.DynamicFilters;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.Students;

namespace LCPS.v2015.v001.NwUsers.Filters
{
    [Table("StudentFilterClause", Schema = "Filters")]
    public class StudentFilterClause 
    {
        private LcpsDbContext _dbContext;

        [Key]
        public Guid StudentFilterClauseId { get; set; }

        public Guid FilterId { get; set; }

        public int SortIndex { get; set; }

        public DynamicQueryConjunctions ClauseConjunction { get; set; }

        public bool BuildingInclude { get; set; }
        public DynamicQueryConjunctions BuildingConjunction { get; set; }
        public DynamicQueryOperators BuildingOperator { get; set; }
        [Display(Name = "Building")]
        public Guid BuildingValue { get; set; }

        public bool InstructionalLevelInclude { get; set; }
        public DynamicQueryConjunctions InstructionalLevelConjunction { get; set; }
        public DynamicQueryOperators InstructionalLevelOperator { get; set; }
        [Display(Name = "Level")]
        public Guid InstructionalLevelValue { get; set; }

        public bool StatusInclude { get; set; }
        public DynamicQueryConjunctions StatusConjunction { get; set; }
        public DynamicQueryOperators StatusOperator { get; set; }
        [Display(Name = "Status")]
        public StudentEnrollmentStatus StatusValue { get; set; }

        public bool NameInclude { get; set; }
        public DynamicQueryConjunctions NameConjunction { get; set; }
        public DynamicQueryOperators NameOperator { get; set; }
        [Display(Name = "Last Name")]
        public string NameValue { get; set; }

        public bool StudentIdInclude { get; set; }
        public DynamicQueryConjunctions StudentIdConjunction { get; set; }
        public DynamicQueryOperators StudentIdOperator { get; set; }
        [Display(Name = "ID")]
        public string StudentIdValue { get; set; }

    }
}
