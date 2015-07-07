#region Using

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Serialization;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.Security;
using LCPS.v2015.v001.NwUsers.HumanResources;
#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    [Table("HRStaff", Schema = "HumanResources")]
    public class HRStaff : IPerson, IStaff 
    {
        #region Fields

        private LcpsDbContext db = new LcpsDbContext();

        #endregion


        #region Constructors

        public HRStaff()
        { }


        public HRStaff(Guid staffLinkId)
        {
            /*
            HRStaff dbStaff = db.StaffMembers.FirstOrDefault(x => x.StaffLinkId.Equals(staffLinkId));
            if(dbStaff != null)
            {
                this.StaffLinkId = staffLinkId;
                this.StaffId = dbStaff.StaffId;
                this.FirstName = dbStaff.FirstName;
                this.MiddleInitial = dbStaff.MiddleInitial;
                this.LastName = dbStaff.LastName;
                this.Birthdate = dbStaff.Birthdate;
                this.Gender = dbStaff.Gender;
                this.Email = dbStaff.Email;
                _positions = new HRStaffPositionCollection(staffLinkId);
             * }
             */ 
            
        }

        #endregion

        #region Staff Properties and Methods

        [Key]
        public Guid StaffKey { get; set; }

        [Display(Name = "Staff ID", Description = "An ID that uniquely identifies the staff member in the division")]
        [Index("IX_StaffID", IsUnique = true)]
        [Required]
        [MaxLength(25)]
        public string StaffId { get; set; }

        #endregion

        public override string ToString()
        {
            return "(" + StaffId + "), " + LastName + ", " + FirstName + " " + MiddleInitial + " (" + Gender.ToString() + " - " + Birthdate.ToShortDateString() + ")";
        }

        #region Person Properties and Methods

        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(75)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Initial")]
        [MaxLength(3)]
        public string MiddleInitial { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(75)]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string StaffEmail { get; set; }


        [Display(Name = "Name")]
        public string SortName
        {
            get { return LastName + ", " + FirstName; }
        }

        [Display(Name = "Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public HRGenders Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        #endregion

        
    }
}
