using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using LCPS.v2015.v001.NwUsers.HumanResources;

namespace LCPS.v2015.v001.NwUsers.Students
{
    public class Student : IPerson, IStudent
    {
        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string SortName
        {
            get { return LastName + ", " + FirstName; }
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public HRGenders Gender { get; set; }

        public DateTime Birthdate { get; set; }

        public string StudentId { get; set; }

        public Guid InstructionalLevelKey { get; set; }

        public Guid BuildingKey { get; set; }

        public StudentEnrollmentStatus Status { get; set; }

        public string SchoolYear { get; set; }




    }
}
