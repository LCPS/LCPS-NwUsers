using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;

using LCPS.v2015.v001.WebUI.Areas.Computers.Models;
using LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models;

namespace LCPS.v2015.v001.WebUI.Areas.Computers.Controllers
{
    public class LDAPComputerController : Controller
    {
        //
        // GET: /Computers/LDAPComputer/
        public ActionResult Index(Guid? ou)
        {
            LDAPComputerModel m = new LDAPComputerModel();
            m.OUTreeModel = new OuTreeModel()
            {
                FormAction = "SelectOU",
                FormArea = "Computers",
                FormController = "LDAPComputer",
                ModalTitle = "Select Computer OU"

            };

            if (ou != null)
                m.OU = new LcpsAdsOu(ou.Value);
            
            return View(m);
        }



    }
}