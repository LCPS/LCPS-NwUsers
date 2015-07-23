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

namespace LCPS.v2015.v001.NwUsers.LcpsEmail
{
    [Table("EmailAccount", Schema = "LcpsEmail")]
    public class EmailAccount
    {
        [Key]
        public Guid RecordId { get; set; }

        public Guid UserLink { get; set; }

        [Display(Name = "User")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Initial Password")]
        [DataType(DataType.Password)]
        public string InitialPassword { get; set; }
    }
}
