using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace LCPS.v2015.v001.NwUsers.Importing
{
    [Table("ImportStatus", Schema = "Importing")]
    public class ImportSession
    {
        [Key]
        public Guid SessionId { get; set; }

        [Display(Name = "Session Date")]
        [DataType(DataType.Date)]
        public DateTime SessionDate { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Item Type")]
        public string TypeName { get; set; }

        [Display(Name = "Assembly Qualified Name")]
        public string FullAssemblyTypeName { get; set; }

        public string FieldNames { get; set; }

        [Display(Name = "Import")]
        public byte[] ImportFileContents { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action {get; set;}

        public ImportFile.ListQualifier DetailMode { get; set; }

        public string ViewLayoutPath { get; set; }
    }
}
