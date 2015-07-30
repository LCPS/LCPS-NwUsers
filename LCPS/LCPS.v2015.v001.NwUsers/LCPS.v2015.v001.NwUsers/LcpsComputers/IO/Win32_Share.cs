using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers.IO
{
    public class Win32_Share
    {
        public enum ShareTypes : uint
        {
            DiskDrive = 0,
            PrintQueue = 1,
            Device = 2,
            IPC = 3,
            DiskDriveAdmin = 2147483648,
            PrintQueueAdmin = 2147483649,
            DeviceAdmin = 2147483650,
            IPCAdmin = 2147483651
        }

        public UInt32 AccessMask { get; set; }
        public bool AllowMaximum { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public DateTime InstallDate { get; set; }
        public UInt32 MaximumAllowed { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Status { get; set; }
        public UInt32 Type { get; set; }

        public ShareTypes ShareType
        {
            get
            {
                return (ShareTypes)this.Type;
            }
            set
            {
                this.Type = (uint)value;
            }
        }
    }
}
