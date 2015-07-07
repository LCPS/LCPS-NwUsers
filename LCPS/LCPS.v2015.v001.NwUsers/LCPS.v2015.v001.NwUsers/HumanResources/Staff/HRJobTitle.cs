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

    [Serializable]
    [Table("HRJobTitle", Schema = "HumanResources")]
    public class HRJobTitle : IJobTitle
    {

        HREmployeeType et;

        LCPS.v2015.v001.NwUsers.Infrastructure.LcpsDbContext db = new Infrastructure.LcpsDbContext();

        [Key]
        [Display(Name = "Record ID", Description = "GUID for indexing the record")]
        public Guid JobTitleKey  { get; set; }

        [Display(Name = "Employee Type Link ID", Description = "GUID that links the job title to an employee type")]
        [Index("IX_JobTitleName", IsUnique = true, Order = 1)]
        public virtual Guid EmployeeTypeLinkId { get; set; }

        [Required]
        [ForeignKey("EmployeeTypeLinkId")]
        [XmlIgnore]
        HREmployeeType HREmployeeType { get; set; }

        public string EmployeeTypeId
        {
            get 
            {
                if (HREmployeeType == null)
                    HREmployeeType = db.EmployeeTypes.First(x => x.EmployeeTypeLinkId.Equals(this.EmployeeTypeLinkId));

                return this.HREmployeeType.EmployeeTypeId;
            }
        }

        public string EmployeeTypeName
        {
            get
            {
                if (HREmployeeType == null)
                    HREmployeeType = db.EmployeeTypes.First(x => x.EmployeeTypeLinkId.Equals(this.EmployeeTypeLinkId));

                return this.HREmployeeType.EmployeeTypeName;
            }
        }

        [Display(Name = "ID", Description = "An ID that uniquely identifies the job title in the division")]
        [Required]
        [Index("IX_JobTitleName", IsUnique = true, Order = 2)]
        [MaxLength(30)]
        public string JobTitleId  { get; set; }

        [Display(Name = "Name", Description = "A descriptive name for the job title")]
        [Required]
        [MaxLength(128)]
        public string JobTitleName  { get; set; }










        
    }
}
