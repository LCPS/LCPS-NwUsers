#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




#endregion

namespace Anvil.v2015.v001.Domain.Entities
{
    [Table("ApplicationBase", Schema = "Anvil")]
    public class ApplicationBase
    {
        #region Events
        #endregion

        #region Constants
        #endregion

        #region Fields
        #endregion

        #region Constructors

        public ApplicationBase()
        {

        }

        #endregion

        #region Properties


        //---------- Identification
        [Key]
        public Guid RecordId { get; set; }

        [Display(Name = "Application Name")]
        [Required]
        [MaxLength(128)]
        [Index("ApplicationName_IX", IsUnique = true)]
        public string AppName { get; set; }

        [Display(Name = "Title")]
        [Required]
        [MaxLength(128)]
        public string Title { get; set; }

        [Display(Name = "Mission Statement")]
        [MaxLength(4096)]
        public string MissionStatement { get; set; }


        // Email Settings

        [Display(Name = "SMTP Server")]
        [MaxLength(128)]
        public string SMTPServer { get; set; }

        [Display(Name = "SMTP Port")]
        public int SMPTPPort { get; set; }

        [Display(Name = "Use Security Layer")]
        public bool SMTPEnableSSL { get; set; }


        [Display(Name = "SMTP User Name")]
        [MaxLength(75)]
        public string SMTPUserName { get; set; }

        [Display(Name = "SMTP Password")]
        public string SMTPPassword { get; set; }

        //Password Policy

        [Display(Name = "Password Length", Description = "The password must contain this number of characters at the least")]
        public int PWDRequiredLength { get; set; }

        [Display(Name = "Non-Aplha Numeric Characters", Description = "The password must contain at least non-alpha-numeric character")]
        public bool PWDRequireNonLetterOrDigit { get; set; }

        [Display(Name = "Numeric Characters", Description = "The password must have at least one numeric character")]
        public bool PWDRequireDigit { get; set; }

        [Display(Name = "Lowercase Characters", Description = "The password must have at least one lowercase alpha character")]
        public bool PWDRequireLowercase { get; set; }

        [Display(Name = "Uppercase Characters", Description = "The password must have at least one uppercase character")]
        public bool PWDRequireUppercase { get; set; }

        [Display(Name = "Enable User Lockout", Description = "If enabled the suspect account will be locked out after an specified nunber of failed login attempts")]
        public bool PWDUserLockoutEnabledByDefault { get; set; }

        [Display(Name = "Lockout TimeSpan", Description = "If a failed login attempt has not been executed with this number of minutes the lock-out count will trip back to zero")]
        public int PWDDefaultAccountLockoutTimeSpan { get; set; }

        [Display(Name = "Maximum Failed Attempts", Description = "The number of failed attempts before the suspect account is locked out")]
        public int PWDMaxFailedAccessAttemptsBeforeLockout { get; set; }

        public static List<string> GetPasswordPolicy(ApplicationBase app)
        {
            List<string> l = new List<string>();

            l.Add(string.Format("Contain {0} characters", app.PWDRequiredLength.ToString()));


            if (app.PWDRequireDigit)
                l.Add("Contain at least one numeric character");

            if (app.PWDRequireLowercase)
                l.Add("Contain at least one lowercase character");

            if (app.PWDRequireUppercase)
                l.Add("Contain at least one uppercase character");

            if (app.PWDRequireNonLetterOrDigit)
                l.Add("Contain at least one non-aplha-numeric character");

            return l;
        }


        // -----------------------------------------------------------------------
        // --------------------------------------------------- LDAP
        // -----------------------------------------------------------------------

        [Display(Name = "Domain Name (Fully Qualified)")]
        [MaxLength(128)]
        public string LDAPDomainFQN { get; set; }

        [Display(Name = "Domain Name")]
        [MaxLength(128)]
        public string LDAPDomain { get; set; }

        [Display(Name = "User Name", Description = "A login id with domain admin privelages")]
        [MaxLength(128)]
        public string LDAPUserName { get; set; }

        [Display(Name = "Password")]
        [MaxLength(128)]
        public string LDAPPassword { get; set; }

        #endregion

    }
}
