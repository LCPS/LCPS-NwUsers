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


#endregion

namespace LCPS.v2015.v001.NwUsers.Students
{
    public class StudentFilter : DynamicQueryClauseCollection
    {

        #region Constructors 

        public StudentFilter(Guid antecedentId)
        {
            Add(new DynamicQueryClauseField()
                {
                    ClauseId = antecedentId,
                    Include = false,
                    Conjunction = DynamicQueryConjunctions.And,
                    FieldName = "BuildingKey",
                    Operator = DynamicQueryOperators.Equals,
                    Value = Guid.Empty
                });

            Add(new DynamicQueryClauseField()
            {
                ClauseId = antecedentId,
                Include = false,
                Conjunction = DynamicQueryConjunctions.And,
                FieldName = "InstructionalLevelKey",
                Operator = DynamicQueryOperators.Equals,
                Value = Guid.Empty
            });

            Add(new DynamicQueryClauseField()
            {
                ClauseId = antecedentId,
                Include = false,
                Conjunction = DynamicQueryConjunctions.And,
                FieldName = "Status",
                Operator = DynamicQueryOperators.Equals,
                Value = StudentEnrollmentStatus.None
            });

            Add(new DynamicQueryClauseField()
            {
                ClauseId = antecedentId,
                Include = false,
                Conjunction = DynamicQueryConjunctions.And,
                FieldName = "Name",
                Operator = DynamicQueryOperators.Contains,
                Value = String.Empty
            });

            Add(new DynamicQueryClauseField()
            {
                ClauseId = antecedentId,
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

    }
}
