using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Anvil.v2015.v001.Domain.Entities;
using Microsoft.Owin.Security.DataProtection;
using Anvil.v2015.v001.Domain.Infrastructure;
using Anvil.v2015.v001.Domain.Services;

namespace LCPS.v2015.v001.WebUI.Infrastructure
{
    public class LcpsUserManager : ApplicationUserManager
    {
        public LcpsUserManager()
            : base(new UserStore<Anvil.v2015.v001.Domain.Entities.ApplicationUser>(new LcpsDbContext()))
        {
            LcpsDbContext db = new LcpsDbContext();

            if (db.Applications.Count() == 0)
            {
                PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true
                };
            }
            else
            {
                ApplicationBase app = db.Applications.ToList()[0];
                
                // Configure validation logic for passwords
                PasswordValidator = new PasswordValidator
                {
                    RequiredLength = app.PWDRequiredLength,
                    RequireNonLetterOrDigit = app.PWDRequireNonLetterOrDigit,
                    RequireDigit = app.PWDRequireDigit,
                    RequireLowercase = app.PWDRequireLowercase,
                    RequireUppercase = app.PWDRequireUppercase
                };

                // Configure user lockout defaults
                UserLockoutEnabledByDefault = app.PWDUserLockoutEnabledByDefault;
                DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(app.PWDDefaultAccountLockoutTimeSpan);
                MaxFailedAccessAttemptsBeforeLockout = app.PWDMaxFailedAccessAttemptsBeforeLockout;
            }
        }
    }
}