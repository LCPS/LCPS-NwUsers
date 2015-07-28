using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Anvil.v2015.v001.Domain.Entities;

using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using LCPS.v2015.v001.NwUsers.HumanResources;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers
{
    [Table("ComputerInfo", Schema = "Computers")]
    public class ComputerInfo : IComputerInfo
    {
        #region Constructors

        public ComputerInfo()
        {
            
        }

        public ComputerInfo(IComputerInfo info)
        {
            AnvilEntity e = new AnvilEntity(info);
            e.CopyTo(this);
            
        }

        #endregion


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

        [Display(Name = "Unit #")]
        public string UnitNumber { get; set; }

        [Display(Name = "Type")]
        public ComputerTypes ComputerType { get; set; }

        [Display(Name = "Description")]
        [MaxLength(256)]
        public string Description { get; set; }

        #region Data Distribution
        
        public void UpdateLDAP()
        {
            if(this.LDAPExists)
            {
                LcpsAdsComputer c = new LcpsAdsComputer(this.LDAPGuid);


                c.RoomName = HRRoom.GetName(this.RoomId);
                c.BuildingName = HRBuilding.GetBuildingName(this.BuildingId);
                c.ComputerType = this.ComputerType;
                c.Description = this.Description;
                c.Manufacturer = this.Manufacturer;
                c.Model = this.Model;
                c.SerialNumber = this.SerialNumber;
                c.UnitNumber = this.UnitNumber;

                c.FillDirectoryEntry(true);
            }
            

        }

        #endregion

        #region Strings

        

        #endregion
    }
}
