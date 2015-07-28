using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using System.Web.Mvc.Html;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Models
{
    public class HRRoomModel : HRRoom
    {
        public static IEnumerable<SelectListItem> GetBuildings()
        {
            LcpsDbContext db = new LcpsDbContext();
            return db.Buildings.ToSelectListItems<HRBuilding>(x => x.Name, x => x.BuildingKey.ToString());
        }

        public static string GetBuildingName(Guid buildingKey)
        {
            LcpsDbContext db = new LcpsDbContext();
            return db.Buildings.First(x => x.BuildingKey.Equals(buildingKey)).Name;
        }
    }
}