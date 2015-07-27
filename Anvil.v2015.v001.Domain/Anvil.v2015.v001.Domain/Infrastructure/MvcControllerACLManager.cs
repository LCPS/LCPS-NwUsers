using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Reflection;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security;
using System.Security.Principal;

using Microsoft.AspNet.Identity;

using Anvil.v2015.v001.Domain.Infrastructure;
using Anvil.v2015.v001.Domain.Entities;

using System.Web.Mvc;

namespace Anvil.v2015.v001.Domain.Infrastructure
{
    public class MvcControllerACLManager
    {
        #region Enum

        [Flags]
        public enum AccessLevel
        {
            None = 0,
            Area = 1,
            Controller = 2,
            Action = 4
        }

        #endregion

        #region Fields

        private ApplicationDbContext _context;

        #endregion

        #region Constructors

        public MvcControllerACLManager(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region ACL

        public bool ACLExists(MvcControllerACL acl)
        {
            try
            {
                int count = _context.MvcControllerACLs
                                .Where(x => x.Area == acl.Area
                                & x.Controller == acl.Controller
                                & x.Action == acl.Action
                                & x.RoleId == acl.RoleId).Count();

                return (count > 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching for ACL in database", ex);
            }
        }

        public void AddAccess(string roleName, string area)
        {
            AddAccess(roleName, area, null, null);
        }


        public void AddAccess(string roleName, string area, string controller)
        {
            AddAccess(roleName, area, controller, null);
        }

        public void AddAccess(string roleName, string area, string controller, string action)
        {

            ApplicationRole r = _context.Roles.FirstOrDefault(x => x.Name.ToLower() == roleName.ToLower());

            if (r != null)
            {
                MvcControllerACL acl = new MvcControllerACL()
                {
                    RecordId = Guid.NewGuid(),
                    Area = area,
                    Controller = controller,
                    Action = action,
                    RoleId = r.Id
                };

                if (!ACLExists(acl))
                {
                    _context.MvcControllerACLs.Add(acl);
                    _context.SaveChanges();
                }
            }
        }

        #endregion

        #region Get Roles

        public ApplicationRoleCollection GetRoles(string area)
        {
            List<ApplicationRole> rr = (from MvcControllerACL acl in _context.MvcControllerACLs
                                        join ApplicationRole rl in _context.Roles on acl.RoleId
                                            equals rl.Id
                                        where acl.Area == area
                                        select rl)
                                        .ToList();

            ApplicationRoleCollection c = new ApplicationRoleCollection();

            c.AddRange(rr);

            return c;
        }

        public ApplicationRoleCollection GetRoles(string area, string controller)
        {
            List<ApplicationRole> rr = (from MvcControllerACL acl in _context.MvcControllerACLs
                                        join ApplicationRole rl in _context.Roles on acl.RoleId
                                            equals rl.Id
                                        where acl.Area == area & acl.Controller == controller
                                        select rl)
                                        .ToList();

            ApplicationRoleCollection c = new ApplicationRoleCollection();

            c.AddRange(rr);

            return c;
        }

        public ApplicationRoleCollection GetRoles(string area, string controller, string action)
        {
            List<ApplicationRole> rr = (from MvcControllerACL acl in _context.MvcControllerACLs
                                        join ApplicationRole rl in _context.Roles on acl.RoleId
                                            equals rl.Id
                                        where acl.Area == area & acl.Controller == controller & acl.Action == action
                                        select rl)
                                        .ToList();

            ApplicationRoleCollection c = new ApplicationRoleCollection();

            c.AddRange(rr);

            return c;
        }



        #endregion

        #region User Access

        public bool UserCanAccess(IPrincipal user, string area, string controller, string action)
        {
            if (action == null)
                return UserCanAccess(user, area, controller);

            if (controller == null)
                return UserCanAccess(user, area);



            ApplicationRoleCollection arAcl = GetRoles(area);
            ApplicationRoleCollection cnAcl = GetRoles(area, controller);
            ApplicationRoleCollection acAcl = GetRoles(area, controller, action);

            if (acAcl.Count == 0)
            {
                if (cnAcl.Count == 0)
                    return IsUserInRoles(arAcl, user);
                else
                    return IsUserInRoles(cnAcl, user);
            }
            else
                return IsUserInRoles(acAcl, user);

        }

        public bool UserCanAccess(IPrincipal user, string area, string controller)
        {
            if (controller == null)
                return UserCanAccess(user, area);

            ApplicationRoleCollection cAcl = GetRoles(area, controller);
            ApplicationRoleCollection aAcl = GetRoles(area);

            if (cAcl.Count == 0)
                return IsUserInRoles(aAcl, user);
            else
                return IsUserInRoles(cAcl, user);
        }

        public bool UserCanAccess(IPrincipal user, string area)
        {

            if (area == null)
                throw new Exception("Area cannot be null when searching action lists");

            ApplicationRoleCollection rc = GetRoles(area);
            return IsUserInRoles(rc, user);
        }

        private bool IsUserInRoles(ApplicationRoleCollection rc, IPrincipal user)
        {
            if (rc.Count() == 0)
                return true;

            if (!user.Identity.IsAuthenticated)
                return false;

            foreach (ApplicationRole r in rc)
            {
                if (user.IsInRole(r.Name))
                    return true;
            }

            return false;
        }

        private string GetNameSpace(string nameSpace)
        {
            string[] ns = nameSpace.Split('.');
            List<string> ss = new List<string>(ns);
            return ss.Last();
        }

        #endregion
    }
}
