using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    [Flags]
    public enum LcpsAdsObjectTypes
    {
        Domain = 0,
        OrganizationalUnit = 1,
        Group = 2,
        User = 4
    }
}
