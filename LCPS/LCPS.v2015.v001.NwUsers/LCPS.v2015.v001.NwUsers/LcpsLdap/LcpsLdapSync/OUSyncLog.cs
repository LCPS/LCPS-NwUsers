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
    [Table("OUSyncLog", Schema = "LcpsLdap")]
    public class OUSyncLog
    {
        [Key]
        public Guid LogId { get; set; }

        [Display(Name = "Author")]
        public string LogAuthor { get; set; }

        [Display(Name = "Log Date")]
        public DateTime LogDate { get; set; }

        public virtual ICollection<OUSyncEntry> LogEntries { get; set; }
    }
}
