using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity.EntityFramework;

using Anvil.v2015.v001.Domain.Infrastructure;
using Anvil.v2015.v001.Domain.Entities;

namespace LCPS.v2015.v001.WebUI.Infrastructure
{
    public class LcpsRoleManager : ApplicationRoleManager
    {

        #region Constants

        public const string ApplicationAdminRole = "app-admin";
        public const string HrAdminRole = "hr-admin";
        public const string StudentAdminRole = "student-admin";
        public const string StudentEmailRole = "student-email";
        public const string StaffEmailRole = "staff-email";
        public const string StudentLanRole = "student-lan";
        public const string StaffLanRole = "staff-lan";
        public const string StudentPwdRole = "student-pwd";
        public const string StaffPwdRole = "staff-pwd";

        #endregion

        public LcpsRoleManager()
            : base(new RoleStore<ApplicationRole>(new LcpsDbContext()))
        {

        }
    }
}