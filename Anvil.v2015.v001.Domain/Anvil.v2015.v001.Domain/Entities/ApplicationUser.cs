#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Anvil.v2015.v001.Domain.Infrastructure;
using System.Net.Mail;
using System.Web;

#endregion

namespace Anvil.v2015.v001.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        #region Enums

        public enum PrivacyQualifier
        {
            Personal = 0,
            Community = 1,
            Neighborhood = 2,
            Global = 4
        }

        public enum AccountStatusQualifier
        {
            Active = 0,
            PendingEmailConfirmation = 1,
            PendingAccountInfo = 2,
            Suspended = 4,
            Locked = 8
        }

        #endregion

        #region Properties

        [Display(Name = "Account Status", Description = "The status of the account, 'Active' = The account is functioning account; 'Pendnging Email Confirmation' = The account will be funtional as soon as the author confirms the email address; 'PendingInfo' = The account will be functional as soon as the author completes the application wizard; 'Suspended' = The publishers have suspended this account. The Bio nor work will be visible by anyone accept the authors; 'Locked Out' = The account has been accesses with the wrong password too many times")]
        public AccountStatusQualifier AccountStatus { get; set; }

        [Display(Name = "Account Visibility", Description = "Determines who will be able to view your account")]
        public PrivacyQualifier AccountVisibility { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(128)]
        public string GivenName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(128)]
        public string SurName { get; set; }

        public string FullName
        {
            get { return GivenName + " " + SurName; }
        }

        public string SortName
        {
            get { return SurName + ", " + GivenName; }
        }

        [Display(Name = "Email Visibility", Description = "Determines who can see your email address")]
        public PrivacyQualifier EmailVisibility { get; set; }

        [Display(Name = "Avatar")]
        public byte[] AvatarContent { get; set; }

        [Display(Name = "Pen Name")]
        [MaxLength(128)]
        public string Psuedonym { get; set; }

        [Display(Name = "Birthday", Description = "We need to know how old you are")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Bio")]
        [MaxLength(4096)]
        public string Autobiography { get; set; }

        [Display(Name = "Home Town")]
        [MaxLength(128)]
        public string HomeTownCity { get; set; }

        [Display(Name = "State")]
        [MaxLength(2)]
        [MinLength(2)]
        public string HomeTownState { get; set; }

        [Display(Name = "Company Id")]
        [MaxLength(25)]
        public string CompanyId { get; set; }

        [Display(Name = "Avatar")]
        public byte[] AvatarData { get; set; }




        #endregion



    }
}