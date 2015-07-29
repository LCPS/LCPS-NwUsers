using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.LcpsEmail;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapAccounts;
using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Models
{
    public class HRStaffDetailModel
    {
        public HRStaff StaffMember { get; set; }

        public IEnumerable<HRStaffPosition> StaffPositions { get; set; }

        public IEnumerable<EmailAccount> EmailAccounts { get; set; }

        public IEnumerable<LdapAccount> LdapAccounts { get; set; }

        public void Load(Guid staffKey)
        {
            try
            {
                LcpsDbContext db = new LcpsDbContext();
                StaffMember = db.StaffMembers.First(x => x.StaffKey.Equals(staffKey));
                StaffPositions = db.StaffPositions.Where(x => x.StaffMemberId.Equals(staffKey));
                EmailAccounts = db.EmailAccounts.Where(x => x.UserLink.Equals(staffKey));
                LdapAccounts = db.LdapAccounts.Where(x => x.UserKey.Equals(staffKey));
            }
            catch (Exception ex)
            {
                throw new Exception("could not load staff data", ex);
            }
        }
    }
}