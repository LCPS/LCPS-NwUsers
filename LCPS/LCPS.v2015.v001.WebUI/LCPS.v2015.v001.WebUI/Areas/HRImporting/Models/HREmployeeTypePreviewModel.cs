using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;
using LCPS.v2015.v001.NwUsers.Importing;

namespace LCPS.v2015.v001.WebUI.Areas.HRImporting.Models
{
    public class HREmployeeTypePreviewModel
    {

        LcpsDbContext db = new LcpsDbContext();
        

        public HREmployeeTypePreviewModel(Guid sessionId, Stream inputStream)
        {
            ImportSession s = db.ImportSessions.First(x => x.SessionId.Equals(sessionId));
            s.Mode = ImportSessionMode.Preview;
            
            Session = s;
            HREmployeeTypeImportSession eSess = new HREmployeeTypeImportSession(s);

            if (db.ImportItems.Where(x => x.SessionId.Equals(sessionId)).Count() == 0)
            {
                using (StreamReader r = new StreamReader(inputStream))
                {
                    try
                    {
                        eSess.ParseItems(r);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Could not read source file", ex);
                    }
                }
            }

            ImportItem[] items = db.ImportItems.Where(x => x.SessionId.Equals(s.SessionId)).ToArray();
            Items = items.Select(x => x.Deserialize(s)).Cast<HREmployeeTypeCandidate>().OrderBy(x => x.EmployeeTypeId).ToList();
        }


        public ImportSession Session { get; set;}

        public List<HREmployeeTypeCandidate> Items { get; set; }

    }
}