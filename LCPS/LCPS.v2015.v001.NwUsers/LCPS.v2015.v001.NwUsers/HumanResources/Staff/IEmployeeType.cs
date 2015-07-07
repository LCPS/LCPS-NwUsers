#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Mvc;

#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{

    public interface IEmployeeType
    {
        Guid EmployeeTypeLinkId { get; set; }

        string EmployeeTypeId { get; set; }

        string EmployeeTypeName { get; set; }
    }
}
