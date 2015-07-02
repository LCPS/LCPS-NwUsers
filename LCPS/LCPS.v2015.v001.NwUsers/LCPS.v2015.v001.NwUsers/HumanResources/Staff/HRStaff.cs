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
        private List<HRStaffPosition> _positions;
        private IEnumerable<LcpsStaffEmail> _emails;

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
        public virtual IEnumerable<HRStaffPosition> Positions
        {
            get 
            { 
                if (_positions == null)
                {
                    _positions = db.StaffPositions.Where(x => x.StaffLinkId.Equals(this.StaffLinkId)).ToList();
                }
                return _positions;
            }
            set { _positions = value.ToList(); }
        }

        [XmlIgnore]                
        public virtual IEnumerable<LcpsStaffEmail> Emails
        {
            get
            {
                return db.StaffEmails.Where(x => x.StaffLinkId.Equals(StaffLinkId)).ToList();
            }
            set
            {
                _emails = value;
            }
        }

        #endregion

        
    }
}
