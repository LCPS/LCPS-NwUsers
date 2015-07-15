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
    public class LDAPUserCategory
    {
        [Key]
        public Guid LdapUserCatgoryId { get; set; }

        [Display(Name = "Category")]
        [Required]
        [Index("IX_LdapUserCategoryName", IsUnique = true)]
        [MaxLength(75)]
        public string Name { get; set; }

        [Display(Name = "Type", Description = "Denotes if this category applies to students or staff")]
        public LDAPUserCategoryType CategoryType { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MaxLength(4096)]
        public string Description { get; set; }

        [Display(Name = "LDAP Organizational Unit", Description = "The organizational unit these users will exist in")]
        public Guid LdapOuGuid { get; set; }

        [Display(Name = "User Account Flags")]
        public LcpsAdsUserAccountControl UserAccountControl { get; set; }

        [Display(Name = "Home Folder Repository", Description = @"The folder the user's home folder will be created in. (ie. \\server\share\ -- would result in a folder named '\\server\share\username' being created)")]
        [Required]
        [MaxLength(4096)]
        public string HomeFolderRepository { get; set; }


        #region Methods

        public LcpsAdsOu GetOrganizationalUnit()
        {
            return new LcpsAdsOu(this.LdapOuGuid);
        }

        #endregion
    }
}
