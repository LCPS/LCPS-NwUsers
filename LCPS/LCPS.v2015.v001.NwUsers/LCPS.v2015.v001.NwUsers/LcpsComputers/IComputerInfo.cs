using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers
{
    public interface IComputerInfo
    {
         Guid ComputerId { get; set; }

         string ComputerName { get; set; }

         string Description { get; set; }

         string OSName { get; set; }

         string OSServicePack { get; set; }

         string SerialNumber { get; set; }

         string Manufacturer { get; set; }

         string Model { get; set; }

         ComputerTypes ComputerType { get; set; }
    }
}
