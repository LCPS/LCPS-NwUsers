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

namespace LCPS.v2015.v001.NwUsers.Support
{
    [Table("SupprotStaff", Schema = "Support")]
    public class SupportStaff
    {
        [Key]
        Guid SupportStaffId { get; set; }

        [Index("IX_SupportStaffMember", IsUnique = true, Order = 1)]
        public Guid UserId { get; set; }

        [Index("IX_SupportStaffMember", IsUnique = true, Order = 1)]
        public Guid DepartmentId { get; set; }

        public SupportStaffStatus Status { get; set;}


    }
}
