using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    [Table("HRJobTitle", Schema = "HumanResources")]
    public class HRJobTitle
    {
        [Key]
        [Display(Name = "Record ID", Description = "GUID for indexing the record")]
        public Guid RecordId { get; set; }

        [Display(Name = "Employee Type Link ID", Description = "GUID that links the job title to an employee type")]
        [Index("IX_JobTitleName", IsUnique = true, Order = 1)]
        public virtual Guid EmployeeTypeLinkId { get; set; }

        [Required]
        [ForeignKey("EmployeeTypeLinkId")]
        [XmlIgnore]
        public virtual HREmployeeType HREmployeeType { get; set; }

        [Display(Name = "ID", Description = "An ID that uniquely identifies the job title in the division")]
        [Required]
        [Index("IX_JobTitleName", IsUnique = true, Order = 2)]
        [MaxLength(30)]
        public string JobTitleId { get; set; }

        [Display(Name = "Name", Description = "A descriptive name for the job title")]
        [Required]
        [MaxLength(128)]
        public string JobTitleName { get; set; }

    }
}
