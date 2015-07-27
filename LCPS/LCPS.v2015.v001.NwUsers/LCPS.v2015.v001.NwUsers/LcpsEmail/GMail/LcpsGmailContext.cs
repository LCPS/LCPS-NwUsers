using Google.Apis.Auth.OAuth2;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace LCPS.v2015.v001.NwUsers.LcpsEmail.GMail
{
    public class LcpsGmailContext
    {
        static string[] scopes = { DirectoryService.Scope.AdminDirectoryUserReadonly };
        static string ApplicationName = "lcps-admin-dir";

        public void Validate()
        {
            const string serviceAccountEmail = "61410732382-0tiqcjvhv4c1bl0dandsus64odntdi8m@developer.gserviceaccount.com";
            string serviceAccountCertPath = System.Web.HttpContext.Current.Server.MapPath("~/Assets/Gmail/lcps-admin-dir.p12"); 
            const string serviceAccountCertPassword = "notasecret";
            const string userEmail = "matthew.early@k12lcps.org";

            var certificate = new X509Certificate2(serviceAccountCertPath, 
                serviceAccountCertPassword, 
                X509KeyStorageFlags.Exportable);
                ServiceAccountCredential credential = new ServiceAccountCredential(

           new ServiceAccountCredential.Initializer(serviceAccountEmail)
           {
               Scopes = scopes,
               User = userEmail

           }.FromCertificate(certificate));


            
            var service = new DirectoryService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });


            UsersResource.ListRequest request = service.Users.List();
            request.Customer = "my_customer";
            request.OrderBy = UsersResource.ListRequest.OrderByEnum.Email;

            // List users. UsersResource.ListRequest.Execute().UsersValue
            IList<User> users = request.Execute().UsersValue;
            
            if (users != null && users.Count > 0)
            {
                foreach (var userItem in users)
                {
                    string pe = userItem.PrimaryEmail;
                }
            }
            else
            {
                throw new Exception("No users");
            }
        }
    }
}
