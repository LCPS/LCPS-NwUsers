using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using System.DirectoryServices;

using System.Management;

using LCPS.v2015.v001.NwUsers.LcpsComputers.Peripherals;
using LCPS.v2015.v001.NwUsers.LcpsComputers.IO;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers
{
    public class RemoteComputer : IComputerInfo
    {

        #region Constants

        public const string PathExp = @"\\{0}\Root\CimV2";

        #endregion

        #region Fields

        private ManagementScope _scope;
        private Win32_Share[] _shares;

        #endregion

        #region Constructors

        public RemoteComputer(string computerName, string userName, string password)
        {
            this.ComputerName = computerName;
            this.UserName = userName;
            this.Password = password;
        }

        #endregion

        #region Properties

        public string UserName { get; set; }

        public string Password { get; set; }

        public Guid ComputerId { get; set; }

        public string ComputerName { get; set; }

        [Display(Name = "Operating System")]
        public string OSName { get; set; }

        [Display(Name = "Service Pack")]
        public string OSServicePack { get; set; }

        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public Guid LDAPGuid { get; set; }

        public bool LDAPExists { get; set; }

        public ComputerTypes ComputerType { get; set; }

        #endregion

        #region Peripherals

        public Win32_NetworkAdapter[] NetworkAdapters { get; set; }

        public Win32_Processor[] Processors { get; set; }

        #endregion

        #region Get

        private void GetOperatingSystem(ManagementScope scope)
        {
            string q = "SELECT * FROM Win32_OperatingSystem";
            ManagementObjectSearcher srch = new ManagementObjectSearcher(scope, new ObjectQuery(q));
            foreach (ManagementObject i in srch.Get())
            {
                OSName = i.GetPropertyValue("Caption").ToString();
                OSServicePack = (i.GetPropertyValue("CSDVersion") != null) ? i.GetPropertyValue("CSDVersion").ToString() : "";
            }
        }

        private void GetBios(ManagementScope scope)
        {
            string q = "SELECT * FROM Win32_BIOS";
            ManagementObjectSearcher srch = new ManagementObjectSearcher(scope, new ObjectQuery(q));
            foreach (ManagementObject i in srch.Get())
            {
                this.SerialNumber = i.GetPropertyValue("SerialNumber").ToString();
            }

        }

        private void GetComputerSystem(ManagementScope scope)
        {
            string q = "SELECT * FROM Win32_ComputerSystem";
            ManagementObjectSearcher srch = new ManagementObjectSearcher(scope, new ObjectQuery(q));
            foreach (ManagementObject i in srch.Get())
            {
                this.Manufacturer = i.GetPropertyValue("Manufacturer").ToString();
                this.Model = i.GetPropertyValue("Model").ToString();
            }
        }



        private ArrayList GetList(ManagementScope scope, System.Type t, string q)
        {
            ArrayList items = new ArrayList();

            ManagementObjectSearcher srch = new ManagementObjectSearcher(scope, new ObjectQuery(q));
            foreach (ManagementObject i in srch.Get())
            {
                object n = Activator.CreateInstance(t, true);

                foreach(PropertyInfo tp in t.GetProperties())
                {
                    if (tp.CanWrite)
                    {
                        PropertyData p = (from PropertyData x in i.Properties 
                                              where x.Name.ToLower() == tp.Name.ToLower()
                                              select x).FirstOrDefault();
                        if(p != null)
                        {
                            object v = p.Value;

                            if (v != null)
                                tp.SetValue(n, v, null);
                        }
                    }
                }

                items.Add(n);
            }

            return items;
        }

        #endregion

        #region IO

        public Win32_Share[] SharedFolders
        {
            get
            {
                if (_scope == null)
                    Refresh();

                if(_shares == null)
                {
                    string q = "SELECT * FROM Win32_Share WHERE Type = 0";
                    _shares = GetList(_scope, typeof(Win32_Share), q).Cast<Win32_Share>().ToArray();
                }

                return _shares;
            }
        }

        #endregion


        public string BuildingName { get; set; }

        public string RoomName { get; set; }

        public string UnitNumber { get; set; }

        public void DBAcrchive(ComputerInfo m, string author)
        {
            LcpsAdsDomain dom = LcpsAdsDomain.Default;
            string fltr = "(&(cn=" + this.ComputerName + "))";
            DirectorySearcher s = new DirectorySearcher(dom.DirectoryEntry, fltr);
            SearchResult r = s.FindOne();
            if (r.GetDirectoryEntry() == null)
                m.LDAPExists = false;
            else
            {
                m.LDAPExists = true;
                LcpsAdsComputer ldapC = new LcpsAdsComputer(r.GetDirectoryEntry());
                m.LDAPGuid = ldapC.ObjectGuid;
            }

            ComputerInfo i = new ComputerInfo()
            {
                ComputerId = Guid.NewGuid(),
                LDAPExists = m.LDAPExists,
                LDAPGuid = m.LDAPGuid,
                AcrchiveDate = DateTime.Now,
                BuildingId = m.BuildingId,
                RoomId = m.RoomId,
                UnitNumber = m.UnitNumber,
                ArchiveAuthor = author,
                ComputerName = this.ComputerName,
                ComputerType = this.ComputerType,
                Description = this.Description,
                Manufacturer = this.Manufacturer,
                Model = this.Model,
                SerialNumber = this.SerialNumber,
                OSName = this.OSName,
                OSServicePack = this.OSServicePack,
                RecordState = ComputerRecordState.Active
            };

            List<ArchiveNic> _nics = new List<ArchiveNic>();
            foreach (Win32_NetworkAdapter n in this.NetworkAdapters)
            {
                ArchiveNic archN = new ArchiveNic()
                {
                    RecordId = Guid.NewGuid(),
                    ComputerId = i.ComputerId,
                    MacAddress = n.MacAddress,
                    Manufacturer = n.Manufacturer,
                    Name = n.Name
                };
                _nics.Add(archN);
            }

            LcpsDbContext db = new LcpsDbContext();

            foreach (ComputerInfo ac in db.Computers)
            {
                ac.RecordState = ComputerRecordState.Archive;
                db.Entry(ac).State = System.Data.Entity.EntityState.Modified;
            }

            db.Computers.Add(i);
            db.ArchivedNics.AddRange(_nics);
            db.SaveChanges();
    
        }

        public void Refresh()
        {
            _scope = new ManagementScope(string.Format(PathExp, this.ComputerName));
            if (this.ComputerName.ToLower() != Environment.MachineName.ToLower())
            {
                _scope.Options = new ConnectionOptions()
                {
                    Username = this.UserName,
                    Password = this.Password,
                    Authentication = AuthenticationLevel.PacketPrivacy,
                    Impersonation = ImpersonationLevel.Impersonate
                };
            }

            _scope.Connect();
            GetOperatingSystem(_scope);
            GetComputerSystem(_scope);
            GetBios(_scope);

            string nQry = "SELECT * FROM Win32_NetworkAdapter WHERE Manufacturer <> 'Microsoft' AND Manufacturer <> 'Symantec' AND Manufacturer <> 'VMware, Inc.'";
            this.NetworkAdapters = GetList(_scope, typeof(Win32_NetworkAdapter), nQry).ToArray().Cast<Win32_NetworkAdapter>().ToArray();

            string cpuQry = "SELECT * FROM Win32_Processor";
            this.Processors = GetList(_scope, typeof(Win32_Processor), cpuQry).ToArray().Cast<Win32_Processor>().ToArray();

            ComputerType = ComputerTypes.Unknown;

            if (Model.ToLower().StartsWith("latitude"))
                ComputerType = ComputerTypes.Laptop;

            if (Model.ToLower().StartsWith("optiplex"))
                ComputerType = ComputerTypes.PC;

            if (Manufacturer.ToLower().StartsWith("asus"))
                ComputerType = ComputerTypes.Tablet;
        }

        #region Conversions

        public ComputerInfo ToComputerInfo()
        {
            LcpsDbContext db = new LcpsDbContext();
            ComputerInfo dbC = db.Computers.FirstOrDefault(x => x.RecordState == ComputerRecordState.Active & x.ComputerName.ToLower() == this.ComputerName.ToLower());
            ComputerInfo wmi = new ComputerInfo(this);
            if(dbC != null)
            {
                wmi.BuildingId = dbC.BuildingId;
                wmi.RoomId = dbC.RoomId;
                wmi.UnitNumber = dbC.UnitNumber;
            }
            return wmi;
        }

        #endregion
    }
}
