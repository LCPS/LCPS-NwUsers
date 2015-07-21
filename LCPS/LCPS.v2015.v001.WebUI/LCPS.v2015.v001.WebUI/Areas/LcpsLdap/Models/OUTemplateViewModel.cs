using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.LcpsLdap;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates;
//using LCPS.v2015.v001.WebUI.Areas.Filters.Models;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models
{
    public class OUTemplateViewModel
    {
        private LcpsDbContext _dbContext;
        private List<OUTemplate> _ouTemplates;
        private LcpsAdsOuTree _ouTree;

        public OUTemplateViewModel()
        { }

        public OUTemplateViewModel(LcpsDbContext db)
        {
            _dbContext = db;
        }

        public OUTemplate OUTemplate { get; set; }

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();

                return _dbContext;
            }
        }

        public List<OUTemplate> Templates
        {
            get
            {
                if (_ouTemplates == null)
                    _ouTemplates = GetTemplates();
                return _ouTemplates;
            }
        }

        public LcpsAdsOuTree OuTree
        {
            get 
            {
                if (_ouTree == null)
                    _ouTree = new LcpsAdsOuTree();

                return _ouTree;
            }
        }


        private List<OUTemplate> GetTemplates()
        {
            try
            {
                return DbContext.OUTemplates.OrderBy(x => x.TemplateName).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Coudl not get templates from database", ex);
            }
        }
    }
}