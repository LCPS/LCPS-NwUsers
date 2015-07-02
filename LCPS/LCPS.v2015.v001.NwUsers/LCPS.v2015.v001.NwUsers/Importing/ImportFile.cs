using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCPS.v2015.v001.NwUsers.Importing
{
    [Table("ImportFile", Schema = "Importing")]
    public abstract class ImportFile
    {
        #region Enum

        public enum ListQualifier
        {
            Read = 0,
            Import = 1
        }

        #endregion

        #region Fields

        string[] _headers;

        List<ImportItem> _items = new List<ImportItem>();

        #endregion

        #region Constructors

        public ImportFile(ImportSession session)
        {
            this.Session = session;
        }

        #endregion

        #region Properties

        public ImportSession Session { get; set; }

        public ListQualifier ListFor { get; set; }

        public abstract List<ImportItem> Items { get; }

        #endregion

        #region Parse

        public void ReadFile()
        {
            int index = 0;
            using (StreamReader r = new StreamReader(new MemoryStream(Session.ImportFileContents)))
            {
                while (r.Peek() >= 0)
                {
                    if (index == 0)
                    {
                        Session.FieldNames = r.ReadLine();
                        _headers = Session.FieldNames.Split('\t');
                        index++;
                    }
                    else
                    {
                        string l = r.ReadLine();
                        Dictionary<string, string> _def = ParseToDictionary(l);

                        ImportItem i = new ImportItem()
                        {
                            RecordId = Guid.NewGuid(),
                            SessionId = this.Session.SessionId,
                            EntryDate = DateTime.Now
                        };

                        object item = null;
                        try
                        {
                            item = ParseItem(_def);
                        }
                        catch(Exception ex)
                        {
                            item = null;
                            i.Description = l;
                            i.Comment = (string.Join("\n", (new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionCollector(ex).ToArray())));
                            i.ReadStatus = ImportItem.LcpsImportStatus.danger;
                        }

                        if (item != null)
                        {
                            XmlSerializer x = new XmlSerializer(item.GetType());
                            MemoryStream m = new MemoryStream();
                            x.Serialize(m, item);

                            i.SerializedItem = m.ToArray();
                            i.Description = item.ToString();



                            ImportStatus status = GetReadStatus(item);

                            i.ReadStatus = status.Status;
                            i.Comment = status.Comment;

                            _items.Add(i);
                        }
                        WriteImportItemToDb(i);
                    }
                }
            
                _items = _items.OrderBy(x => x.EntryDate).ToList();
            
            }
        }

        public virtual Dictionary<string, string> ParseToDictionary(string record)
        {
            Dictionary<string, string> _item = new Dictionary<string, string>();

            string[] _values = record.Split('\t');

            for (int i = 0; i <= _headers.Length - 1; i++)
            {
                _item.Add(_headers[i], _values[i]);
            }

            return _item;
        }

        public virtual object ParseItem(Dictionary<string, string> item)
        {
            System.Type t = System.Type.GetType(Session.FullAssemblyTypeName);

            object target = Activator.CreateInstance(t, true);
            foreach (string k in item.Keys)
            {
                PropertyInfo p = t.GetProperty(k);
                if (p != null)
                {
                    string v = item[k];

                    if (p.PropertyType == typeof(int))
                        p.SetValue(target, Convert.ToInt32(v), null);

                    if (p.PropertyType == typeof(DateTime))
                    {
                        if (!string.IsNullOrEmpty(v))
                        {
                            try
                            {
                                p.SetValue(target, Convert.ToDateTime(v), null);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("{0} is not a valid date.", v), ex);
                            }
                        }
                    }

                    if(p.PropertyType.IsEnum)
                    {
                        try
                        {
                            object e = System.Enum.Parse(p.PropertyType, v);
                            p.SetValue(target, e, null);
                        }
                        catch(Exception ex)
                        {
                            throw new Exception(string.Format("{0} is not a valid enum value for {1}", v, p.PropertyType.Name), ex);
                        }
                    }

                    if (p.PropertyType == typeof(Guid))
                        p.SetValue(target, new Guid(v), null);

                    if (p.PropertyType == typeof(string))
                        p.SetValue(target, v, null);

                    if (p.PropertyType == typeof(bool))
                        p.SetValue(target, Convert.ToBoolean(v), null);

                    
                }
            }



            return target;

        }

        public virtual ImportStatus GetReadStatus(object item)
        {
            var context = new ValidationContext(item, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(item, context, results);

            if (isValid)
            {
                return new ImportStatus()
                {
                    Status = ImportItem.LcpsImportStatus.@default,
                    Comment = null
                };
            }
            else
            {
                string[] comments = results.Select(x => x.ErrorMessage).ToArray();
                return new ImportStatus()
                {
                    Status = ImportItem.LcpsImportStatus.danger,
                    Comment = string.Join("\n", comments)
                };
            }


        }


        public abstract void WriteImportItemToDb(ImportItem item);



        #endregion

        public T GetAttributeFrom<T>(object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            T t = (T)property.GetCustomAttributes(attrType, false).FirstOrDefault();
            if (t == null)
            {
                MetadataTypeAttribute[] metaAttr = (MetadataTypeAttribute[])instance.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), true);
                if (metaAttr.Length > 0)
                {
                    foreach (MetadataTypeAttribute attr in metaAttr)
                    {
                        var subType = attr.MetadataClassType;
                        var pi = subType.GetField(propertyName);
                        if (pi != null)
                        {
                            t = (T)pi.GetCustomAttributes(attrType, false).FirstOrDefault();
                            return t;
                        }


                    }
                }

            }
            else
            {
                return t;
            }
            return null;
        }

    }
}
