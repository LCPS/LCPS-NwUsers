using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPS.v2015.v001.NwUsers.HumanResources
{
    [Table("HRRoom", Schema = "HumanResources")]
    public class HRRoom : IRoom
    {
        [Key]
        public Guid RoomKey { get; set; }

        [Index("IX_RoomName", IsUnique = true, Order = 1)]
        public Guid BuildingId { get; set; }

        [Index("IX_RoomName", IsUnique = true, Order = 2)]
        [Display(Name = "Room")]
        [MaxLength(128)]
        [Required]
        public string RoomNumber { get; set; }

        [Display(Name = "Id")]
        [MaxLength(128)]
        [Required]
        public string RoomId { get; set; }

        [Display(Name = "Type")]
        public HRRoomTypes RoomType { get; set; }

        [Display(Name = "Occupant")]
        [MaxLength(128)]
        public string PrimaryOccupant { get; set; }
    }
}
