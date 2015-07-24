
using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.NwUsers.Importing2
{
    public class ImportFile : IImportFile
    {
        #region Fields

        private List<IImportFileRecord> _lines = new List<IImportFileRecord>();
        private LcpsDbContext _dbContext = new LcpsDbContext();

        #endregion

        #region Constructors

        public ImportFile(Stream contents, Char delimiter)
        {
            this.Delimiter = delimiter;
            MemoryStream ms = new MemoryStream();
            contents.CopyTo(ms);
            ms.Position = 0;
            this.Contents = new StreamReader(ms);
        }

        #endregion

        public bool Overwrite { get; set; }

        public string[] Columns { get; set; }

        public StreamReader Contents { get; set; }

        public List<object> Items { get; set; }

        public void ParseFile()
        {

            try
            {
                Items = new List<object>();



                int index = 0;

                while (!this.Contents.EndOfStream)
                {
                    string line = this.Contents.ReadLine();
                    if (index == 0)
                    {
                        Columns = line.Split(this.Delimiter);
                        index++;
                    }
                    else
                    {
                        this.ParseLine(line);

                        IImportFileRecord record = Lines[index - 1];

                        record.Validate(_dbContext);

                        if (record.ValidationStatus != ImportRecordStatus.danger)
                            record.GetCrudStatus(_dbContext);

                        index++;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error reading input file", ex);
            }

        }

        public virtual void ParseLine(string line)
        {
            var item = Activator.CreateInstance(ItemType, true);

            ((IImportFileRecord)item).Fields = line.Split(this.Delimiter);

            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int index = 0; index <= Columns.Count() - 1; index++)
            {
                dic.Add(Columns[index], ((IImportFileRecord)item).Fields[index]);
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
            Lines.Add((IImportFileRecord)item);

        }

        public void Import()
        {
            foreach (IImportFileRecord item in Lines)
            {
                if (item.CrudStatus == ImportCrudStatus.Insert)
                    item.Create(_dbContext);

                if (item.CrudStatus == ImportCrudStatus.Update & Overwrite)
                    item.Update(_dbContext);

            }
        }

        public Type ItemType { get; set; }

        public char Delimiter { get; set; }

        public List<IImportFileRecord> Lines
        {
            get { return _lines; }
            set { _lines = value; }
        }




    }
}
