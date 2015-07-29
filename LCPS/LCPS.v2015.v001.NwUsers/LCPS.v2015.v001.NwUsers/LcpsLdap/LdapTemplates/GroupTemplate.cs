using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates
{
    [Table("GroupTemplate", Schema = "LcpsLdap")]
    public class GroupTemplate
    {
        #region Fields

        LcpsAdsGroup _group;

        #endregion

        [Key]
        public Guid GroupId { get; set; }

        [Display(Name = "Template Name")]
        [Required]
        [MaxLength(75)]
        [Index("IX_GroupTemplateName", IsUnique = true)]
        public string TemplateName { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName
        {
            get
            {
                return GetGroup().Name;
            }
        }

        [Display(Name = "OU Description")]
        public string GroupDescription
        {
            get { return GetGroup().Description; }
        }

        #region Methods

        public LcpsAdsGroup GetGroup()
        {
            bool refresh = false;

            if (_group == null) 
                refresh = true;

            if (_group != null && (!_group.ObjectGuid.Equals(GroupId)))
                refresh = true;

            if (refresh)
            {
                try
                {
                    _group = new LcpsAdsGroup(GroupId);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} is an invalid Group", GroupId.ToString()), ex);
                }
            }
            return _group;

        }


        #endregion
    }
}
