using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using System.IO;

namespace LCPS.v2015.v001.NwUsers.Importing
{
    [Table("ImportItem", Schema = "Importing")]
    public class ImportItem
    {
        public enum LcpsImportStatus
        {
            @default = 0,
            success = 1,
            info = 2,
            warning = 3,
            danger = 4
        }


        [Key]
        public Guid RecordId { get; set; }

        public Guid SessionId { get; set; }

        public byte[] SerializedItem { get; set; }

        [Display(Name = "Candidate Status")]
        public LcpsImportStatus ReadStatus { get; set; }

        [Display(Name = "Import Status")]
        public LcpsImportStatus ImportStatus { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        public Object GetDeserialized(string assemblyTypeName)
        {
            System.Type t = System.Type.GetType(assemblyTypeName);
            return Deserialize(SerializedItem, t);
        }

        public static object Deserialize(byte[] content, System.Type itemType)
        {
            XmlSerializer x = new XmlSerializer(itemType);
            MemoryStream m = new MemoryStream(content);
            return x.Deserialize(m);
        }

    }
}
