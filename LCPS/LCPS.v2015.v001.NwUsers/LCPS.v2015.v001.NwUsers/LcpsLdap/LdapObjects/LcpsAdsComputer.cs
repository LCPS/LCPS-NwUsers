using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices;

using LCPS.v2015.v001.NwUsers.LcpsComputers;


namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    public class LcpsAdsComputer : LcpsAdsObject, IComputerInfo
    {
        #region Constructors

        public LcpsAdsComputer()
        {

        }

        public LcpsAdsComputer(Guid id)
            : base(id)
        {
        }

        public LcpsAdsComputer(DirectoryEntry e)
            : base(e)
        {
        }


        #endregion

        #region Properties

        public string BuildingName { get; set; }

        public string RoomName { get; set; }

        public string UnitNumber { get; set; }

        public Guid ComputerId { get; set; }

        public string ComputerName { get; set; }

        public string Description { get; set; }

        public string OSName { get; set; }

        public string OSServicePack { get; set; }

        public string SerialNumber { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public ComputerTypes ComputerType { get; set; }

        public string ComputerTypeName
        { 
            get { return this.ComputerType.ToString(); }
            set {
                if (value == null)
                    this.ComputerType = ComputerTypes.Unknown;
                else
                    this.ComputerType = (ComputerTypes)System.Enum.Parse(typeof(ComputerTypes), value);
            }
        }

        #endregion

        #region Field Maps

        public override void FillFieldMaps()
        {
            AddFieldMap("ComputerTypeName", "businessCategory");
            AddFieldMap("ComputerName", "cn");
            AddFieldMap("BuildingName", "l");
            AddFieldMap("RoomName", "sn");
            AddFieldMap("Description", "description");
            AddFieldMap("UnitNumber", "carLicense");
            AddFieldMap("SerialNumber", "serialNumber");
            AddFieldMap("Manufacturer", "company");
            AddFieldMap("Model", "adminDescription");
            AddFieldMap("OSName", "operatingSystem");
            AddFieldMap("OSServicePack", "operatingSystemServicePack");
        }

        #endregion


    }
}
