using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.WebUI.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.Import.Models
{
    public class ImportPreviewModel
    {
        LcpsDbContext db = new LcpsDbContext();

        List<ImportItem> _items;
        ImportSession _session;

        public ImportPreviewModel(Guid id)
        {
            _session = db.ImportSessions.First(x => x.SessionId.Equals(id));
            _items = db.ImportItems.Where(x => x.SessionId.Equals(id)).ToList();
        }

        public ImportSession Session
        {
            get { return _session; }
        }


        public List<ImportItem> Items
        {
            get { return _items; }
        }

    }
}