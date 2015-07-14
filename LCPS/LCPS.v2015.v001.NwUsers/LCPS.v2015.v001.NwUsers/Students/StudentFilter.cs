#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using LCPS.v2015.v001.NwUsers.Infrastructure;

#endregion

namespace LCPS.v2015.v001.NwUsers.Students
{
    public class StudentFilter : DynamicQueryClauseFilterCollection
    {
        #region Constructors

        public StudentFilter()
        {
            Add(new DynamicQueryClauseField()
                {
                    Include = false,
                    Conjunction = DynamicQueryConjunctions.And,
                    FieldName = "BuildingKey",
                    Operator = DynamicQueryOperators.Equals,
                    Value = Guid.Empty
                });

            Add(new DynamicQueryClauseField()
            {
                Include = false,
                Conjunction = DynamicQueryConjunctions.And,
                FieldName = "InstructionalLevelKey",
                Operator = DynamicQueryOperators.Equals,
                Value = Guid.Empty
            });

            Add(new DynamicQueryClauseField()
            {
                Include = false,
                Conjunction = DynamicQueryConjunctions.And,
                FieldName = "Status",
                Operator = DynamicQueryOperators.Equals,
                Value = StudentEnrollmentStatus.None
            });

            Add(new DynamicQueryClauseField()
            {
                Include = false,
                Conjunction = DynamicQueryConjunctions.And,
                FieldName = "Name",
                Operator = DynamicQueryOperators.Contains,
                Value = String.Empty
            });

            Add(new DynamicQueryClauseField()
            {
                Include = false,
                Conjunction = DynamicQueryConjunctions.And,
                FieldName = "StudentId",
                Operator = DynamicQueryOperators.Contains,
                Value = String.Empty
            });
        }

        #endregion

        #region Properties

        public DynamicQueryClauseField Building
        {
            get { return Get("BuildingKey"); }
        }

        [Display(Name = "Level")]
        public DynamicQueryClauseField InstructionalLevel
        {
            get { return Get("InstructionalLevelKey"); }
        }

        public DynamicQueryClauseField Status
        {
            get { return Get("Status"); }
        }

        public DynamicQueryClauseField Name
        {
            get { return Get("Name"); }
        }

        public DynamicQueryClauseField StudentId
        {
            get { return Get("StudentId"); }
        }

        #endregion

        #region Seed

        public void Seed(LcpsDbContext db)
        {
            var props = from p in this.GetType().GetProperties()
                        let attr = p.GetCustomAttributes(typeof(DynamicQueryAttribute), true)
                        where attr.Length == 1
                        select new { Property = p, Attribute = attr.First() as DynamicQueryAttribute };

            foreach(var p in props)
            {

            }
        }

        #endregion
    }
}
