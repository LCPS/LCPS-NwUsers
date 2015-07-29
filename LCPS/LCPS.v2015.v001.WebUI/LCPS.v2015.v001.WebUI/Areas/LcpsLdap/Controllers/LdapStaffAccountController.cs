using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapAccounts;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using PagedList;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Controllers
{
    public class LdapStaffAccountController : Controller
    {
        //
        // GET: /LcpsLdap/LdapStaffAccount/
        public ActionResult Index(int? page, int? pageSize)
        {
            page = (page == null) ? 1 : page;
            
            pageSize = (pageSize == null) ? 12 : page;

            List<StaffLdapAccount> accounts = (new LdapAccountContext())
                .StaffLdapAccounts
                .OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial).ToList();

            @ViewBag.Total = accounts.Count().ToString();

            PagedList<StaffLdapAccount> pl = new PagedList<StaffLdapAccount>(accounts, page.Value, pageSize.Value);
            

            return View(pl);
        }
	}
}