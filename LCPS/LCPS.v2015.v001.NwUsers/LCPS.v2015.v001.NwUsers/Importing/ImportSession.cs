using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.IO;

namespace LCPS.v2015.v001.NwUsers.Importing
{
    public enum ImportSessionMode
    {
        Preview,
        Import
    }

    [Table("ImportSession", Schema = "Importing")]
    [Serializable]
    public class ImportSession : IImportSession
    {

        LCPS.v2015.v001.NwUsers.Infrastructure.LcpsDbContext db = new Infrastructure.LcpsDbContext();

        public ImportSession()
        { }

        public ImportSession(string action, string controller,
                string area, System.Type itemType,
                bool addIfNotExist, bool updateIfExists, string viewTitle, string viewLayoutPath)
        {
            this.SessionDate = DateTime.Now;
            this.SessionId = Guid.NewGuid();
            this.Area = area;
            this.Author = HttpContext.Current.User.Identity.Name;
            this.Controller = controller;
            this.Action = action;
            this.AddIfNotExist = addIfNotExist;
            this.UpdateIfExists = updateIfExists;
            this.FullAssemblyTypeName = itemType.AssemblyQualifiedName;
            this.ViewLayoutPath = viewLayoutPath;
            this.ViewTitle = viewTitle;
        }

        [Key]
        public Guid SessionId { get; set; }

        [NotMapped]
        public ImportSessionMode Mode { get; set; } 

        [NotMapped]
        public HttpPostedFileBase ImportFile { get; set; }

        public DateTime SessionDate { get; set; }

        public string Author { get; set; }

        public string TypeName
        {
            get { return ItemType.Name; }
        }

        public string FullAssemblyTypeName  { get; set; }

        public Type ItemType 
        {
            get { return System.Type.GetType(FullAssemblyTypeName); }
        }

        public ImportSession ToImportSession()
        {
            throw new NotImplementedException();
        }

        public bool AddIfNotExist  { get; set; }

        public bool UpdateIfExists  { get; set; }

        public System.Collections.Generic.List<IImportStatus> Candidates
        {
            get { throw new NotImplementedException(); }
        }

        public string Delimiter  { get; set; }

        public void ParseItems(StreamReader reader)
        {
            throw new NotImplementedException();
        }

        public string[] FieldNames  { get; set; }

        public byte[] ImportFileContents  { get; set; }

        public string ViewTitle  { get; set; }

        public string Area  { get; set; }

        public string Controller { get; set; }

        public string ViewLayoutPath  { get; set; }

        public void Record()
        {
            try
            {
                db.ImportSessions.Add(this);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create import session", ex);
            }
        }

        public void Import()
        {
            throw new NotImplementedException();
        }

        public void DetectConflicts()
        {
            throw new NotImplementedException();
        }


        public string Action  { get; set; }

        public virtual ICollection<ImportItem> Items { get; set; }


        
        public static int Count(IEnumerable<ImportItem> items, ImportResultStatus status)
        {
            return items.Where(x => x.ImportStatus == status).Count();
        }

        public static int Count(IEnumerable<ImportItem> items, ImportEntityStatus status)
        {
            return items.Where(x => x.EntityStatus == status).Count();
        }

        public static IEnumerable<ImportItem> Filter(IEnumerable<ImportItem> items, ImportResultStatus status)
        {
            return items.Where(x => x.ImportStatus == status).OrderBy(x => x.EntryDate);
        }

        public static IEnumerable<ImportItem> Filter(IEnumerable<ImportItem> items, ImportEntityStatus status)
        {
            return items.Where(x => x.EntityStatus == status).OrderBy(x => x.EntryDate);
        }

    }
}
