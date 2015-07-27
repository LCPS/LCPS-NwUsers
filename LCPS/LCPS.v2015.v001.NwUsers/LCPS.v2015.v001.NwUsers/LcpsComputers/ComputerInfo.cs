using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers
{
    [Table("ComputerInfo", Schema = "Computers")]
    public class ComputerInfo : IComputer
    {
        [Key]
        public Guid ComputerId { get; set; }

        [Display(Name = "Computer Name")]
        [MaxLength(128)]
        [Required]
        public string ComputerName { get; set; }

        [Display(Name = "Operating System")]
        [MaxLength(256)]
        [Required]
        public string OSName { get; set; }

        [Display(Name = "Service Pack")]
        [MaxLength(256)]
        [Required]
        public string OSServicePack { get; set; }

        [Display(Name = "SN")]
        [MaxLength(256)]
        [Required]
        public string SerialNumber { get; set; }

        [Display(Name = "Manufacturer")]
        [MaxLength(256)]
        [Required]
        public string Manufacturer { get; set; }

        [Display(Name = "Model")]
        [MaxLength(256)]
        [Required]
        public string Model { get; set; }

        [Display(Name = "Record")]
        public ComputerRecordState RecordState { get; set; }

        [DisplayName("Archive Date")]
        public DateTime AcrchiveDate { get; set; }

        [DisplayName("Archive Author")]
        public string ArchiveAuthor { get; set; }

        [DisplayName("Active Directory Id")]
        public Guid LDAPGuid {get; set;}

        [DisplayName("IN ADS")]
        public bool LDAPExists { get; set; }

        [DisplayName("Building")]
        public Guid BuildingId { get; set; }

        [DisplayName("Room")]
        public Guid RoomId { get; set; }
    }
}
