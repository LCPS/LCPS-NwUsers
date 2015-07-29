using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCPS.v2015.v001.NwUsers.Importing2;

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public class HRStaffImportFile : IImportFile
    {

        public string[] Columns { get; set; }

        public bool Overwrite { get; set; }

        public List<IImportFileRecord> Lines { get; set; }

        public System.IO.StreamReader Contents { get; set; }

        public Type ItemType { get; set; }

        public char Delimiter { get; set; }

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
                        for(int i = 0; i <=8; i++)
                        {

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error reading input file", ex);
            }
        }

        public void ParseLine(string line, int lineIndex)
        {
            throw new NotImplementedException();
        }

        public void Import()
        {
            throw new NotImplementedException();
        }
    }
}
