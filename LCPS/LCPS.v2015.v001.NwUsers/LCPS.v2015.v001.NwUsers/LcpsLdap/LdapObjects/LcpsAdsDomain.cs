using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using Anvil.v2015.v001.Domain.Entities;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    public class LcpsAdsDomain : LcpsAdsObject
    {
        #region Fields

        private string _name;

        #endregion

        #region Constructors

        public LcpsAdsDomain()
        { }

        public LcpsAdsDomain(string adsPath)
            :base(adsPath)
        {

        }

        public LcpsAdsDomain(Guid objectGuid, System.Net.NetworkCredential credential)
            :base(objectGuid)
        {
        }

        public LcpsAdsDomain(DirectoryEntry d)
            : base(d)
        {
        }

        public override void FillFieldMaps()
        {
            AddFieldMap("Name", "dc");
        }

        #endregion

        #region Properties

        public static LcpsAdsDomain Default
        {
            get
            {
                LcpsAdsDomain dom = null;
                LcpsDbContext _dbContext = new LcpsDbContext();
                ApplicationBase app = null;

                try
                {
                    app = LcpsDbContext.DefaultApp;
                }
                catch(Exception ex)
                {
                    throw new Exception("Could get domain connection details from server", ex);
                }

                try
                {
                    dom = new LcpsAdsDomain("LDAP://" + app.LDAPDomainFQN);
                    return dom;
                }
                catch(Exception ex)
                {
                    throw new Exception("Could not communicate with domain controller", ex);
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Children

        public List<LcpsAdsOu> GetOus(bool recursive)
        {
            ICollection x = FindAll("(&(objectCategory=organizationalUnit))", DirectoryEntry, recursive, typeof(LcpsAdsOu));
            List<LcpsAdsOu>  i =x.Cast<LcpsAdsOu>().ToList();
            return i;
        }

        public List<LcpsAdsGroup> GetGroups(bool recursive)
        {
            ICollection x = FindAll("(&(objectCategory=group))", DirectoryEntry, recursive, typeof(LcpsAdsGroup));
            List<LcpsAdsGroup> i = x.Cast<LcpsAdsGroup>().ToList();
            return i;

        }

        public List<LcpsAdsUser> GetUsers(bool recursive)
        {
            ICollection x = FindAll("(&(objectCategory=user))", DirectoryEntry, recursive, typeof(LcpsAdsUser));
            List<LcpsAdsUser> i = x.Cast<LcpsAdsUser>().ToList();
            return i;
        }


        #endregion
    }
}
