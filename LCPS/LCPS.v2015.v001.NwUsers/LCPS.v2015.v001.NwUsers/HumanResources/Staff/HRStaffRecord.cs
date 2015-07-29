using System;
using System.Collections;
using System.Collections.Generic;

using Anvil.v2015.v001.Domain.Entities.DynamicFilters;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

using System.Data.Linq;
using System.Linq;
using System.Linq.Dynamic;

using System.ComponentModel.DataAnnotations;

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{


    [MetadataType(typeof(HRStaffRecordMetaData))]
    partial class HRStaffRecord
    {

        public HRStaffRecord(HRStaff staff, HRStaffPosition position, HRBuilding building, HREmployeeType employeeType, HRJobTitle jobTitle)
        {
            if (staff != null)
            {
                FirstName = staff.FirstName;
                MiddleInitial = staff.MiddleInitial;
                LastName = staff.LastName;
                Gender = staff.Gender;
                Birthdate = staff.Birthdate;
                StaffKey = staff.StaffKey;
                StaffId = staff.StaffId;
            }

            if (position != null)
            {

                PositionKey = position.PositionKey;
                StaffKey = position.StaffMemberId;
                BuildingKey = position.BuildingKey;
                EmployeeTypeKey = position.EmployeeTypeKey;
                JobTitleKey = position.JobTitleKey;
                Status = position.Status;
                FiscalYear = position.FiscalYear;
            }
        }

        public string SortName
        {
            get { return LastName + ", " + FirstName; }
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public HRGenders Gender
        {
            get { return (HRGenders)this.GenderVal; }
            set { this.GenderVal = Convert.ToInt32(value); }
        }

        public HRStaffPositionQualifier Status
        {
            get { 
                return (HRStaffPositionQualifier)this.StatusVal; }
            set { this.StatusVal = Convert.ToInt32(value); }
        }


        public static HRStaffRecord Get(Guid building, Guid employeeType, Guid jobTitle)
        {
            LcpsDbContext db = new LcpsDbContext();

            HRStaffRecord item = new HRStaffRecord(null,
                                                   null,
                                                   db.Buildings.FirstOrDefault(x => x.BuildingKey.Equals(building)),
                                                   db.EmployeeTypes.FirstOrDefault(x => x.EmployeeTypeLinkId.Equals(employeeType)),
                                                   db.JobTitles.FirstOrDefault(x => x.JobTitleKey.Equals(jobTitle)));

            return item;


        }

        public static List<HRStaffRecord> GetList(DynamicQueryStatement statement)
        {
            return GetList(statement.Query, statement.Parms);
        }

        public static List<HRStaffRecord> GetList(string query, object[] parms)
        {
            if (parms == null)
                return new List<HRStaffRecord>();

            if (parms.Count() == 0)
                return new List<HRStaffRecord>();

            try
            {
                HRStaffContext db = new HRStaffContext(Properties.Settings.Default.ConnectionString);

                List<HRStaffRecord> items = db.HRStaffRecords.Where(query, parms)
                    .OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial)
                    .ToList();

                return items;
            }
            catch (Exception ex)
            {
                Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionCollector ec = new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionCollector("Error getting staff members");
                ec.Add(query);
                ec.Add(ex);
                throw ec.ToException();

            }
        }



    }

    public class HRStaffRecordMetaData
    {
        [Display(Name = "Name")]
        public string SortName { get; set; }

        [Display(Name = "Building")]
        public string BuildingName { get; set; }


        [Display(Name = "Type")]
        public string EmployeeTypeName { get; set; }

        [Display(Name = "Title")]
        public string JobTitleName { get; set; }

        [Display(Name = "Year")]
        public string FiscalYear { get; set; }

    }
}