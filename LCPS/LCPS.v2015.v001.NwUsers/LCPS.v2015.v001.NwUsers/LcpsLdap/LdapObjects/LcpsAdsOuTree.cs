using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using Anvil.v2015.v001.Domain.Html;
using Anvil.v2015.v001.Domain.Entities;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    public class LcpsAdsOuTree
    {
        #region Fields

        private LcpsAdsContainer _domain;
        private ApplicationBase _defaultApp;
        private LcpsDbContext _dbContext;
        private AnvilTreeNode _rootNode;

        #endregion


        #region Properties

        public LcpsAdsContainer Domain
        {
            get
            {
                if (_domain == null)
                    _domain = new LcpsAdsContainer(DefaultApp.LdapDomainEntry);

                return _domain;
            }
        }

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();

                return _dbContext;
            }
        }

        public ApplicationBase DefaultApp
        {
            get
            {
                if (_defaultApp == null)
                    _defaultApp = LcpsDbContext.DefaultApp;

                return _defaultApp;
            }
        }

        public AnvilTreeNode RootNode
        {
            get
            {

                if (_rootNode == null)
                    Load();

                return _rootNode;
            }
        }

        public bool Groups { get; set; }

        #endregion

        #region Load

        public void Load()
        {
            _rootNode = new AnvilTreeNode(Domain.Name, Domain.ObjectGuid.ToString(), "domain");
            LoadChildren(ref _rootNode, _domain);
        }

        private void LoadChildren(ref AnvilTreeNode n, LcpsAdsContainer parent)
        {
            LcpsAdsObjectTypes t = LcpsAdsObjectTypes.OrganizationalUnit;

            if (Groups)
                t = t | LcpsAdsObjectTypes.Group;

            List<LcpsAdsContainer> cc = parent.GetContainers(parent, t, false);
            foreach (LcpsAdsContainer c in cc)
            {
                AnvilTreeNode child = new AnvilTreeNode()
                {
                    Text = c.Name,
                    Value = c.ObjectGuid.ToString(),
                    LinkClass = c.ObjectCategory.ToString()
                };

                if(String.IsNullOrEmpty(c.Name))
                    child.Text = c.AdsPath;

                switch (c.ObjectType)
                {
                    case LcpsAdsObjectTypes.Unknown:
                        child.InitGlyph = "fa fa-question";
                        child.ItemGlyphCss = "fa-question";
                        child.SelectedItemGlyphCss = "fa-question";
                        break;
                    case LcpsAdsObjectTypes.Container:
                        child.InitGlyph = "fa fa-folder-o";
                        child.ItemGlyphCss = "fa-folder-o";
                        child.SelectedItemGlyphCss = "fa-folder-o";
                        break;
                    case LcpsAdsObjectTypes.Domain:
                        child.InitGlyph = "fa fa-globe";
                        child.ItemGlyphCss = "fa-globe";
                        child.SelectedItemGlyphCss = "fa-globe";
                        break;
                    case LcpsAdsObjectTypes.OrganizationalUnit:
                        child.InitGlyph = "fa fa-folder-o";
                        child.ItemGlyphCss = "fa-folder-o";
                        child.SelectedItemGlyphCss = "fa-folder-o";
                        break;
                    case LcpsAdsObjectTypes.Group:
                        child.InitGlyph = "fa fa-group";
                        child.ItemGlyphCss = "fa-group";
                        child.SelectedItemGlyphCss = "fa-group";
                        break;
                }

                


                if (c.ObjectType == LcpsAdsObjectTypes.OrganizationalUnit | c.ObjectType == LcpsAdsObjectTypes.Container)
                    LoadChildren(ref child, c);

                n.Children.Add(child);
            }
        }


        #endregion

    }
}
