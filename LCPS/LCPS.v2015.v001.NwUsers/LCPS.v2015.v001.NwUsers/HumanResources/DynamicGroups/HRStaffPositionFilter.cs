#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Infrastructure;

#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.DynamicGroups
{
    public class HRStaffPositionFilter
    {
        LcpsDbContext db = new LcpsDbContext();

        public List<object> QueryParms = new List<object>();
        public List<string> Elements = new List<string>();

        public bool FilterBuilding { get; set; }
        public Guid BuildingValue { get; set; }

        public bool FilterEmployeeType { get; set; }
        public Guid EmployeeTypeValue { get; set; }

        public bool FilterJobTitle { get; set; }
        public Guid JobTitleValue { get; set; }

        public bool FilterStatus { get; set; }
        public HRStaffPositionQualifier StatusValue { get; set; }

        public bool FilterName { get; set; }
        public string NameValue{ get; set; }

        public LcpsDbContext DbContext
        {
            get
            {
                if (db == null)
                    db = new LcpsDbContext();
                return db;
            }

        }


        public List<HRStaffPosition> GetStaff()
        {

            List<HRStaffPosition> positions = new List<HRStaffPosition>();

            if (FilterBuilding |
                FilterEmployeeType |
                FilterJobTitle |
                FilterStatus)
            {

                if (FilterBuilding)
                    BuildElement("BuildingKey", BuildingValue);

                if (FilterEmployeeType)
                    BuildElement("EmployeeTypeKey", EmployeeTypeValue);

                if (FilterJobTitle)
                    BuildElement("JobTitleKey", JobTitleValue);

                if (FilterStatus)
                    BuildElement("Status", StatusValue);

                
                string q = string.Join(" AND ", Elements.ToArray());

                positions = DbContext.StaffPositions.Where(q, QueryParms.ToArray()).ToList();

                positions = positions.OrderBy(x => x.StaffMember.LastName + x.StaffMember.FirstName + x.StaffMember.MiddleInitial).ToList();
            }
            else
            {
                positions = DbContext.StaffPositions.ToList().OrderBy(x => x.StaffMember.LastName + x.StaffMember.FirstName + x.StaffMember.MiddleInitial).ToList();
            }

            if (FilterName)
            {
                Elements.Add("StaffMember.FullName.Contains@(" + QueryParms.Count().ToString() + ")");
                QueryParms.Add(NameValue);
                positions = positions.Where(x => x.StaffMember.FullName.ToLower().Contains(NameValue.ToLower())).ToList();
            }

            return positions;

        }

        private void BuildElement(string field, object value)
        {
            Elements.Add(field + "= @" + QueryParms.Count().ToString());
            QueryParms.Add(value);
        }


        public static bool GetDoField(string fieldName)
        {
            HRStaffPositionFilter f = new HRStaffPositionFilter();
            PropertyInfo p = f.GetType().GetProperty(fieldName + "Value");
            bool v = (bool)p.GetValue(f, null);
            return v;
        }

    }
}
