#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

//using System.Xml.Serialization;

using Anvil.v2015.v001.Domain.Exceptions;
using Anvil.v2015.v001.Domain.Entities;
using LCPS.v2015.v001.NwUsers.Infrastructure;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace LCPS.v2015.v001.NwUsers.Importing
{
    public abstract class ImportFileTSV : IImportSession
    {

        List<IImportStatus> _candidates = new List<IImportStatus>();
        LcpsDbContext db = new LcpsDbContext();

        public ImportFileTSV(ImportSession s)
        {
            AnvilEntity e = new AnvilEntity(s);
            e.CopyTo(this);
            Delimiter = '\t'.ToString();
        }

        public ImportFileTSV(System.Type itemType, string viewTitle,
            string area, string controller, string action, string viewLayoutPath,
            bool addIfNotExists, bool updateIfExists)
        {
            SessionDate = DateTime.Now;
            Author = HttpContext.Current.User.Identity.Name;
            ItemType = itemType;
            FullAssemblyTypeName = ItemType.AssemblyQualifiedName;
            ViewTitle = viewTitle;
            Area = area;
            Controller = controller;
            Action = action;
            ViewLayoutPath = viewLayoutPath;
            Delimiter = '\t'.ToString();
            AddIfNotExist = addIfNotExists;
            UpdateIfExists = updateIfExists;
        }

        public Guid SessionId { get; set; }

        public DateTime SessionDate { get; set; }

        public string Author { get; set; }

        public string TypeName { get; set; }

        public string FullAssemblyTypeName { get; set; }

        public Type ItemType { get; set; }

        [DataType(DataType.Text)]
        public string Delimiter { get; set; }

        public string[] FieldNames { get; set; }

        public byte[] ImportFileContents { get; set; }

        public string ViewTitle { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string ViewLayoutPath { get; set; }


        public void ParseItems(StreamReader reader)
        {

            this.Record();

            int index = 0;

            ItemType = System.Type.GetType(FullAssemblyTypeName);

            while (!reader.EndOfStream)
            {
                string l = reader.ReadLine();
                if (l == null)
                    break;

                if (index == 0)
                {
                    try
                    {
                        FieldNames = l.Split(Delimiter.ToCharArray()[0]);
                        index++;
                    }
                    catch (Exception ex)
                    {
                        AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                        ec.Insert(0, "Could not parse the columns from the first line");
                        throw ec.ToException();
                    }
                }
                else
                {
                    if (l.Contains("CONSULT"))
                    {
                        int x = 0;
                        x++;
                    }


                    object sourceItem = null;

                    try
                    {
                        sourceItem = ParseLine(l);
                    }
                    catch (Exception ex)
                    {
                        ImportItem i = new ImportItem()
                        {
                            ImportItemId = Guid.NewGuid(),
                            SessionId = SessionId,
                            Description = l,
                            EntityStatus = ImportEntityStatus.Error,
                            EntryDate = DateTime.Now,
                            LineIndex = index - 1,
                            Comment = string.Join("\n", (new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionCollector(ex)).ToArray())
                        };

                        try
                        {
                            db.ImportItems.Add(i);
                            db.SaveChanges();
                        }
                        catch (Exception ex2)
                        {
                            AnvilExceptionCollector iEx = new AnvilExceptionCollector(ex2);
                            throw iEx.ToException();
                        }
                    }

                    index++;

                    if (sourceItem != null)
                    {
                        IImportStatus iis = sourceItem as IImportStatus;
                        if (iis == null)
                            throw new Exception("Parse import file failed. The items must implement IImportStatus");

                        IImportEntity iie = sourceItem as IImportEntity;

                        if (iie == null)
                            throw new Exception("Parse import file failed. The items must implement IImportEntity");

                        try
                        {
                            iis.LineIndex = index - 1;
                            iis.ImportItemId = Guid.NewGuid();
                            iis.SessionId = this.SessionId;
                            iis.EntryDate = DateTime.Now;
                            Byte[] _bytes = SerializeItem(sourceItem);
                            iis.SerializedData = _bytes;
                            iis.SourceItem = sourceItem;
                        }
                        catch (Exception ex)
                        {
                            AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                            ec.Insert(0, string.Format("Could not serialize item at index {0}", index.ToString()));
                            ec.Insert(0, l);
                            throw ec.ToException();
                        }



                        try
                        {
                            try
                            {
                                iis.Validate();
                            }
                            catch (Exception ex)
                            {
                                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                                iis.Comment = string.Join("\n", ec.ToArray());
                                iis.EntityStatus = ImportEntityStatus.Error;
                            }

                            if (iis.EntityStatus == ImportEntityStatus.Error)
                                goto completesync;

                            if (iie.TargetExists())
                            {
                                if (UpdateIfExists)
                                {
                                    if (iie.IsSyncJustified())
                                        iis.EntityStatus = ImportEntityStatus.Update;
                                    else
                                        iis.EntityStatus = ImportEntityStatus.None;
                                }
                                else
                                    iis.EntityStatus = ImportEntityStatus.Ignore;
                            }
                            else
                            {
                                if (AddIfNotExist)
                                    iis.EntityStatus = ImportEntityStatus.Create;
                                else
                                    iis.EntityStatus = ImportEntityStatus.Ignore;
                            }
                        }

                        catch (Exception ex)
                        {
                            throw new Exception("Could not get sync status of this item", ex);
                        }

                    completesync:
                        iis.SerializedData = SerializeItem(iis);
                        iis.Record();


                    }
                }
            }



        }

        private object ParseLine(string line)
        {
            var item = Activator.CreateInstance(ItemType, true);
            char d = Delimiter.ToCharArray()[0];
            string[] record = line.Split(d);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int index = 0; index <= FieldNames.Count() - 1; index++)
            {
                dic.Add(FieldNames[index], record[index]);
            }

            foreach (string k in dic.Keys)
            {
                PropertyInfo p = item.GetType().GetProperty(k);

                if (p == null)
                    throw new Exception(string.Format("{0} could not be set. Make sure the field name is correct", k));

                if (p.CanWrite)
                {
                    string t = dic[k];
                    object v = null;

                    if (!string.IsNullOrEmpty(t))
                    {
                        try
                        {
                            if (p.PropertyType == typeof(DateTime))
                                v = Convert.ToDateTime(t);

                            if (p.PropertyType == typeof(string))
                                v = t;

                            if (p.PropertyType == typeof(Guid))
                                v = new Guid(t);

                            if (p.PropertyType == typeof(int))
                                v = Convert.ToInt32(t);

                            if (p.PropertyType.IsEnum)
                                v = System.Enum.Parse(p.PropertyType, t);

                            if (p.PropertyType == typeof(bool))
                                v = Convert.ToBoolean(t);

                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Could not parse item from string record", ex);
                        }

                        if (v != null)
                            p.SetValue(item, v, null);
                        else
                            throw new Exception(string.Format("{0} is not an accepted type", p.PropertyType.Name));
                    }
                }
            }

            return item;

        }

        static IEnumerable<string> ReadLines(Stream source, Encoding encoding)
        {
            using (StreamReader reader = new StreamReader(source, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public void Record()
        {

            LcpsDbContext db = new LcpsDbContext();

            ImportSession session = db.ImportSessions.FirstOrDefault(x => x.SessionId.Equals(this.SessionId));

            if (session == null)
            {
                ImportSession i = this.ToImportSession();
                db.ImportSessions.Add(i);
            }
            else
            {
                db.Entry(this.ToImportSession()).State = System.Data.Entity.EntityState.Modified;
            }


            db.SaveChanges();


        }

        public abstract void DetectConflicts();


        public bool AddIfNotExist { get; set; }

        public bool UpdateIfExists { get; set; }

        public List<IImportStatus> Candidates { get { return _candidates; } }

        public void Import()
        {
            List<ImportItem> cc = db.ImportItems.Where(x => x.SessionId.Equals(this.SessionId) & (x.EntityStatus == ImportEntityStatus.Create | x.EntityStatus == ImportEntityStatus.Update)).ToList();

            System.Type t = System.Type.GetType(FullAssemblyTypeName);

            foreach (ImportItem item in cc)
            {

                object i = DeserializeItem(t, item.SerializedData);

                IImportEntity entity = i as IImportEntity;

                switch (item.EntityStatus)
                {
                    case ImportEntityStatus.Create:
                        try
                        {
                            entity.Create();
                            item.Comment = "Added successfuly";
                            item.ImportStatus = ImportResultStatus.success;
                        }
                        catch (Exception ex)
                        {
                            AnvilExceptionCollector ec = new AnvilExceptionCollector(new Exception("Could not create entity", ex));
                            item.Comment = string.Join("\n", ec.ToArray());
                            item.ImportStatus = ImportResultStatus.danger;
                        }
                        break;
                    case ImportEntityStatus.Update:
                        try
                        {
                            entity.Update();
                            item.Comment = "Synced successfuly";
                            item.ImportStatus = ImportResultStatus.success;
                        }
                        catch (Exception ex)
                        {
                            AnvilExceptionCollector ec = new AnvilExceptionCollector(new Exception("Could not modify entity", ex));
                            item.Comment = string.Join("\n", ec.ToArray());
                            item.ImportStatus = ImportResultStatus.danger;
                        }
                        break;
                }

                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }


        public ImportSession ToImportSession()
        {
            ImportSession s = new ImportSession();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(s);
            return s;
        }

        public static byte[] SerializeItem(object item)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, item);
            return ms.ToArray();
        }

        public static object DeserializeItem(System.Type itemType, byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(bytes);
            object i = bf.Deserialize(ms);
            return i;
        }

        public static void ValidateItem(object item)
        {
            var context = new ValidationContext(item, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(item, context, results);

            if (!isValid)
            {
                string[] comments = results.Select(x => x.ErrorMessage).ToArray();
                AnvilExceptionCollector ec = new AnvilExceptionCollector("Validation failed");
                ec.AddRange(comments);
                throw ec.ToException();
            }
        }

        public static void Record(ImportItem item, LcpsDbContext db)
        {
            try
            {

                db.ImportItems.Add(item);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("A fatal error occured while recording import item status", ex);
            }
        }
    }
}
