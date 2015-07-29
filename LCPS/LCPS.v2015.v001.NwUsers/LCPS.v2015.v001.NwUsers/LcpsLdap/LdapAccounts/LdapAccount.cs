using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapAccounts
{
    [Table("LdapAccount", Schema = "LcpsLdap")]
    public class LdapAccount
    {
        [Key]
        public Guid AccountId { get; set; }

        [Index("IX_LdapAccount_UserName", IsUnique = true, Order = 1)]
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Index("IX_LdapAccount_UserName", IsUnique = true, Order = 2)]
        public Guid UserKey { get; set; }

        [Required]
        [MaxLength(256)]
        public string InitialPassword { get; set; }

        public bool Active { get; set; }
        
    }
}
