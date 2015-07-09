using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LCPS.v2015.v001.NwUsers.Students
{
    public enum StudentEnrollmentStatus
    {
        None = 0,
        Unenrolled = 1,
        Enrolled = 2
    }

    public interface IStudent
    {
        string StudentId { get; set; }
        
        Guid InstructionalLevelKey { get; set; }
        
        Guid BuildingKey { get; set; }

        StudentEnrollmentStatus Status { get; set; }

        string SchoolYear { get; set; }

    }
}
