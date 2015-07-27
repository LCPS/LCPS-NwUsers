using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers.Peripherals
{
    public class Win32_Processor : IProcessor
    {
        public string Name { get; set; }

        public UInt16 Architecture { get; set; }

        public ProcessorArchitectures ArchitectureCaption
        { get { return (ProcessorArchitectures)Architecture; } }

        public string Manufacturer { get; set; }

        public string Description { get; set; }

        public uint CurrentClockSpeed { get; set; }

        public uint MaxClockSpeed { get; set; }

        public uint NumberOfCores { get; set; }

        public string SerialNumber { get; set; }
    }
}
