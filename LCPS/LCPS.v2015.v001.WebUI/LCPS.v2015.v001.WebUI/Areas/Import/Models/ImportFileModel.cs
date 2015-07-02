using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.WebUI.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.Import.Models
{
    public class ImportFileModel : ImportFile
    {
        private LcpsDbContext db = new LcpsDbContext();

        public ImportFileModel(ImportSession session)
            :base(session)
        {

        }

        public override void WriteImportItemToDb(ImportItem item)
        {
            db.ImportItems.Add(item);
            db.SaveChanges();
        }

        public override List<ImportItem> Items
        {
            get 
            {
                return db.ImportItems.Where(x => x.SessionId.Equals(Session.SessionId)).ToList();
            }
        }
    }
}