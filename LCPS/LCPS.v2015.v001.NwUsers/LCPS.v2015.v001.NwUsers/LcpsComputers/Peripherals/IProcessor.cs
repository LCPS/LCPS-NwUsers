using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers.Peripherals
{
    public interface IProcessor
    {
        string Name { get; set; }

        UInt16 Architecture { get; set; }

        ProcessorArchitectures ArchitectureCaption { get; }

        string Manufacturer { get; set; }

        string Description { get; set; }

        UInt32 CurrentClockSpeed { get; set; }

        UInt32 MaxClockSpeed { get; set; }

        UInt32 NumberOfCores { get; set; }

        string SerialNumber { get; set; }
    }
}
