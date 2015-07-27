using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

using Anvil.v2015.v001.Domain.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Infrastructure
{
    public class LcpsMvcControllerACLManager : MvcControllerACLManager
    {
        public LcpsMvcControllerACLManager()
            :base(new LcpsDbContext())
        {

        }

        public static bool GetUserCanAccess(IPrincipal user, string area)
        {
            LcpsMvcControllerACLManager m = new LcpsMvcControllerACLManager();
            return m.UserCanAccess(user, area);
        }

    }
}