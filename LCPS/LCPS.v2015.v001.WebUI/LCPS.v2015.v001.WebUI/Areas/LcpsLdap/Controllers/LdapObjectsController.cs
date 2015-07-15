using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Controllers
{
    public class LdapObjectsController : Controller
    {

        public OUTreeModel TreeModel
        {
            get
            {
                if (Session["TreeModel"] == null)
                    return null;
                else
                    return (OUTreeModel)Session["TreeModel"];
            }
            set
            {
                Session["TreeModel"] = value;
            }
        }
        

        //
        // GET: /LcpsLdap/LdapObjects/
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Ous()
        {
            OUTreeModel t = new OUTreeModel();
            this.TreeModel = t;
            return View(t);
        }

        public ActionResult OuSelect(Guid? ouId)
        {
            OUTreeModel m = this.TreeModel;
            if (ouId != null)
            {
                
                LcpsAdsOu ou = new LcpsAdsOu(ouId.Value);
                m.CurrentOu = ou;
                this.TreeModel = m;
            }

            return View("OUs", m);
        }
	}
}