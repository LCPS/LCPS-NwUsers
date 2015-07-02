using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace LCPS.v2015.v001.NwUsers.Security
{
    [Table("LcpsStaffEmail", Schema = "Security")]
    public class LcpsStaffEmail : LcpsEmail
    {

        LcpsDbContext db = new LcpsDbContext();
        HRStaff _staff;

        [Key]
        public Guid EmailId { get; set; }

        public Guid StaffLinkId { get; set; }

        [Required]
        [ForeignKey("StaffLinkId")]
        [XmlIgnore]
        public virtual HRStaff StaffMember
        {
            get { return db.StaffMembers.FirstOrDefault(x => x.StaffLinkId.Equals(StaffLinkId)); }
            set { _staff = value;  }
        }


    }
}
