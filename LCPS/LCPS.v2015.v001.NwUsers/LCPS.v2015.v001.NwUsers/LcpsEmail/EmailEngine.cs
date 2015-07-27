using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Net;
using System.Net.Mail;

using Anvil.v2015.v001.Domain.Entities;
using LCPS.v2015.v001.NwUsers.Infrastructure;


namespace LCPS.v2015.v001.NwUsers.LcpsEmail
{
    public class EmailEngine
    {
        #region Fields

        

        #endregion

        #region Email

        public static void SendEmail(Guid toUserId, string subject, string body)
        {
            LcpsDbContext _dbContext = new LcpsDbContext();
            ApplicationBase app = LcpsDbContext.DefaultApp;

            ApplicationUser u = _dbContext.Users.Find(toUserId);

            NetworkCredential c = new NetworkCredential(app.SMTPUserName, app.SMTPPassword);

            string from = LcpsDbContext.DefaultApp.SMTPUserName;
            string to = u.Email;
            SendEmail(to, from, subject, body, c);
        }

        public static void SendEmail(string to, string subject, string body)
        {
            ApplicationBase app = LcpsDbContext.DefaultApp;

            NetworkCredential c = new NetworkCredential(app.SMTPUserName, app.SMTPPassword);

            string from = LcpsDbContext.DefaultApp.SMTPUserName;
            SendEmail(to, from, subject, body, c);
        }



        public static void SendEmail(string from, string to, string subject, string body, NetworkCredential credential)
        {
            try
            {
                MailMessage m = new MailMessage(from, to, subject, body);
                m.IsBodyHtml = true;

                ApplicationBase app = LcpsDbContext.DefaultApp;

                SmtpClient c = new SmtpClient(app.SMTPServer, app.SMPTPPort);
                c.Credentials = credential;

                c.EnableSsl = app.SMTPEnableSSL;

                c.Send(m);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not send email", ex);
            }
        }

        #endregion

    }
}
