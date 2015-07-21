using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Anvil.v2015.v001.Domain.Exceptions;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates;
using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Controllers
{
    public class LdapOuTemplateController : Controller
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

        public OUTemplateViewModel Model
        {
            get
            {
                if(Session["OUTemplateViewModel"] == null)
                    Session["OUTemplateViewModel"] = new OUTemplateViewModel(DbContext);
                return (OUTemplateViewModel)Session["OUTemplateViewModel"];
            }
            set
            {
                Session["OUTemplateViewModel"] = value;
            }
        }

        #endregion
        //
        // GET: /LcpsLdap/LdapOuTemplate/
        public ActionResult Index()
        {
            return View(Model);
        }

        //
        // GET: /LcpsLdap/LdapOuTemplate/
        public ActionResult Create()
        {
            try 
            {
                OUTemplateViewModel m = Model;
                LcpsAdsContainer c = m.OuTree.Domain;
                return View(Model);
            }
            catch(Exception ex)
            {
                AnvilExceptionModel em = new AnvilExceptionModel(ex, "Error loading create page", "LcpsLdap", "LdapOuTemplate", "Index");
                return View("~/Views/Shared/Error.cshtml", em);
            }
        }

        [HttpPost]
        public ActionResult Create(OUTemplateViewModel model)
        {
            if (model.OUTemplate.OUId.Equals(Guid.Empty))
                ModelState.AddModelError(null, "Please select a valid OU");



            if(ModelState.IsValid)
            {
                try
                {
                    DbContext.OUTemplates.Add(model.OUTemplate);
                    DbContext.SaveChanges();
                    return View("Index", model);
                }
                catch (Exception ex)
                {
                    Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel ec = new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Create OU Template", "LcpsLdap", "LdapOuTemplate", "Create" );
                    return View("~/Views/Shared/Error.cshtml", ec);
                }
            }

            return View(model);
        }

        public ActionResult SelectOu(Guid id)
        {
            OUTemplateViewModel m = Model;

            if (m.OUTemplate == null)
                m.OUTemplate = new NwUsers.LcpsLdap.LdapTemplates.OUTemplate();

            m.OUTemplate.OUId = id;
            Model = m;
            return View("Create", Model);
        }

        public ActionResult SelectTemplate(Guid id)
        {
            OUTemplateViewModel m = Model;

            OUTemplate t = DbContext.OUTemplates.First(x => x.OUId.Equals(id));

            m.OUTemplate = t;

            Model = m;

            return View("Index", Model);

        }
	}
}