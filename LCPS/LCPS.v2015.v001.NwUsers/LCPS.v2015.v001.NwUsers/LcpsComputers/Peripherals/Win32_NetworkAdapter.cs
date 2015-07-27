using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers.Peripherals
{
    public class Win32_NetworkAdapter : INetworkAdapter
    {

        public string Name { get; set; }

        public string MacAddress { get; set; }

        public string Manufacturer { get; set; }
    }
}
