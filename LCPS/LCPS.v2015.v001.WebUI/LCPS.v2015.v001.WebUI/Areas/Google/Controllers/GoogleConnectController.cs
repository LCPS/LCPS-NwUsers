using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DotNetOpenAuth.OAuth2;
 

using Google.Apis.Auth.OAuth2;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
//using Google.Apis.Samples.Helper;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;

using System.Security.Cryptography.X509Certificates;


using Microsoft.Owin.Security.Google;

namespace LCPS.v2015.v001.WebUI.Areas.Google.Controllers
{
    public class GoogleConnectController : Controller
    {

        private   IAuthenticator CreateAuthenticator()
    {
        var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
        provider.ClientIdentifier = <myClientId>;
        provider.ClientSecret = <myClientSecret>";
        return new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthentication);
    }
        // GET: Google/GoogleConnect
        public ActionResult Index()
        {
            string p = HttpContext.Server.MapPath("~/Assets/API-Project.p12");
            String serviceAccountEmail = "295890335281-0893v3u7vrecvd3auaacof0kh5uro9ba@developer.gserviceaccount.com";
            X509Certificate2 certificate = new X509Certificate2(p, "notasecret", X509KeyStorageFlags.Exportable);
            ServiceAccountCredential credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = new[]
                    {
                        DirectoryService.Scope.AdminDirectoryUser
                    },
                User = "matthew.early@k12lcps.org"
            }.FromCertificate(certificate));

            var service = new DirectoryService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "LCPS-MSE-01",
            });



            //service.Users.List().Domain = "k12lcps.org";
            var list = service.Users.List();
            var results = list.Execute();

            
            foreach (User u in results.UsersValue)
            {
                string n = u.Name.FullName;
                int x = 0;
                x++;
            }
            

            return View();


        }

       
        
    }
}