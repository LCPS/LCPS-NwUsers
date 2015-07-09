#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#endregion



namespace LCPS.v2015.v001.NwUsers.HumanResources.Students
{

    [Table("InstructionalLevel", Schema = "HumanResources")]
    public class InstructionalLevel : IInstructionalLevel
    {
        [Key]
        public Guid InstructionalLevelKey { get; set; }

        [Index("IX_ILevelId", IsUnique = true)]
        [Required]
        [MaxLength(25)]
        [Display(Name = "Grade ID")]
        public string InstructionalLevelId { get; set; }

        [Index("IX_ILevelName", IsUnique = true)]
        [Required]
        [MaxLength(128)]
        [Display(Name = "Grade")]
        public string InstructionalLevelName { get; set; }
    }
}
