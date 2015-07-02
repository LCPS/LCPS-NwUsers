using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace LCPS.v2015.v001.WebUI.Database
{
    public class LcpsDbContext : LCPS.v2015.v001.NwUsers.Infrastructure.LcpsDbContext
    {
        public LcpsDbContext()
            :base(ConfigurationManager.ConnectionStrings["LcpsDbContext"].ConnectionString)
        {

        }

        public static LcpsDbContext Create()
        {
            return new LcpsDbContext();
        }

    }
}