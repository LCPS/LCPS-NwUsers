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
#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    [Table("HRStaff", Schema = "HumanResources")]
    public class HRStaff : HumanResources.HRPerson
    {
        #region Fields

        private LcpsDbContext db = new LcpsDbContext();
        private HRStaffPositionCollection _positions = new HRStaffPositionCollection();
        private IEnumerable<LcpsStaffEmail> _emails;

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

        #region Properties

        [Key]
        public Guid StaffLinkId { get; set; }

        [Display(Name = "Staff ID", Description = "An ID that uniquely identifies the staff member in the division")]
        [Index("IX_StaffID", IsUnique = true)]
        [Required]
        [MaxLength(25)]
        public string StaffId { get; set; }

        [XmlIgnore]
        public virtual HRStaffPositionCollection Positions
        {
            get
            {
                return _positions;
            }
            set { _positions = value; }
        }

        [XmlIgnore]
        public virtual IEnumerable<LcpsStaffEmail> Emails
        {
            get
            {
                return new List<LcpsStaffEmail>(); // return db.StaffEmails.Where(x => x.StaffLinkId.Equals(StaffLinkId)).ToList();
            }
            set
            {
                _emails = value;
            }
        }

        #endregion

        public override string ToString()
        {
            return "(" + StaffId + "), " + LastName + ", " + FirstName + " " + MiddleInitial + " (" + Gender.ToString() + Birthdate.ToShortDateString() + ")";
        }
    }
}
