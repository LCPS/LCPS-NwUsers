using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapAccounts
{

    [MetadataType(typeof(StudentLdapAccountdMetaData))]
    partial class StaffLdapAccount
    {
        [Display(Name = "Name")]
        public string SortName
        {
            get { return this.LastName + ", " + this.FirstName;  }
        }
    }

    public class StudentLdapAccountdMetaData
    {
        [Display(Name = "Staff Id")]
        public string StaffId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Initial")]
        public string MiddleInitial { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Active")]
        public string Active { get; set; }

        

    }
}
