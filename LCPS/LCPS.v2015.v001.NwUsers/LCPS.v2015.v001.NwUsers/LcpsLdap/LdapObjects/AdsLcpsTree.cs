using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    public class AdsLcpsTree
    {
        #region Fields

        public List<LcpsAdsTreeNode> _children = new List<LcpsAdsTreeNode>();

        #endregion

        #region Constructors

        public AdsLcpsTree(LcpsAdsObjectTypes containerTypes, LcpsAdsContainer root)
        {
            ContainerTypes = containerTypes;
            Root = root;
        }

        #endregion

        #region Properties

        public LcpsAdsObjectTypes ContainerTypes { get; set; }

        public LcpsAdsContainer Root { get; set; }

        public LcpsAdsTreeNode RootNode { get; set; }

        public List<LcpsAdsTreeNode> Children
        {
            get { return _children; }
        }

        #endregion

        #region Load

        public void Load()
        {
            LcpsAdsTreeNode n = new LcpsAdsTreeNode(Root.ObjectGuid, Root.Name, Root.ObjectType);
            Load(ContainerTypes, Root, ref n);
            RootNode = n;

        }

        private void Load(LcpsAdsObjectTypes containerTypes, LcpsAdsContainer root, ref LcpsAdsTreeNode parent)
        {
            List<LcpsAdsContainer> items = root.GetContainers(root, containerTypes, false);

            foreach(LcpsAdsContainer c in items)
            {
                LcpsAdsTreeNode n = new LcpsAdsTreeNode(c.ObjectGuid, c.Name, c.ObjectType);
                parent.Children.Add(n);
                Load(containerTypes, c, ref n);
            }
        }

        #endregion



    }
}
