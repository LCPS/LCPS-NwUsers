using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPS.v2015.v001.NwUsers.HumanResources
{
    public interface IPerson
    {
        string FirstName { get; set; }

        string MiddleInitial { get; set; }

        string LastName { get; set; }

        string SortName { get; }

        string FullName { get; }

        HRGenders Gender { get; set; }

        DateTime Birthdate { get; set; }
    }
}
