using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    public class LcpsAdsGroup : LcpsAdsObject
    {
        #region Fields

        private string _name;
        private string _description;
        private string _distinguishedName;
        private string _displayName;

        #endregion

        #region Constructors

        public LcpsAdsGroup()
            : base()
        {
        }

        public LcpsAdsGroup(string adsPath)
            : base(adsPath)
        {
        }

        public LcpsAdsGroup(Guid objectGuid)
            :base(objectGuid)
        {
        }

        public LcpsAdsGroup(DirectoryEntry e)
            :base(e)
        {
        }

        public override void FillFieldMaps()
        {
            AddFieldMap("Name", "cn");
            AddFieldMap("Description", "description");
            AddFieldMap("DisplayName", "displayName");
            AddFieldMap("DistinguishedName", "distinguishedName");
        }

        #endregion

        #region Properties

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string DistinguishedName
        {
            get { return _distinguishedName; }
            set { _distinguishedName = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        #endregion

        #region Members

        public LcpsAdsUser[] GetUsers()
        {
            List<LcpsAdsUser> l = new List<LcpsAdsUser>();

            LcpsAdsDomain dmn = new LcpsAdsDomain(LcpsDbContext.DefaultApp.LdapDomainEntry);

            string fx = "(&(memberof={0})(objectCategory=user))";
            string f = string.Format(fx, _distinguishedName);
            using (DirectorySearcher s = new DirectorySearcher(dmn.DirectoryEntry, f))
            {
                s.SearchScope = SearchScope.Subtree;
                foreach(SearchResult r in s.FindAll())
                {
                    DirectoryEntry d = r.GetDirectoryEntry();
                    if (d.SchemaClassName == "user")
                        l.Add(new LcpsAdsUser(d));
                }
            }

           
            return l.ToArray();
        }

        public void AddMember(LcpsAdsUser u)
        {
            try
            {
                DirectoryEntry.Properties["member"].Add(u.DistinguishedName);
                DirectoryEntry.CommitChanges();
                DirectoryEntry.RefreshCache();
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Error adding {0} to {1}", u.UserName, Name), ex);
            }
        }

        public void RemoveMember(LcpsAdsUser u)
        {
            try
            {
                DirectoryEntry.Properties["member"].Remove(u.DistinguishedName);
                DirectoryEntry.CommitChanges();
                DirectoryEntry.RefreshCache();
                u.DirectoryEntry.CommitChanges();
                u.DirectoryEntry.RefreshCache();
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Error removing {0} from {1}", u.UserName, Name), ex);
            }
        }

        #endregion
    }
}
