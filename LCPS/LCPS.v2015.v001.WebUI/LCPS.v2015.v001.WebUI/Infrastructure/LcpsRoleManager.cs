using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity.EntityFramework;

using Anvil.v2015.v001.Domain.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Infrastructure
{
    public class LcpsRoleManager : ApplicationRoleManager
    {
        public LcpsRoleManager()
            : base(new RoleStore<Anvil.v2015.v001.Domain.Entities.ApplicationRole>(new LcpsDbContext()))
        {

        }
    }
}