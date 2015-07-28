using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers.Peripherals
{
    [Table("ArchiveNic", Schema = "Computers")]
    public class ArchiveNic : INetworkAdapter
    {
        [Key]
        public Guid RecordId { get; set; }

        public Guid ComputerId { get; set; }

        public string Name { get; set; }

        public string MacAddress { get; set; }

        public string Manufacturer { get; set; }
    }
}
