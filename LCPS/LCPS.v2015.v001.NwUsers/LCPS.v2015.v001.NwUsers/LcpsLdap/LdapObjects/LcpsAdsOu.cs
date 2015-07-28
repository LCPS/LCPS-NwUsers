using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    public class LcpsAdsOu : LcpsAdsObject
    {

        #region Fields

        private string _name;
        private string _description;
        private string _distinguishedName;

        #endregion

        #region Constructors

        public LcpsAdsOu(Guid id)
            :base(id)
        {
            
        }

        public LcpsAdsOu(DirectoryEntry e)
            :base(e)
        {

        }

        public override void FillFieldMaps()
        {
            AddFieldMap("Name", "ou");
            AddFieldMap("Description", "description");
            AddFieldMap("DistinguishedName", "distinguishedName");
        }


        #endregion

        #region Properties

        public string Name
        {
            get
            {
                return _name;
            }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string DistinguishedName
        {
            get { return _distinguishedName; }
            set { _distinguishedName = value; }
        }

        #endregion

        #region Children

        public LcpsAdsGroup[] GetGroups(bool recursive)
        {
            ICollection x = FindAll("(&(objectCategory=group))", DirectoryEntry, recursive, typeof(LcpsAdsGroup));
            LcpsAdsGroup[] i = x.Cast<LcpsAdsGroup>().ToArray();
            return i;
            
        }

        public LcpsAdsUser[] GetUsers(bool recursive)
        {
            ICollection x = FindAll("(&(objectCategory=user))", DirectoryEntry, recursive, typeof(LcpsAdsUser));
            LcpsAdsUser[] i = x.Cast<LcpsAdsUser>().ToArray();
            return i;
        }

        public LcpsAdsComputer[] GetComputers(bool recursive)
        {
            ICollection x = FindAll("(&(objectCategory=computer))", DirectoryEntry, recursive, typeof(LcpsAdsComputer));
            List<LcpsAdsComputer> l = x.Cast<LcpsAdsComputer>().ToList();
            l = l.OrderBy(c => c.ComputerName).ToList();
            LcpsAdsComputer[] i = l.ToArray();
            return i;
        }

        #endregion
    }
}
