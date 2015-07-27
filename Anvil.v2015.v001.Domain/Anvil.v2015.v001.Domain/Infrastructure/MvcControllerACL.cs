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


namespace Anvil.v2015.v001.Domain.Infrastructure
{
    [Table("MvcControllerACL", Schema = "Infrastructure")]
    public class MvcControllerACL
    {
        [Key]
        public Guid RecordId { get; set; }

        [MaxLength(35)]
        public string Area { get; set;}

        [MaxLength(35)]
        public string Controller { get; set;}

        [MaxLength(35)]
        public string Action { get; set; }

        [MaxLength(128)]
        public string RoleId { get; set; }

    }
}
