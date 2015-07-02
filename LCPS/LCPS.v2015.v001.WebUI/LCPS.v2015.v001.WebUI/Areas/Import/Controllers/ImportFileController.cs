using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.WebUI.Areas.Import.Models;
using LCPS.v2015.v001.WebUI.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.Import.Controllers
{
    public class ImportFileController : Controller
    {
        // GET: Import/ImportFile
        public ActionResult Index(ImportSessionModel s)
        {
            return View(s);
        }


        [HttpPost]
        public ActionResult ReadFile(ImportSessionModel m)
        {
            byte[] b = null;
            using (var memoryStream = new MemoryStream())
            {
                m.ImportFilePost.InputStream.CopyTo(memoryStream);
                b = memoryStream.ToArray();
            }


            ImportSession sess = null;
            LcpsDbContext db = new LcpsDbContext();
            int count = db.ImportSessions.Where(x => x.SessionId.Equals(m.SessionId)).Count();
            ImportFileModel f = null;

            if (count == 0)
            {
                sess = new ImportSession()
                {
                    SessionId = m.SessionId,
                    SessionDate = m.SessionDate,
                    Author = m.Author,
                    FullAssemblyTypeName = m.FullAssemblyTypeName,
                    TypeName = m.TypeName,
                    ImportFileContents = b,
                    Area = m.Area,
                    Controller = m.Controller,
                    Action = m.Action,
                    DetailMode = ImportFile.ListQualifier.Read,
                    ViewLayoutPath = m.ViewLayoutPath
                };


                db.ImportSessions.Add(sess);
                db.SaveChanges();

                f = new ImportFileModel(sess);
                f.ListFor = ImportFile.ListQualifier.Read;
                f.ReadFile();
                f = new ImportFileModel(sess);
            }
            else
            {
                sess = db.ImportSessions.First(x => x.SessionId.Equals(m.SessionId));
                f = new ImportFileModel(sess);
                f.ListFor = ImportFile.ListQualifier.Read;
                f.Items.Clear();
                f.Items.AddRange(db.ImportItems.Where(x => x.SessionId.Equals(m.SessionId)).OrderBy(x => x.EntryDate));
            }



            return View("Results", f);
        }

        public ActionResult ImportResults(Guid id)
        {
            LcpsDbContext db = new LcpsDbContext();
            ImportSession s = db.ImportSessions.First(x => x.SessionId.Equals(id));
            ImportFileModel f = new ImportFileModel(s);
            f.Session = s;
            f.ListFor = ImportFile.ListQualifier.Import;
            f.Session.DetailMode = ImportFile.ListQualifier.Import;
            f.Items.AddRange(db.ImportItems.Where(x => x.SessionId.Equals(id)));
            return View("Results", f);

            
        }
    }
}