using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace LCPS.v2015.v001.NwUsers.Importing
{
    [Table("ImportItem", Schema = "Importing")]
    public class ImportItem : IImportStatus
    {
        ImportSession _session = new ImportSession();

        [Key]
        public Guid ImportItemId { get; set; }

        public Guid SessionId { get; set; }

        [ForeignKey("SessionId")]
        public virtual ImportSession Session { get; set; }
        
        public string Comment { get; set; }

        public string Description { get; set; }

        public DateTime EntryDate { get; set; }

        public ImportResultStatus ImportStatus { get; set; }

        public byte[] SerializedData { get; set; }

        public object SourceItem { get; set; }

        public void Serialize()
        {
            throw new NotImplementedException();
        }

      
        public ImportEntityStatus EntityStatus { get; set; }

        public int LineIndex { get; set; }

        public void Validate()
        {
            throw new NotImplementedException();
        }

        public void Record()
        {
            throw new NotImplementedException();
        }

        public ImportItem ToImportItem()
        {
            throw new NotImplementedException();
        }


        public object Deserialize()
        {
            throw new NotImplementedException();
        }

        public object Deserialize(IImportSession session)
        {
            System.Type t = System.Type.GetType(session.FullAssemblyTypeName);
            return ImportFileTSV.DeserializeItem(t, SerializedData);
        }
    }
}
