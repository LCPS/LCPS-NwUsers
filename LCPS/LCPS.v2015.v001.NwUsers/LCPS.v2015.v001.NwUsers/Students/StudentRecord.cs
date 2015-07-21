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
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.Students;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.NwUsers.Infrastructure;

#endregion

namespace LCPS.v2015.v001.NwUsers.Students
{
    partial class StudentRecord
    {

        public HRGenders Gender
        {
            get { return (HRGenders)this.GenderVal; }
            set { this.GenderVal = Convert.ToInt32(value); }
        }

        public StudentEnrollmentStatus Status
        {
            get { return (StudentEnrollmentStatus)this.StatusVal; }
            set { this.StatusVal = Convert.ToInt32(value); }
        }


        public static List<StudentRecord> GetStudents(DynamicQueryStatement dqs)
        {
            try
            {
                StudentsContext db = new StudentsContext();

                return db.StudentRecords.Where(dqs.Query, dqs.Parms)
                    .OrderBy( x => x.LastName + x.FirstName + x.MiddleInitial)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get students from database", ex);
            }
        }


    }
}
