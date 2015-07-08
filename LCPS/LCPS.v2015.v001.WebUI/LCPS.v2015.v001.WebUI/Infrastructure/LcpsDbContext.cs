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

        public const string RoleAdminName = "APP-Admins";
        public const string RoleAdminDesc = "Add, edit, and delete roles and users";

        #endregion

        public LcpsDbContext()
            :base(ConfigurationManager.ConnectionStrings["LcpsDbContext"].ConnectionString)
        {
            this.Configuration.ProxyCreationEnabled = false;
            // helloe world
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
            try
            {
                LcpsRoleManager rm = new LcpsRoleManager();
                LcpsUserManager um = new LcpsUserManager();

                if(!rm.RoleExists(RoleAdminName))
                    rm.Create(new ApplicationRole() { Name = RoleAdminName, Description = RoleAdminDesc });

                if(um.FindByName(UserAdminName) == null)
                    um.Create(new ApplicationUser(){
                        UserName = UserAdminName, 
                        Email = UserAdminEmail, 
                        Birthdate = new DateTime(1953, 6, 12),
                        GivenName = "System",
                        SurName = "Admin"
                    }, UserAdminPassword);

                ApplicationUser u = um.FindByName(UserAdminName);

                if (!um.IsInRole(u.Id, RoleAdminName))
                    um.AddToRole(u.Id, RoleAdminName);

                if(Applications.Count() == 0)
                {
                    ApplicationBase app = new ApplicationBase()
                    {
                        AppName = "Lunenburg County Public Schools",
                        LDAPDomain = "lcps",
                        LDAPDomainFQN = "lcps.local",
                        LDAPPassword = "#",
                        LDAPUserName = "#",
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
                throw new Exception("Could not create default entities", ex);
            }
        }
    }
}