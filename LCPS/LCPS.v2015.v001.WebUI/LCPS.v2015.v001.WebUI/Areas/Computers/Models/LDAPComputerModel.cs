
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;

using LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models;


namespace LCPS.v2015.v001.WebUI.Areas.Computers.Models
{
    public class LDAPComputerModel
    {

        private LcpsAdsComputer[] _computers;

        public OuTreeModel OUTreeModel { get; set; }

        public LcpsAdsOu OU { get; set; }

        public LcpsAdsComputer[] Computers
        {
            get
            {
                if (_computers == null)
                    _computers = this.OU.GetComputers(false);

                return _computers;
            }
        }

        public static IEnumerable<SelectListItem> GetBuildings()
        {
            LcpsDbContext db = new LcpsDbContext();
            return db.Buildings.OrderBy(x => x.Name).ToSelectListItems(x => x.Name, x => x.BuildingKey.ToString());
        }
    }
}