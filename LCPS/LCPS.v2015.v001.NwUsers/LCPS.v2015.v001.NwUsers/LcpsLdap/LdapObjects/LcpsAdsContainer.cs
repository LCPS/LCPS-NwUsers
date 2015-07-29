using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    public class LcpsAdsContainer : LcpsAdsObject
    {
        #region Fields

        string _distinguishedName;
        string _description;
        string _name;

        #endregion


        #region Constructors

        public LcpsAdsContainer()
            :base()
        {

        }

        public LcpsAdsContainer(DirectoryEntry e)
            : base(e)
        { }

        #endregion

        #region Properties

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

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        public override void FillFieldMaps()
        {
            AddFieldMap("DistinguishedName", "distinguishedName");
            AddFieldMap("Description", "description");
            if (DirectoryEntry.SchemaClassName == "organizationalUnit")
                AddFieldMap("Name", "ou");
            if (DirectoryEntry.SchemaClassName == "domainDNS")
                AddFieldMap("Name", "dc");
            if (DirectoryEntry.SchemaClassName == "group")
                AddFieldMap("Name", "cn");

        }

        #region Get

        public List<LcpsAdsContainer> GetContainers(LcpsAdsObject root, LcpsAdsObjectTypes containerTypes, bool recursive)
        {
            try
            {
                string catFilter = "(objectClass={0})";
                List<string> _cats = new List<string>();

                if (containerTypes.HasFlag(LcpsAdsObjectTypes.Domain))
                    _cats.Add(string.Format(catFilter, "domain"));
                
                if (containerTypes.HasFlag(LcpsAdsObjectTypes.OrganizationalUnit))
                    _cats.Add(string.Format(catFilter, "organizationalUnit"));

                if (containerTypes.HasFlag(LcpsAdsObjectTypes.Group))
                    _cats.Add(string.Format(catFilter, "group"));

                if (containerTypes.HasFlag(LcpsAdsObjectTypes.User))
                    throw new Exception("User is an unsupported option in this context");

                string f = "(|" + string.Join("", _cats.ToArray()) + ")";

                DirectorySearcher s = new DirectorySearcher(root.DirectoryEntry, f);
                if (recursive)
                    s.SearchScope = SearchScope.Subtree;
                else
                    s.SearchScope = SearchScope.OneLevel;

                List<LcpsAdsContainer> containers = new List<LcpsAdsContainer>();

                foreach(SearchResult r in s.FindAll())
                {
                    LcpsAdsContainer c = new LcpsAdsContainer(r.GetDirectoryEntry());
                    containers.Add(c);
                }

                return containers.OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while attempting to get a list of containers", ex);
            }
        }
        #endregion
    }
}
