using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LCPS.v2015.v001.NwUsers.Security
{
    [Table("LcpsEmail", Schema = "LcpsSecurity")]
    public class LcpsEmail 
    {

        [Display(Name = "Email Address")]
        [Required]
        [MaxLength(50)]
        [Index("IX_LCPSEmailAddress", IsUnique = true)]
        public string Email { get; set; }

        [Required]
        [MaxLength(128)]
        public string InitialPassword { get; set; }

    }
}
