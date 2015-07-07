using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;
using LCPS.v2015.v001.WebUI.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.HRImporting.Models
{
    public class HREmployeeTypeImportModel
    {

        LcpsDbContext db = new LcpsDbContext();        

        public HREmployeeTypeImportModel(Guid id)
        {
            try
            {
                ImportSession fromDb = db.ImportSessions.First(x => x.SessionId.Equals(id));

                HREmployeeTypeImportSession es = new HREmployeeTypeImportSession(fromDb);

                es.Import();
            }
            catch(Exception ex)
            {
                throw new Exception("Import files", ex);
            }
        }
    }
}