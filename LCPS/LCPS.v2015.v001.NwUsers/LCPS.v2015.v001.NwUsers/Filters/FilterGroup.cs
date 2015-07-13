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

namespace LCPS.v2015.v001.NwUsers.Filters
{
    public class FilterGroup
    {
        #region Events
        #endregion

        #region Constants
        #endregion

        #region Fields

        [Key]
        public Guid FilterId { get; set; }

        public Guid AntecedentId { get; set; }

        [Index("IX_FilterName", IsUnique=true)]
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Description { get; set; }


        #endregion

     

        #region Properties
        #endregion

    }
}
