using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    public class LcpsAdsTreeNode
    {
        private List<LcpsAdsTreeNode> _children = new List<LcpsAdsTreeNode>();

        public LcpsAdsTreeNode() { }

        public LcpsAdsTreeNode(Guid objectGuid, string name, LcpsAdsObjectTypes objectType)
        {
            this.ObjectGuid = objectGuid;
            this.Name = name;
            this.ObjectType = objectType;
        }

        public Guid ObjectGuid { get; set; }

        public string Name { get; set; }

        public LcpsAdsObjectTypes ObjectType { get; set; }

        public string CssClass
        {
            get
            {
                string c = ObjectType.ToString().ToLower();
                if (_children.Count() > 0)
                    c += " parent";

                return c;
            }
        }

        public List<LcpsAdsTreeNode> Children 
        {
            get { return _children; }
        }
    }
}
