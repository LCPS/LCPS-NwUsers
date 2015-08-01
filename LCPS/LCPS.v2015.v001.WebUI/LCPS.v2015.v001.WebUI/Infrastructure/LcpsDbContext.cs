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
using System.Data.Entity;

namespace LCPS.v2015.v001.WebUI.Infrastructure
{
    public class LcpsDbContext : LCPS.v2015.v001.NwUsers.Infrastructure.LcpsDbContext
    {
        #region Constants

        public const string UserAdminName = "lcps";
        public const string UserAdminEmail = "lcps@k12lcps.org";
        public const string UserAdminPassword = "Lcp$-pw1";

        #endregion

        public LcpsDbContext()
            : base(ConfigurationManager.ConnectionStrings["LcpsDbContext"].ConnectionString)
        {
            Database.SetInitializer<LcpsDbContext>(new LcpsDbInitializer());

            this.Configuration.ProxyCreationEnabled = false;

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
    }

}