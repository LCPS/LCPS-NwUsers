using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.HumanResources
{
    public interface IRoom
    {
        Guid RoomKey { get; set; }
        Guid BuildingId { get; set; }
        string RoomNumber { get; set; }
        string RoomId { get; set; }
        HRRoomTypes RoomType { get; set; }
        string PrimaryOccupant { get; set; }
    }
}
