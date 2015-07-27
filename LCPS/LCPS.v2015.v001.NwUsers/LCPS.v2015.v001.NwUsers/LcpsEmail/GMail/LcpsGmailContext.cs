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
        static string[] Scopes = { DirectoryService.Scope.AdminDirectoryUserReadonly };
        static string ApplicationName = "Directory API Quickstart";

        public void Validate()
        {
            Assembly _assembly = typeof(LCPS.v2015.v001.NwUsers.Infrastructure.LcpsDbContext).Assembly;
            string[] names = _assembly.GetManifestResourceNames();
            string json = "LCPS.v2015.v001.NwUsers.LcpsEmail.Assets.API-Project.p12";
            StreamReader reader = new StreamReader(_assembly.GetManifestResourceStream(json));

            var bytes = default(byte[]);
            using (var memstream = new MemoryStream())
            {
                var buffer = new byte[512];
                var bytesRead = default(int);
                while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                    memstream.Write(buffer, 0, bytesRead);
                bytes = memstream.ToArray();
            }
            


            const string serviceAccountEmail = "295890335281-0893v3u7vrecvd3auaacof0kh5uro9ba@developer.gserviceaccount.com";
            //const string serviceAccountCertPath = @"E:\Test.p12";
            const string serviceAccountCertPassword = "notasecret";
            const string userEmail = "matthew.early@k12lcps.org";

            

            var certificate = new X509Certificate2(bytes, 
                serviceAccountCertPassword, 
                X509KeyStorageFlags.Exportable);
            ServiceAccountCredential credential = new ServiceAccountCredential(

           new ServiceAccountCredential.Initializer(serviceAccountEmail)

           {
               Scopes = new[] { DirectoryService.Scope.AdminDirectoryUser },

               User = userEmail


           }.FromCertificate(certificate));

            
            var service = new DirectoryService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "User Provisioning",
            });


            UsersResource.ListRequest request = service.Users.List();
            //request.Customer = "my_customer";
            request.Domain = "mail.k12lcps.org";
            request.MaxResults = 10;
            request.OrderBy = UsersResource.ListRequest.OrderByEnum.Email;

            // List users.
            IList<User> users = request.Execute().UsersValue;
            Console.WriteLine("Users:");
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
