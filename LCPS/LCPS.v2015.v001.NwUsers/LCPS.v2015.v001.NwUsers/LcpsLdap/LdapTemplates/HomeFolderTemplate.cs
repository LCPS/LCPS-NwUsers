using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;

using System.IO;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates
{
    [Table("HomeFolderTemplate", Schema = "LcpsLdap")]
    public class HomeFolderTemplate
    {
        [Key]
        public Guid HomeFolderId { get; set; }

        [Display(Name = "Template Name")]
        [Required]
        [MaxLength(75)]
        [Index("IX_HomeFolderTemplateName", IsUnique = true)]
        public string TemplateName { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        [Display(Name = "Path")]
        [Required]
        [MaxLength(1024)]
        public string HomeFoldePath { get; set; }

        [Display(Name = "ID Field")]
        public HomeFolderIdFields IdField { get; set; }

    }
}
