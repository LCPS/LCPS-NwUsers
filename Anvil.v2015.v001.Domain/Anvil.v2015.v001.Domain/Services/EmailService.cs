#region Using

using Anvil.v2015.v001.Domain.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

#endregion

namespace Anvil.v2015.v001.Domain.Services
{
    public class EmailService : IIdentityMessageService
    {

        public ApplicationBase AppBase { get; set; }


        public Task SendAsync(IdentityMessage message)
        {

            SmtpClient _client;

            if (AppBase == null)
                throw new Exception("The application's SMTP configuration has not been provided.");

            _client = new SmtpClient(AppBase.SMTPServer, AppBase.SMPTPPort);
            _client.EnableSsl = AppBase.SMTPEnableSSL;
            _client.Credentials = new System.Net.NetworkCredential(AppBase.SMTPUserName, AppBase.SMTPPassword);

            MailMessage msg = new MailMessage(AppBase.SMTPUserName, message.Destination, message.Subject, message.Body);
            return _client.SendMailAsync(msg);

        }
    }

}
