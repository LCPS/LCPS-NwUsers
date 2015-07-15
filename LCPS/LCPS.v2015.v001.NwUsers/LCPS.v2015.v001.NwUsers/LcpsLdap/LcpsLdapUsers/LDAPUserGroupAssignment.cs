using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LcpsLdapUsers
{
    [Table("LDAPUserGroupAssignment", Schema = "LcpsLdap")]
    public class LDAPUserGroupAssignment
    {
        public enum LDAPUserGroupAssignmentAction 
        {
            Add = 0,
            Remove = 1
        }

        [Key]
        public Guid LDAPUserGroupAssignmentId { get; set; }

        public Guid LdapUserCatgoryId { get; set; }

        public Guid LdapGroupObjectGuid { get; set; }

        [ForeignKey("LdapUserCatgoryId")]
        [Required]
        public virtual LDAPUserGroupAssignment GroupAssignment { get; set; }


        #region Methods

        public LcpsAdsGroup GetGroup()
        {
            return new LcpsAdsGroup(LdapGroupObjectGuid);
        }

        #endregion
    }
}
