using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

using System.Web.Mvc;

using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using Anvil.v2015.v001.Domain.Exceptions;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

namespace LCPS.v2015.v001.NwUsers.Filters
{
    public class DynamicStaffClause : DynamicQueryClause
    {
        #region Constants

        public const string fBuilding = "BuildingKey";
        public const string fEmployeeType = "EmployeeTypeKey";
        public const string fJobTitle = "JobTitleKey";
        public const string fStatus = "StatusVal";
        public const string fStaffId = "StaffId";
        public const string fLastName = "LastName";
        public const string fFiscalYear= "FiscalYear";


        #endregion

        #region Fields

        private LcpsDbContext _dbContext;
        private HRStaffContext _hrContext;

        #endregion

        #region Constructors

        public DynamicStaffClause()
        { }

        public DynamicStaffClause(Guid filterId)
        {
            this.FilterId = filterId;
        }

        public DynamicStaffClause(StaffFilterClause c)
        {
            FromStaffClause(c);
        }

        #endregion


        #region Properties

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();

                return _dbContext;
            }
        }

        public HRStaffContext StaffContext
        {
            get
            {
                if (_hrContext == null)
                    _hrContext = new HRStaffContext();
                return _hrContext;
            }
        }

        #endregion

        #region Defaults

        public static StaffFilterClause GetDefault(Guid filterId)
        {
            StaffFilterClause c = new StaffFilterClause();
            c.FilterId = filterId;
            c.ClauseConjunction = DynamicQueryConjunctions.Or;
            c.BuildingConjunction = DynamicQueryConjunctions.And;
            c.EmployeeTypeConjunction = DynamicQueryConjunctions.And;
            c.JobTitleConjunction = DynamicQueryConjunctions.And;
            c.StatusConjunction = DynamicQueryConjunctions.And;
            c.StaffIdConjunction = DynamicQueryConjunctions.And;
            c.LastNameConjunction = DynamicQueryConjunctions.And;
            c.FiscalYearConjunction = DynamicQueryConjunctions.And;

            return c;
        }

        public static StaffFilterClause GetDefaultSearch()
        {
            StaffFilterClause c = new StaffFilterClause();
            c.ClauseConjunction = DynamicQueryConjunctions.Or;
            c.StatusConjunction = DynamicQueryConjunctions.And;
            c.StatusInclude = true;
            c.StatusOperator = DynamicQueryOperators.Equals;
            c.StatusValue = HRStaffPositionQualifier.Active;
            return c;
            
        }

        #endregion


        #region Conversions

        public void FromStaffClause(StaffFilterClause c)
        {
            FilterId = c.FilterId;
            ClauseConjunction = c.ClauseConjunction;
            ClauseId = c.StaffFilterClauseId;


            if (c.BuildingInclude)
                Add(c.BuildingInclude, c.BuildingConjunction, fBuilding, c.BuildingOperator, c.BuildingValue);

            if (c.EmployeeTypeInclude)
                Add(c.EmployeeTypeInclude, c.EmployeeTypeConjunction, fEmployeeType, c.EmployeeTypeOperator, c.EmployeeTypeValue);

            if (c.JobTitleInclude)
                Add(c.JobTitleInclude, c.JobTitleConjunction, fJobTitle, c.JobTitleOperator, c.JobTitleValue);

            if (c.StatusInclude)
                Add(c.StatusInclude, c.StatusConjunction, fStatus, c.StatusOperator, Convert.ToInt32(c.StatusValue));

            if (c.LastNameInclude)
                Add(c.LastNameInclude, c.LastNameConjunction, fLastName, c.LastNameOperator, c.LastNameValue);

            if (c.StaffIdInclude)
                Add(c.StaffIdInclude, c.StaffIdConjunction, fStaffId, c.StaffIdOperator, c.StaffIdValue);

            if (c.FiscalYearInclude)
                Add(c.FiscalYearInclude, c.FiscalYearConjunction, fFiscalYear, c.FiscalYearOperator, c.FiscalYearValue);
        }


        public override string ToString()
        {
            return ToFriendlyString();
        }

        public override string ToFriendlyString()
        {
            string q = "(";

            foreach (DynamicQueryClauseField f in this)
            {
                if (f.Include)
                {
                    if (this.IndexOf(f) > 0)
                        q += f.Conjunction.ToString() + " ";

                    string v = f.Value.ToString();
                    string fn = f.FieldName;

                    if (f.FieldName == fBuilding)
                    {
                        fn = "Building";
                        v = DbContext.Buildings.Find(f.Value).Name;
                    }

                    if(f.FieldName == fEmployeeType)
                    {
                        fn = "Employee Type";
                        v = DbContext.EmployeeTypes.Find(f.Value).EmployeeTypeName;
                    }

                    if(f.FieldName == fJobTitle)
                    {
                        fn = "Job Title";
                        v = DbContext.JobTitles.Find(f.Value).JobTitleName;
                    }

                    if(f.FieldName == "StatusVal")
                    {
                        fn = "Status";
                        v = ((HRStaffPositionQualifier)f.Value).ToString();
                    }

                    if (f.Operator == DynamicQueryOperators.Contains)
                        q += "[" + fn + "] Contains \"" + v + "\" ";
                    else
                        q += "[" + fn + "] " + OperatorLibrary.GetOperator(f.Operator) + " \"" + v + "\" ";
                }
            }

            return q + ")";

        }

        #endregion

        #region Database

        public List<HRStaffRecord> Execute()
        {
            DynamicQueryStatement dqs = ToDynamicQueryStatement();

            try
            {
                List<HRStaffRecord> ss = this.StaffContext.HRStaffRecords
                    .Where(dqs.Query, dqs.Parms)
                    .OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial)
                    .ToList();

                return ss;

            }
            catch(Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector("Could not get students from database");
                ec.Add(ex);
                ec.Add(dqs.Query);
                throw ec.ToException();
            }
        }

        #endregion

        #region Lists

        public static List<SelectListItem> GetBuildings()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {
                LcpsDbContext db = new LcpsDbContext();
                List<HRBuilding> bb = db.Buildings.OrderBy(x => x.Name).ToList();

                SelectListItem[] ii = (from HRBuilding x in bb
                                       select new SelectListItem()
                                       {
                                           Text = x.Name,
                                           Value = x.BuildingKey.ToString()
                                       }).ToArray();

                items.AddRange(ii);
            }
            catch
            {
                items.Add(new SelectListItem() { Text = "Error", Value = Guid.Empty.ToString() });
            }

            return items;
        }

        public static List<HREmployeeType> GetEmployeeTypes(Guid? buildingId, LcpsDbContext db)
        {
            try
            {
                List<HREmployeeType> ii = new List<HREmployeeType>();
                if (buildingId == null)
                    ii = db.EmployeeTypes.Distinct().OrderBy(x => x.EmployeeTypeName).ToList();
                else
                    ii = (from HREmployeeType et in db.EmployeeTypes
                          join HRStaffPosition s in db.StaffPositions on et.EmployeeTypeLinkId equals s.EmployeeTypeKey
                          where s.BuildingKey.Equals(buildingId.Value)
                          select et).Distinct().OrderBy(x => x.EmployeeTypeName).ToList();

                return ii;
            }
            catch(Exception ex)
            {
                throw new Exception("could not get employeeTypes from database", ex);
            }
        }

        public static List<HRJobTitle> GetJobTitles(Guid? buildingId, Guid? employeeTypeId, LcpsDbContext db)
        {
            try
            {
                List<HRJobTitle> jtt = new List<HRJobTitle>();

                if (buildingId == null & employeeTypeId == null)
                    jtt = db.JobTitles.Distinct().OrderBy(x => x.JobTitleName).ToList();

                if (buildingId != null & employeeTypeId == null)
                    jtt = (from HRJobTitle jt in db.JobTitles
                           join HRStaffPosition s in db.StaffPositions on jt.JobTitleKey equals s.JobTitleKey
                           where s.BuildingKey.Equals(buildingId.Value)
                           select jt).Distinct().OrderBy(x => x.JobTitleName).ToList();

                if (buildingId == null & employeeTypeId != null)
                    jtt = (from HRJobTitle jt in db.JobTitles
                           join HRStaffPosition s in db.StaffPositions on jt.JobTitleKey equals s.JobTitleKey
                           where s.EmployeeTypeKey.Equals(employeeTypeId.Value)
                           select jt).Distinct().OrderBy(x => x.JobTitleName).ToList();

                if (buildingId != null & employeeTypeId != null)
                    jtt = (from HRJobTitle jt in db.JobTitles
                           join HRStaffPosition s in db.StaffPositions on jt.JobTitleKey equals s.JobTitleKey
                           where s.EmployeeTypeKey.Equals(employeeTypeId.Value) &
                             s.BuildingKey.Equals(buildingId.Value)
                           select jt).Distinct().OrderBy(x => x.JobTitleName).ToList();

                return jtt;
            }
            catch(Exception ex)
            {
                throw new Exception("Could not get job titles form database", ex);
            }
        }

        #endregion


    }
}
