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
using LCPS.v2015.v001.NwUsers.Students;
using LCPS.v2015.v001.NwUsers.HumanResources;


namespace LCPS.v2015.v001.NwUsers.Filters
{
    public class DynamicStudentClause : DynamicQueryClause
    {
        #region Constants

        public const string fBuildingKey = "BuildingKey";
        public const string fInstructionalLevelKey = "InstructionalLevelKey";
        public const string fStatus = "StatusVal";
        public const string fLastName = "LastName";
        public const string fStudentId = "Studentid";

        #endregion

        #region Fields

        private LcpsDbContext _dbContext = new LcpsDbContext();

        #endregion

        #region Constructors

        public DynamicStudentClause(Guid filterId)
        {
            this.FilterId = filterId;
        }

        public DynamicStudentClause(StudentFilterClause c)
        {
            FromStudentClause(c);
        }

        #endregion

        #region Dynamic Fields

        public void FromStudentClause(StudentFilterClause c)
        {
            base.ClauseConjunction = c.ClauseConjunction;
            base.ClauseId = c.StudentFilterClauseId;
            base.FilterId = c.FilterId;

            if (c.BuildingInclude)
                AddElement(c.BuildingConjunction, fBuildingKey, c.BuildingOperator, c.BuildingValue);

            if (c.InstructionalLevelInclude)
                AddElement(c.InstructionalLevelConjunction, fInstructionalLevelKey, c.InstructionalLevelOperator, c.InstructionalLevelValue);

            if (c.StatusInclude)
                AddElement(c.StatusConjunction, fStatus, c.StatusOperator, Convert.ToInt32( c.StatusValue));

            if (c.NameInclude)
                AddElement(c.NameConjunction, fLastName, c.NameOperator, c.NameValue);

            if (c.StudentIdInclude)
                AddElement(c.StudentIdConjunction, fStudentId, c.StudentIdOperator, c.StudentIdValue);
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

        #endregion

        #region Defaults

        public static StudentFilterClause GetDefaultStudentClause(Guid filterId)
        {
            StudentFilterClause c = new StudentFilterClause();
            c.FilterId = filterId;
            c.ClauseConjunction = DynamicQueryConjunctions.Or;
            c.BuildingConjunction = DynamicQueryConjunctions.And;
            c.InstructionalLevelConjunction = DynamicQueryConjunctions.And;
            c.StatusConjunction = DynamicQueryConjunctions.And;
            c.StudentIdConjunction = DynamicQueryConjunctions.And;
            c.NameConjunction = DynamicQueryConjunctions.And;

            return c;
        }

        #endregion

        #region Conversions

        

        public override string ToString()
        {
            return ToFriendlyString();
        }


        public override string ToFriendlyString()
        {
            string q = "(";

            //if (ClauseConjunction != DynamicQueryConjunctions.None)
            //    q = ClauseConjunction.ToString() + " (";

            foreach (DynamicQueryClauseField f in this)
            {
                if (f.Include)
                {
                    string fn = f.FieldName;
                    string v = f.Value.ToString();

                    if (f.FieldName == fBuildingKey)
                    {
                        Guid bk = new Guid(f.Value.ToString());
                        v = DbContext.Buildings.First(x => x.BuildingKey.Equals(bk)).Name;
                        fn = "Building";
                    }

                    if (f.FieldName == fInstructionalLevelKey)
                    {
                        Guid ik = new Guid(f.Value.ToString());
                        v = DbContext.InstructionalLevels.First(x => x.InstructionalLevelKey.Equals(ik)).InstructionalLevelName;
                        fn = "Grade";
                    }

                    if(f.FieldName == fStatus)
                    {
                        v = System.Enum.Parse(typeof(StudentEnrollmentStatus), f.Value.ToString()).ToString();
                        fn = "Status";
                    }

                    if (IndexOf(f) > 0)
                        q += f.Conjunction.ToString() + " ";


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

        public List<StudentRecord> Execute()
        {
            DynamicQueryStatement dqs = ToDynamicQueryStatement();

            try
            {
                StudentsContext context = new StudentsContext();
               

                List<StudentRecord> students = context.StudentRecords.Where(dqs.Query, dqs.Parms)
                    .OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial)
                    .ToList();

                return students;
            }
            catch (Exception ex)
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

        public static List<SelectListItem> GetInstructionalLevels(Guid? buildingId)
        {

            List<SelectListItem> items = new List<SelectListItem>();

            try
            {
                LcpsDbContext db = new LcpsDbContext();

                List<InstructionalLevel> ill = new List<InstructionalLevel>();

                if(buildingId == null)
                    ill = db.InstructionalLevels.OrderBy(x => x.InstructionalLevelName).ToList();
                else
                {
                    ill = (from InstructionalLevel x in db.InstructionalLevels 
                           join Student s in db.Students on x.InstructionalLevelKey equals s.InstructionalLevelKey
                           join HRBuilding b in db.Buildings on s.BuildingKey equals b.BuildingKey
                           where b.BuildingKey.Equals(buildingId.Value)
                           orderby x.InstructionalLevelName
                           select x).ToList();

                }

                items = (from InstructionalLevel x in ill
                         select new SelectListItem()
                         {
                             Text = x.InstructionalLevelName,
                             Value = x.InstructionalLevelKey.ToString()
                         }).ToList();

            }
            catch
            {
                items.Add(new SelectListItem() { Text = "Error", Value = Guid.Empty.ToString() });
            }

            return items;
        }

        #endregion

    }
}
