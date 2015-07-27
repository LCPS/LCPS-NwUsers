using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers.Peripherals
{
    public interface INetworkAdapter
    {
        string Name { get; set; }
        string MacAddress { get; set; }
        string Manufacturer { get; set; }
    }
}
