#region Using

using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

#endregion

namespace Anvil.v2015.v001.Domain.Entities
{
    public class ApplicationRole : IdentityRole
    {
        #region Enums

        public enum BLRoleTypes
        {
            Security = 0,
            PoetGroup = 1
        }

        #endregion

        #region Fields

        static Dictionary<string, string> defaultRoles = new Dictionary<string, string>();

        #endregion

        #region Properties

        [Display(Name = "Description")]
        [MaxLength(256)]
        public string Description { get; set; }

        [Display(Name = "Role Type")]
        public BLRoleTypes RoleType { get; set; }

        #endregion

    }
}
