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
    [Table("OUTemplate", Schema = "LcpsLdap")]
    public class OUTemplate
    {
        #region Fields

        LcpsAdsOu _ou;

        #endregion

        [Key]
        public Guid OUId { get; set; }

        [Display(Name = "Template Name")]
        [Required]
        [MaxLength(75)]
        [Index("IX_OUTemplateName", IsUnique = true)]
        public string TemplateName { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        [Display(Name = "Apply To")]
        public Filters.MemberFilterClass FilterClass { get; set; }

        [Display(Name = "OU Name")]
        public string OUName
        {
            get
            {
                return GetOu().Name;
            }
        }

        [Display(Name = "OU Description")]
        public string OUDescription
        {
            get { return GetOu().Description; }
        }

        #region Methods

        public LcpsAdsOu GetOu()
        {
            bool refresh = false;

            if (_ou == null) 
                refresh = true;

            if (_ou != null && (!_ou.ObjectGuid.Equals(OUId)))
                refresh = true;

            if (refresh)
            {
                try
                {
                    _ou = new LcpsAdsOu(OUId);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} is an invalid OU", OUId.ToString()), ex);
                }
            }
            return _ou;

        }


        #endregion
    }
}
