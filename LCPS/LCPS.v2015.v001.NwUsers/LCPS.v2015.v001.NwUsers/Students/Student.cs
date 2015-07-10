using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Web.Mvc;

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

        [Display(Name = "Year")]
        [Required]
        [MaxLength(4)]
        public string SchoolYear { get; set; }


        public override string ToString()
        {
            return "(" + StudentId + ") " + SortName + ", " + Gender.ToString() + " - " + Birthdate.ToShortDateString() + ", " + BuildingName + " " + InstructionalLevelName;
        }


        #region Filter

        public static IEnumerable<SelectListItem> GetFilterList()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            SelectListItem noFilter = new SelectListItem() { Text = "No Filter", Value = "" };

            items.Add(noFilter);
            
            
            LcpsDbContext db = new LcpsDbContext();
            
            List<SelectListItem> buildings = (from HRBuilding b in db.Buildings
                                              orderby b.Name
                                              select new SelectListItem() { Text = "Building: " + b.Name, Value = "BuildingKey:" + b.BuildingKey.ToString()}).ToList();

            items.AddRange(buildings);

            List<SelectListItem> grades = (from InstructionalLevel x in db.InstructionalLevels
                                           orderby x.InstructionalLevelName
                                           select new SelectListItem() { Text = "Grade: " + x.InstructionalLevelName, Value = "InstructionalLevelKey:" + x.InstructionalLevelKey.ToString()}).ToList();

            items.AddRange(grades);

            List<SelectListItem> bg = (from Student stu in db.Students
                                       join bld in db.Buildings on stu.BuildingKey equals bld.BuildingKey
                                       join il in db.InstructionalLevels on stu.InstructionalLevelKey equals il.InstructionalLevelKey
                                       orderby bld.Name + il.InstructionalLevelName
                                       select new SelectListItem() { Text = "Building: " + bld.Name + " - " + il.InstructionalLevelName, Value = "BuildingKey:" + bld.BuildingKey.ToString() + ",InstructionalLevelKey:" + il.InstructionalLevelKey.ToString() }
                                      ).Distinct().ToList();

            items.AddRange(bg);

            return items;

        }

        #endregion
    }
}
