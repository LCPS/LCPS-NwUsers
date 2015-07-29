using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using Anvil.v2015.v001.Domain.Entities;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    public abstract class LcpsAdsObject
    {
        #region Fields

        private Guid _objectGuid;
        private string _adsPath;
        private DirectoryEntry _directoryEntry;
        private Dictionary<string, string> _lcpsToAds;
        private Dictionary<string, string> _adsToLcps;
        private ApplicationBase _defaultApp;

        #endregion

        #region Constructors

        public LcpsAdsObject()
        {
            FillFieldMaps();
        }

        public LcpsAdsObject(string adsPath)
        {
            _directoryEntry = new DirectoryEntry(adsPath, Credential.UserName, Credential.Password);
            _adsPath = adsPath;
            _objectGuid = GetObjectGuid(_directoryEntry);
            this.ObjectCategory = GetObjectCategory(_directoryEntry);
            FillFieldMaps();
            FillFromDirectoryEntry();
        }

        public LcpsAdsObject(Guid objectGuid)
        {
            _objectGuid = objectGuid;
            string p = String.Format("LDAP://{0}/<GUID={1}>", Credential.Domain, objectGuid.ToString());
            _directoryEntry = new DirectoryEntry(p, Credential.UserName, Credential.Password);
            _adsPath = _directoryEntry.Path;
            this.ObjectCategory = GetObjectCategory(_directoryEntry);
            FillFieldMaps();
            FillFromDirectoryEntry();
        }

        public LcpsAdsObject(DirectoryEntry de)
        {
            _directoryEntry = de;
            _adsPath = de.Path;
            _objectGuid = GetObjectGuid(de);
            this.ObjectCategory = GetObjectCategory(_directoryEntry);
            FillFieldMaps();
            FillFromDirectoryEntry();
        }

        #endregion

        #region Properties

        public ApplicationBase DefaultApp
        {
            get
            {
                if (_defaultApp == null)
                    _defaultApp = LcpsDbContext.DefaultApp;

                return _defaultApp;
            }
        }

        public System.Net.NetworkCredential  Credential
        {
            get { return DefaultApp.GetLdapCredential(); }
        }

        public Guid ObjectGuid
        {
            get { return _objectGuid; }
            set { _objectGuid = value; }
        }

        public string AdsPath
        {
            get { return _adsPath; }
            set { _adsPath = value; }
        }

        public string ObjectCategory { get; set; }

        public LcpsAdsObjectTypes ObjectType
        {
            get
            {
                if (this.ObjectCategory.ToLower().Contains("domain-dns"))
                    return LcpsAdsObjectTypes.Domain;

                if (this.ObjectCategory.ToLower().Contains("organizational-unit"))
                    return LcpsAdsObjectTypes.OrganizationalUnit;

                if (this.ObjectCategory.ToLower().Contains("group"))
                    return LcpsAdsObjectTypes.Group;

                throw new Exception(String.Format("{0} is not valid in this context", this.ObjectCategory));
            }
        }

        public DirectoryEntry DirectoryEntry
        {
            get { return _directoryEntry; }
        }

        public Dictionary<string, string> LcpsToAdsFieldMaps
        {
            get { return _lcpsToAds; }
        }

        public Dictionary<string, string> AdsToLcpsFieldMaps
        {
            get { return _adsToLcps; }
        }

        #endregion

        #region Field Map

        public abstract void FillFieldMaps();

        public void AddFieldMap(string lcpsProperty, string adsProperty)
        {
            if (_adsToLcps == null)
                _adsToLcps = new Dictionary<string, string>();

            if (_lcpsToAds == null)
                _lcpsToAds = new Dictionary<string, string>();

            _adsToLcps.Add(adsProperty, lcpsProperty);
            _lcpsToAds.Add(lcpsProperty, adsProperty);
        }

        #endregion

        #region DirectoryEntry

        

        public void SetDirectoryEntry(DirectoryEntry de)
        {
            _directoryEntry = de;
        }



        public static Guid GetObjectGuid(DirectoryEntry d)
        {
            object gid = null;
            try
            { 
                gid = d.InvokeGet("objectGuid"); 
            }
            catch (Exception ex) 
            { 
                throw new Exception(string.Format("Could not get GUID on entry: {0}", d.Path), ex); 
            }

            byte[] b = gid as byte[];
            Guid g = new Guid(b);
            return g;
        }

        public static string GetObjectCategory(DirectoryEntry d)
        {
            object c = d.InvokeGet("objectCategory");
            return c.ToString();
        }

        public virtual void FillFromDirectoryEntry()
        {
            if (_adsToLcps == null)
                throw new Exception("The LCPS to ADS field map is null for this object type");

            foreach(string k in _adsToLcps.Keys)
            {
                PropertyInfo p = this.GetType().GetProperty(_adsToLcps[k]);
                if (p == null)
                    throw new Exception(String.Format("Property {0} was not found in type {1}", _adsToLcps[k], this.GetType().Name));

                object v = _directoryEntry.InvokeGet(k);
                p.SetValue(this, v, null);
            }
        }

        public virtual void FillDirectoryEntry(bool commitChanges)
        {
            if (_lcpsToAds == null)
                throw new Exception("The LCPS to ADS field map is null for this object type");



            foreach (string k in _lcpsToAds.Keys)
            {
                if (_lcpsToAds[k].ToLower() != "cn")
                {
                    try
                    {
                        PropertyInfo p = this.GetType().GetProperty(k);
                        object v = p.GetValue(this, null);
                        UpdateProperty(_lcpsToAds[k], _directoryEntry, v);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("An error occurred while attempting to set property {0}", k), ex);
                    }
                }
            }
            
            if (commitChanges)
                _directoryEntry.CommitChanges();
        }

        public static void UpdateProperty(string propertyName, DirectoryEntry d, object value)
        {
            if (value == null)
                d.Properties[propertyName].Clear();

            else
            {
                if (value.GetType() == typeof(string))
                {
                    if (value.ToString() == "")
                        value = null;
                }

                if (value != null)
                    d.InvokeSet(propertyName, value);
                else
                    d.Properties[propertyName].Clear();
            }
        }

        

        #endregion

        #region Find

        public static DirectoryEntry Find(string filter, DirectoryEntry parent)
        {
            try
            {
                using (DirectorySearcher s = new DirectorySearcher(parent, filter))
                {
                    SearchResult r = s.FindOne();
                    return r.GetDirectoryEntry();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching for directory entry", ex);
            }
        }

        public static ICollection FindAll(string filter, DirectoryEntry parent, bool recursive, System.Type objectType)
        {
            try
            {
                using (DirectorySearcher s = new DirectorySearcher(parent, filter))
                {
                    if (recursive)
                        s.SearchScope = SearchScope.Subtree;
                    else
                        s.SearchScope = SearchScope.OneLevel;

                    ArrayList l = new ArrayList();

                    foreach (SearchResult r in s.FindAll())
                    {
                        DirectoryEntry d = r.GetDirectoryEntry();
                        object i = Activator.CreateInstance(objectType, new object[] { d });
                        l.Add(i);
                    }

                    return l.ToArray(objectType);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching for directory entries", ex);
            }

        }
        #endregion

    }
}
