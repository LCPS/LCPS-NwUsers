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
    public class LcpsAdsCredential 
    {
        [Display(Name = "User Name")]
        [Required]
        [MaxLength(50)]
        [Index("IX_ADSUserName", IsUnique = true)]
        public string UserName { get; set;}
    }
}
