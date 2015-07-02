#region Using

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Anvil.v2015.v001.Domain.Entities;
using Microsoft.Owin.Security.DataProtection;
using Anvil.v2015.v001.Domain.Infrastructure;
using Anvil.v2015.v001.Domain.Services;
#endregion

namespace Anvil.v2015.v001.Domain.Infrastructure
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(UserStore<ApplicationUser> userStore)
            :base(userStore)
        {

            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            EmailService = new EmailService();
            

            SmsService = new SmsService();

            var provider = new DpapiDataProtectionProvider("BlackLamp");
            UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, string>(provider.Create("UserToken"))
                as IUserTokenProvider<ApplicationUser, string>;

            IdentityFactoryOptions<ApplicationUserManager> options = new IdentityFactoryOptions<ApplicationUserManager>();

            var dataProtectionProvider = options.DataProtectionProvider; // options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }

        
        public static ApplicationUserManager Create(UserStore<ApplicationUser> userStore)
        {
            return new ApplicationUserManager(userStore);
        }

        public IdentityResult GetValidatePasswordResult(string password)
        {
            Task<IdentityResult> r = AsyncIsPasswordValid(password);
            return r.Result;
        }

        private async Task<IdentityResult> AsyncIsPasswordValid(string password)
        {
            IdentityResult r = await this.PasswordValidator.ValidateAsync(password);
            return r;
        }


    }

}
