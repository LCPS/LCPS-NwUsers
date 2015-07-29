using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LcpsLdapSync
{
    [Table("OUSyncEntry", Schema = "LcpsLdap")]
    public class OUSyncEntry
    {
        [Key]
        public Guid EntrydId { get; set; }

        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Author")]
        [Required]
        public string EntryAuthor { get; set; }

        [ForeignKey("OUSyncLog")]
        [Required]
        public Guid LogId { get; set; }

        [Display(Name = "OU Id")]
        public Guid OUGuid { get; set; }

        [Display(Name = "OU Name")]
        [Required]
        [MaxLength(1024)]
        public string OUName { get; set; }

        [Display(Name = "User")]
        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }

        [Display(Name = "User ID")]
        public Guid UserId { get; set; }

        [Display(Name = "Action")]
        public OUSyncAction Action { get; set; }

        [Display(Name = "Result")]
        public string Result { get; set; }

        public virtual OUSyncLog OUSyncLog { get; set; }
    }
}
