using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.NwUsers.Students
{
    public class Student : IPerson, IStudent
    {
        LcpsDbContext db;

        [Key]
        public Guid StudentKey { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [MaxLength(75)]
        public string FirstName { get; set; }

        [Display(Name = "MI")]
        [Required]
        [MaxLength(10)]
        public string MiddleInitial { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [MaxLength(75)]
        public string LastName { get; set; }

        [Display(Name = "Student Name")]
        public string SortName
        {
            get { return LastName + ", " + FirstName; }
        }

        [Display(Name = "Student Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        
        [Required]
        public HRGenders Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Student Number")]
        [Required]
        [MaxLength(50)]
        [Index("IX_StudentUnique", IsUnique = true, Order = 1)]
        public string StudentId { get; set; }

        [Index("IX_StudentUnique", IsUnique = true, Order = 2)]
        public Guid InstructionalLevelKey { get; set; }

        public LcpsDbContext DbContext
        {
            get
            {
                if (db == null)
                    db = new LcpsDbContext();
                return db;
            }
        }

        [Display(Name = "Grade/Level")]
        public string InstructionalLevelName
        {
            get
            {
                InstructionalLevel l = DbContext.InstructionalLevels.FirstOrDefault(x => x.InstructionalLevelKey.Equals(InstructionalLevelKey));
                if (l == null)
                    return "";
                else
                    return l.InstructionalLevelName;
            }
        }

        public Guid BuildingKey { get; set; }

        [Display(Name = "School")]
        public string BuildingName { 
            get
            {
                HRBuilding b = DbContext.Buildings.First(x => x.BuildingKey.Equals(BuildingKey));
                if(b != null)
                    return b.Name;
                else
                    return "";
            }
        }

        public StudentEnrollmentStatus Status { get; set; }

        [Display(Name = "Cohort")]
        [Required]
        [MaxLength(4)]
        public string SchoolYear { get; set; }


        public override string ToString()
        {
            return "(" + StudentId + ") " + SortName + ", " + Gender.ToString() + " - " + Birthdate.ToShortDateString() + ", " + BuildingName + " " + InstructionalLevelName;
        }

    }
}
