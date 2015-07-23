
using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace LCPS.v2015.v001.NwUsers.Importing2
{
    public class ImportFile : IImportFile
    {
        #region Constructors

        public ImportFile(Stream contents, Char delimiter)
        {
            this.Delimiter = delimiter;
            this.ParseFile(contents);
        }

        #endregion

        public bool Overwrite { get; set; }

        public string[] Columns { get; set; }

        public StreamReader Contents { get; set; }

        public List<object> Items { get; set; }

        public void ParseFile(Stream content)
        {

            try
            {
                Items = new List<object>();

                MemoryStream ms = new MemoryStream();
                content.CopyTo(ms);
                ms.Position = 0;

                this.Contents = new StreamReader(ms);

                int index = 0;

                while(!this.Contents.EndOfStream)
                {
                    string line = this.Contents.ReadLine();
                    if (index == 0)
                    {
                        Columns = line.Split(this.Delimiter);
                        index++;
                    }
                    else
                    {
                        ImportFileRecord record = new ImportFileRecord()
                        {
                            Index = index,
                            Fields = line.Split(this.Delimiter),
                            Content = line,
                        };

                        this.ParseLine(line);

                        record.Validate();

                        if (record.ValidationStatus != ImportRecordStatus.danger)
                            record.GetCrudStatus();

                        Lines.Add(record);

                        index ++;
                    }
                }

            }
            catch(Exception ex)
            {
                throw new Exception("Error reading input file", ex);
            }

        }

        public virtual void ParseLine(string line)
        {
            var item = Activator.CreateInstance(ItemType, true);
            
            string[] record = line.Split(this.Delimiter);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int index = 0; index <= Columns.Count() - 1; index++)
            {
                dic.Add(Columns[index], record[index]);
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

            Items.Add(item);

        }

        public void Import()
        {
            foreach(ImportFileRecord item in Lines)
            {
                if (item.CrudStatus == ImportCrudStatus.Insert)
                    item.Create();

                if (item.CrudStatus == ImportCrudStatus.Update & Overwrite)
                    item.Update();

            }
        }

        public Type ItemType { get; set; }

        public char Delimiter { get; set; }

        public List<IImportFileRecord> Lines { get; set; }




    }
}
