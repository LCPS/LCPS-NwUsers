using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Anvil.v2015.v001.Domain.Html;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;


namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models
{
    public class OuTreeModel : IAnvilFormHandler
    {
        private LcpsAdsOuTree _ouTree;

        public string ModalTitle { get; set; }

        public string FormArea { get; set; }

        public string FormController { get; set; }

        public string FormAction { get; set; }

        public string SubmitText { get; set; }

        public string LinkText { get; set; }

        public string OnErrorActionName { get; set; }

        public LcpsAdsOuTree OuTree
        {
            get
            {
                if (_ouTree == null)
                    _ouTree = new LcpsAdsOuTree();

                return _ouTree;
            }
        }

    }
}