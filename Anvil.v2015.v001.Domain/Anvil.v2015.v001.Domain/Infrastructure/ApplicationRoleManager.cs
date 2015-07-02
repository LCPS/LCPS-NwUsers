#region Using

using System.Data.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Anvil.v2015.v001.Domain.Entities;

#endregion

namespace Anvil.v2015.v001.Domain.Infrastructure
{
    public class ApplicationRoleManager: RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(RoleStore<ApplicationRole> roleStore)
            :base(roleStore)
        { }
        
    }
}
