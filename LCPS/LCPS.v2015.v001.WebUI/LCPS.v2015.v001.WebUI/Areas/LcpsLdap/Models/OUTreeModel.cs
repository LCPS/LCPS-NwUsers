using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using Anvil.v2015.v001.Domain.Entities;
using System.Web;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models
{
    public class OUTreeModel
    {
        #region Fields

        private LcpsDbContext _dbContext;
        private ApplicationBase _defaultApp;
        private LcpsAdsDomain _domain;

        #endregion

        #region Properties

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
            get {
                if (_defaultApp == null)
                    _defaultApp = LcpsDbContext.DefaultApp;

                return _defaultApp;
            }
        }

        public LcpsAdsDomain Domain
        {
            get
            {
                if (_domain == null)
                    _domain = new LcpsAdsDomain(DefaultApp.LdapDomainEntry);

                return _domain;
            }
        }

        public LcpsAdsOu CurrentOu { get; set; }
       

        #endregion


        public AdsLcpsTree Tree
        {
            get
            {
                LcpsAdsObjectTypes ot = LcpsAdsObjectTypes.OrganizationalUnit;
                

                AdsLcpsTree t = new AdsLcpsTree(ot, new LcpsAdsContainer(Domain.DirectoryEntry));

                t.Load();

                return t;
            }
        }
    }
}