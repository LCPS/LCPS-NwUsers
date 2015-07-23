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

namespace LCPS.v2015.v001.NwUsers.Support
{
    [Table("SuuportDepartment", Schema = "Support")]
    public class SupportDepartment
    {
        #region Events
        #endregion

        #region Constants
        #endregion

        #region Fields
        #endregion

        #region Constructors

        public SupportDepartment()
        {

        }

        #endregion

        #region Properties

        [Key]
        public Guid DepartmentId { get; set; }


        [Display(Name = "Department")]
        [Index("IX_SupportDepartmentName", IsUnique = true)]
        public string DepartmentName { get; set; }

        #endregion

    }
}
