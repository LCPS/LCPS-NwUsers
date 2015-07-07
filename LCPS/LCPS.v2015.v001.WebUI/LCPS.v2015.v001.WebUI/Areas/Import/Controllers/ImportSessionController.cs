using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.WebUI.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.Import.Controllers
{
    public class ImportSessionController : Controller
    {

        LcpsDbContext db = new LcpsDbContext();

        //
        // GET: /Import/ImportSession/
        public ActionResult Delete(Guid id, string u)
        {
            ImportSession s = db.ImportSessions.First(x => x.SessionId.Equals(id));
            db.ImportSessions.Remove(s);
            db.SaveChanges();

            return Redirect(u);
        }

        public ActionResult DeleteAll(string t, string u)
        {
            ImportSession[] ss = db.ImportSessions.Where(x => x.FullAssemblyTypeName == t).ToArray();
            db.ImportSessions.RemoveRange(ss);
            db.SaveChanges();

            return Redirect(u);
        }
	}
}