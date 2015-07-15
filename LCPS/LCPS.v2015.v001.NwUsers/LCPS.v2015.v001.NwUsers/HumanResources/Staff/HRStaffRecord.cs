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

using PagedList;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;

#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public class HRStaffRecord : IPerson, IStaff, IStaffPosition
    {

        public HRStaffRecord(HRStaff staff, HRStaffPosition position, HRBuilding building, HREmployeeType employeeType, HRJobTitle jobTitle)
        {
            FirstName = staff.FirstName;
            MiddleInitial = staff.MiddleInitial;
            LastName = staff.LastName;
            Gender = staff.Gender;
            Birthdate = staff.Birthdate;
            StaffKey = staff.StaffKey;
            StaffId = staff.StaffId;
            StaffEmail = staff.StaffEmail;
            PositionKey = position.PositionKey;
            StaffMemberId = position.StaffMemberId;
            BuildingKey = position.BuildingKey;
            EmployeeTypeKey = position.EmployeeTypeKey;
            JobTitleKey = position.JobTitleKey;
            Status = position.Status;
            FiscalYear = position.FiscalYear;
            Building = building;
            EmployeeType = employeeType;
            JobTitle = jobTitle;
        }

        #region Person

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string SortName
        {
            get { return LastName + ", " + FirstName; }
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public HRGenders Gender { get; set; }

        public DateTime Birthdate { get; set; }

        #endregion


        #region Staff

        public Guid StaffKey { get; set; }

        public string StaffId { get; set; }

        public string StaffEmail { get; set; }

        #endregion


        #region Position

        public HRBuilding Building { get; set; }

        public HREmployeeType EmployeeType { get; set; }

        public HRJobTitle JobTitle { get; set; }

        public Guid PositionKey { get; set; }

        public Guid StaffMemberId { get; set; }

        public Guid BuildingKey { get; set; }

        public Guid EmployeeTypeKey { get; set; }

        public Guid JobTitleKey { get; set; }

        public HRStaffPositionQualifier Status { get; set; }

        public string FiscalYear { get; set; }

        #endregion

        public static IPagedList<HRStaffRecord> GetPagedList(DynamicQueryStatement statement, int pageNumber, int pageSize)
        {
            List<HRStaffRecord> items = GetList(statement);
            return items.ToPagedList(pageNumber, pageSize);
        }

        public static List<HRStaffRecord> GetList(DynamicQueryStatement statement)
        {
            return GetList(statement.Query, statement.Parms);
        }

        public static List<HRStaffRecord> GetList(string query, object[] parms)
        {
            try
            { 
                LcpsDbContext db = new LcpsDbContext();

                List<HRStaffRecord> items = (from HRStaff staff in db.StaffMembers
                                             join HRStaffPosition pos in db.StaffPositions
                                                on staff.StaffKey equals pos.StaffMemberId
                                             join HRBuilding b in db.Buildings 
                                                on pos.BuildingKey equals b.BuildingKey 
                                             join HREmployeeType et in db.EmployeeTypes 
                                                on pos.EmployeeTypeKey equals et.EmployeeTypeLinkId
                                             join HRJobTitle jt in db.JobTitles 
                                                on pos.JobTitleKey equals jt.JobTitleKey
                                             select new HRStaffRecord(staff, pos, b, et, jt))
                                             .Where(query, parms)
                                             .OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial)
                                             .ToList();
                return items;
                             
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get a list of staff records", ex);
            }
        }


    }
}
