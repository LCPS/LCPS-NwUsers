using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

using System.Management;

using LCPS.v2015.v001.NwUsers.LcpsComputers.Peripherals;

namespace LCPS.v2015.v001.NwUsers.LcpsComputers
{
    public class RemoteComputer : IComputer
    {

        #region Constants

        public const string PathExp = @"\\{0}\Root\CimV2";

        #endregion

        #region Constructors

        public RemoteComputer(string computerName, string userName, string password)
        {
            this.ComputerName = computerName;
            this.UserName = UserName;
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

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public Guid LDAPGuid { get; set; }

        public bool LDAPExists { get; set; }



        #endregion

        #region Peripherals

        public Win32_NetworkAdapter[] NetworkAdapters { get; set; }

        public Win32_Processor[] Processors { get; set; }

        #endregion

        #region Get

        public void Refresh()
        {
            ManagementScope _scope = new ManagementScope(string.Format(PathExp, this.ComputerName));
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

        }

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

    }
}
