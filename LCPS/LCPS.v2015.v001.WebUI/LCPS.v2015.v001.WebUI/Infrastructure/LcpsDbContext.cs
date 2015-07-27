using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Configuration;
using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Infrastructure
{
    public class LcpsDbContext : LCPS.v2015.v001.NwUsers.Infrastructure.LcpsDbContext
    {
        #region Constants

        public const string UserAdminName = "lcps";
        public const string UserAdminEmail = "lcps@k12lcps.org";
        public const string UserAdminPassword = "Lcp$-pw1";

        #endregion

        public LcpsDbContext()
            : base(ConfigurationManager.ConnectionStrings["LcpsDbContext"].ConnectionString)
        {
            this.Configuration.ProxyCreationEnabled = false;

        }

        #region Properties

        public static ApplicationBase Application
        {
            get
            {
                try
                {
                    LcpsDbContext db = new LcpsDbContext();
                    return db.Applications.ToList()[0];
                }
                catch (Exception ex)
                {
                    throw new Exception("Fatal Error: No default application definition was found in the database", ex);
                }

            }
        }


        #endregion

        public static LcpsDbContext Create()
        {
            return new LcpsDbContext();
        }


        public void Seed()
        {
            SeedDefaultSecurityEntities();
            SeedDefaultApplication();
            SeedSql();
            SeedDefaultACL();
        }

        public void SeedDefaultSecurityEntities()
        {
            try
            {
                LcpsRoleManager rm = new LcpsRoleManager();
                LcpsUserManager um = new LcpsUserManager();

                ApplicationUserCandidateCollection defaultUsers = new ApplicationUserCandidateCollection();
                defaultUsers.Add("lcps", "Lcp$-2015", "lcps@k12lcps.org");

                ApplicationRoleCollection defaultRoles = new ApplicationRoleCollection();
                defaultRoles.Add(LcpsRoleManager.ApplicationAdminRole, "This user has full access to all application modules and objects");
                defaultRoles.Add(LcpsRoleManager.HrAdminRole, "This user has full access to all application modules and objects in the Human Resources area");
                defaultRoles.Add(LcpsRoleManager.StudentAdminRole, "This user has full access to all application modules and objects in the Students area");
                defaultRoles.Add(LcpsRoleManager.StudentEmailRole, "This user can modify student email accounts");
                defaultRoles.Add(LcpsRoleManager.StaffEmailRole, "This user can modify staff email accounts");
                defaultRoles.Add(LcpsRoleManager.StudentLanRole, "This user can modify staff LAN accounts");
                defaultRoles.Add(LcpsRoleManager.StaffLanRole, "This user can modify staff LAN accounts");
                defaultRoles.Add(LcpsRoleManager.StudentPwdRole, "This user can modify student passwords");
                defaultRoles.Add(LcpsRoleManager.StaffPwdRole, "This user can modify staff passwords");

                ApplicationUserRoleValidator _roleValidator = new ApplicationUserRoleValidator(this, um, rm);
                foreach (ApplicationRole r in defaultRoles)
                {
                    _roleValidator.Add(defaultUsers[0], r);
                }

                _roleValidator.Validate();


            }
            catch (Exception ex)
            {
                throw new Exception("Could not create default entities", ex);
            }
        }

        public void SeedDefaultApplication()
        {
            try
            {
                if (Applications.Count() == 0)
                {
                    ApplicationBase app = new ApplicationBase()
                    {
                        AppName = "Lunenburg County Public Schools",
                        LDAPDomain = "lcps",
                        LDAPDomainFQN = "lcps.local",
                        LDAPPassword = "Lcp$-2015",
                        LDAPUserName = "lcps",
                        PWDDefaultAccountLockoutTimeSpan = 15,
                        PWDMaxFailedAccessAttemptsBeforeLockout = 5,
                        PWDRequireDigit = true,
                        PWDRequireLowercase = true,
                        PWDRequiredLength = 6,
                        PWDRequireNonLetterOrDigit = true,
                        PWDRequireUppercase = true,
                        PWDUserLockoutEnabledByDefault = true,
                        SMPTPPort = 587,
                        SMTPEnableSSL = true,
                        SMTPPassword = "#",
                        SMTPServer = "smtp.gmail.com",
                        SMTPUserName = "#",
                        RecordId = Guid.NewGuid(),
                        Title = "LCPS",
                        MissionStatement = "LCPS"
                    };

                    Applications.Add(app);
                    SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create default application", ex);
            }
        }

        public void SeedDefaultACL()
        {
            LcpsMvcControllerACLManager manager = new LcpsMvcControllerACLManager();
            manager.AddAccess(LcpsRoleManager.ApplicationAdminRole, "HumanResources");
            manager.AddAccess(LcpsRoleManager.HrAdminRole, "HumanResources");
            
            manager.AddAccess(LcpsRoleManager.ApplicationAdminRole, "Students");
            manager.AddAccess(LcpsRoleManager.StudentAdminRole, "Students");

            manager.AddAccess(LcpsRoleManager.StudentAdminRole, "My");

            manager.AddAccess(LcpsRoleManager.ApplicationAdminRole, "LcpsLdap");




        }

    }
}