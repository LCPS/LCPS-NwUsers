#region Using

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Microsoft.AspNet.Identity;
using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Infrastructure;

#endregion

namespace Anvil.v2015.v001.Domain.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(string connectionString)
            : base(connectionString, throwIfV1Schema: false)
        {
            
        }

        

        public System.Data.Entity.DbSet<ApplicationRole> Roles { get; set; }
        public System.Data.Entity.DbSet<ApplicationBase> Applications { get; set; }

        public System.Data.Entity.DbSet<MvcControllerACL> MvcControllerACLs { get; set; }
    }
}
