#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#endregion

namespace Anvil.v2015.v001.Domain.Entities
{
    public class ApplicationDetailModel
    {
        public ApplicationDetailModel()
        { }

        public ApplicationDetailModel(ApplicationBase app)
        {
            AppName = app.AppName;
            Title = app.Title;
            MissionStatement = app.MissionStatement;
        }

        public Guid RecordId { get; set; }

        [Display(Name = "Application Name")]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(128)]
        public string AppName { get; set; }

        [Display(Name = "Title")]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(128)]
        public string Title { get; set; }

        [Display(Name = "Mission Statement")]
        [MaxLength(4096)]
        public string MissionStatement { get; set; }

    }

    public class ApplicationSMTPModel
    {
        public ApplicationSMTPModel()
        { }

        public ApplicationSMTPModel(ApplicationBase app)
        {
            this.SMPTPPort = app.SMPTPPort;
            this.SMTPPassword = app.SMTPPassword;
            this.SMTPServer = app.SMTPServer;
            this.SMTPUserName = app.SMTPUserName;
            this.SMTPEnableSSL = app.SMTPEnableSSL;
        }

        public Guid RecordId { get; set; }

        [Display(Name = "SMTP Server")]
        [MaxLength(128)]
        [Required(AllowEmptyStrings = false)]
        public string SMTPServer { get; set; }

        [Display(Name = "SMTP Port")]
        public int SMPTPPort { get; set; }

        [Display(Name = "Use Security Layer")]
        public bool SMTPEnableSSL { get; set; }

        [Display(Name = "SMTP User Name")]
        [EmailAddress]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(75)]
        public string SMTPUserName { get; set; }

        [Display(Name = "SMTP Password")]
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string SMTPPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("SMTPPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string SMTPConfirmPassword { get; set; }
    }

    public class ApplicationPasswordPolicyModel
    {
        public ApplicationPasswordPolicyModel()
        { }

        public ApplicationPasswordPolicyModel(ApplicationBase app)
        {
            this.PWDDefaultAccountLockoutTimeSpan = app.PWDDefaultAccountLockoutTimeSpan;
            this.PWDMaxFailedAccessAttemptsBeforeLockout = app.PWDMaxFailedAccessAttemptsBeforeLockout;
            this.PWDRequireDigit = app.PWDRequireDigit;
            this.PWDRequiredLength = app.PWDRequiredLength;
            this.PWDRequireLowercase = app.PWDRequireLowercase;
            this.PWDRequireNonLetterOrDigit = app.PWDRequireNonLetterOrDigit;
            this.PWDRequireUppercase = app.PWDRequireUppercase;
            this.PWDUserLockoutEnabledByDefault = app.PWDUserLockoutEnabledByDefault;
        }

        public Guid RecordId { get; set; }

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
    }

    public class ApplicationLDSPModel
    {

        public ApplicationLDSPModel()
        { }

        public ApplicationLDSPModel(ApplicationBase app)
        {
            this.LDAPDomain = app.LDAPDomain;
            this.LDAPDomainFQN = app.LDAPDomainFQN;
            this.LDAPPassword = app.LDAPPassword;
            this.LDAPUserName = app.LDAPUserName;
        }

        public Guid RecordId { get; set; }

        [Display(Name = "Domain Name (Fully Qualified)")]
        [MaxLength(128)]
        [Required(AllowEmptyStrings = false)]
        public string LDAPDomainFQN { get; set; }

        [Display(Name = "Domain Name")]
        [MaxLength(128)]
        [Required(AllowEmptyStrings = false)]
        public string LDAPDomain { get; set; }

        [Display(Name = "User Name", Description = "A login id with domain admin privelages")]
        [MaxLength(128)]
        [Required(AllowEmptyStrings = false)]
        public string LDAPUserName { get; set; }

        [Display(Name = "Password")]
        [MaxLength(128)]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false)]
        public string LDAPPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [MaxLength(128)]
        [Compare("LDAPPassword", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string LDAPConfirmPassword { get; set; }
    }

}
