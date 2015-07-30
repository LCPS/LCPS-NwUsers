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
        Unknown = 0,
        Domain = 1,
        OrganizationalUnit = 2,
        Group = 4,
        User = 8,
        Container = 16
    }
}
