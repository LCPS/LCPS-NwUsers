using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Controllers
{
    public class LdapTemplateController : Controller
    {

        #region Fields

        private LcpsDbContext _dbContext;

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

        #endregion

        //
        // GET: /LcpsLdap/LdapTemplate/
        public ActionResult Filters()
        {
            return View();
        }
    
        //
        // GET: /LcpsLdap/LdapTemplate/
        public ActionResult OUTemplates()
        {
            OUTemplateViewModel m = new OUTemplateViewModel(DbContext);


            return View(m);
        }

        
    }
}