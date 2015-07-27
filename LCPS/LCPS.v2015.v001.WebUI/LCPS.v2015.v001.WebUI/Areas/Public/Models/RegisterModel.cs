using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Web;
using LCPS.v2015.v001.NwUsers.LcpsEmail;
using System.Web;
using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Students;

using Anvil.v2015.v001.Domain.Entities;

namespace LCPS.v2015.v001.WebUI.Areas.Public.Models
{
    public class RegisterModel
    {

        public RegisterModel(string email, string password)
        {
            this.Email = email;
        }

        public string Email { get; set; }
        public string Password { get; set; }


        public void SendConfirmationEmail(HttpContext context)
        {
            try
            {

                ApplicationBase app = LcpsDbContext.DefaultApp;
                if (!this.Email.ToLower().EndsWith(app.SMTPDomain))
                    throw new Exception(string.Format("Only email accounts that are a part of the {0} domain are permissible", app.SMTPDomain));

                Guid key;
                string firstName;
                string lastName;
                string server = context.Request.ServerVariables["SERVER_NAME"];
                string port = context.Request.ServerVariables["SERVER_PORT"];
                string domain = server + ":" + port;
                string ip = context.Request.ServerVariables["REMOTE_HOST"];
                string d = DateTime.Now.ToString();
                string brwsr = context.Request.ServerVariables["HTTP_USER_AGENT"];
                
                LcpsEmailContext _email = new LcpsEmailContext();
                LcpsEmailKey k = _email.LcpsEmailKeys.FirstOrDefault(x => x.EmailAddress.ToLower() == this.Email.ToLower());
                if(k.StaffKey == null)
                {
                    key = k.StudentKey.Value;
                    firstName = k.StuFirstName;
                    lastName = k.StuLastName;
                }
                else
                {
                    key = k.StaffKey.Value;
                    firstName = k.StfFirstName;
                    lastName = k.StfLastName;
                }

                string bPath = context.Server.MapPath("~/Areas/Public/Views/Home/EmailConfirmationBody.html");
                string bTextExp = File.ReadAllText(bPath);
                string bText = string.Format(bTextExp, firstName, lastName, domain, key.ToString(), ip, d, brwsr);

                EmailEngine.SendEmail(this.Email, "LCPS IntRanet Account Confirmation", bText);
            }
            catch(Exception ex)
            {
                throw new Exception("Could not send confirmation email", ex);
            }
        }

    }
}