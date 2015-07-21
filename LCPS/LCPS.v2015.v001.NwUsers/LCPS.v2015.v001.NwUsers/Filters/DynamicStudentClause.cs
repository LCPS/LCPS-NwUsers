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
    public class DynamicStudentClause : IDynamicFilterClause
    {
        #region Constants

        public const string fBuildingKey = "BuildingKey";
        public const string fInstructionalLevelKey = "InstructionalLevelKey";
        public const string fStatus = "Status";
        public const string fLastName = "LastName";
        public const string fStudentId = "Studentid";

        #endregion

        #region Fields

        private DynamicQueryOperatorLibrary lib = new DynamicQueryOperatorLibrary();
        private List<DynamicQueryClauseField> _fields = new List<DynamicQueryClauseField>();
        private List<object> _parms = new List<object>();
        private List<string> _elements = new List<string>();
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
            ClauseConjunction = c.ClauseConjunction;
            ClauseId = c.StudentFilterClauseId;

            if (c.BuildingInclude)
                AddElement(c.BuildingConjunction, fBuildingKey, c.BuildingOperator, c.BuildingValue);

            if (c.InstructionalLevelInclude)
                AddElement(c.InstructionalLevelConjunction, fInstructionalLevelKey, c.InstructionalLevelOperator, c.InstructionalLevelValue);

            if (c.StatusInclude)
                AddElement(c.StatusConjunction, fStatus, c.StatusOperator, c.StatusValue);

            if (c.StudentIdInclude)
                AddElement(c.NameConjunction, fLastName, c.NameOperator, c.NameValue);

            if (c.StudentIdInclude)
                AddElement(c.StudentIdConjunction, fStudentId, c.StudentIdOperator, c.StudentIdValue);
        }

        public void AddElement(DynamicQueryConjunctions conjunction, string fieldName, DynamicQueryOperators op, object value)
        {
            string element = "";

            if (Parms.Count > 0)
                element = conjunction.ToString();

            if (op == DynamicQueryOperators.Contains)
            {
                element += " " + fieldName + ".Contains(@" + Parms.Count().ToString() + ") ";
            }
            else
            {
                element += " " + fieldName + " " + lib.GetOperator(op) + " @" + Parms.Count().ToString();
            }

            Parms.Add(value);



            Elements.Add(element);

            Add(new DynamicQueryClauseField()
                {
                    Conjunction = conjunction,
                    FieldName = fieldName,
                    Operator = op,
                    Value = value
                });
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

        public DynamicQueryConjunctions ClauseConjunction { get; set; }

        public Guid FilterId { get; set; }

        public Guid ClauseId { get; set; }

        public List<object> Parms
        {
            get { return _parms; }
        }

        public List<string> Elements
        {
            get { return _elements; }
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

        #region List

        public int IndexOf(DynamicQueryClauseField item)
        {
            return _fields.IndexOf(item);
        }

        public void Insert(int index, DynamicQueryClauseField item)
        {
            _fields.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _fields.RemoveAt(index);
        }

        public DynamicQueryClauseField this[int index]
        {
            get { return _fields[index]; }
            set { _fields[index] = value; }
        }

        public void Add(DynamicQueryClauseField item)
        {
            _fields.Add(item);
        }

        public void Clear()
        {
            _fields.Clear();
        }

        public bool Contains(DynamicQueryClauseField item)
        {
            return _fields.Contains(item);
        }

        public void CopyTo(DynamicQueryClauseField[] array, int arrayIndex)
        {
            _fields.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _fields.Count; }
        }

        public bool IsReadOnly
        {
            get { return _fields.ToArray().IsReadOnly; }
        }

        public bool Remove(DynamicQueryClauseField item)
        {
            return _fields.Remove(item);
        }

        public IEnumerator<DynamicQueryClauseField> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        #endregion


        #region Conversions

        public DynamicQueryStatement ToDynamicQueryStatement()
        {
            int count = _fields.Where(x => x.Include = true).Count();

            if (count == 0)
                return null;

            DynamicQueryStatement dq = new DynamicQueryStatement()
            {
                Query = "(" + string.Join(" ", _elements.ToArray()) + ")",
                Parms = _parms.ToArray()
            };

            return dq;
        }


        public string ToFriendlyString()
        {
            string q = "(";

            if (ClauseConjunction != DynamicQueryConjunctions.None)
                q = ClauseConjunction.ToString() + " (";

            foreach (DynamicQueryClauseField f in this)
            {
                if (f.Include)
                {

                    string v = f.Value.ToString();

                    if (f.FieldName == fBuildingKey)
                    {
                        Guid bk = new Guid(f.Value.ToString());
                        v = DbContext.Buildings.First(x => x.BuildingKey.Equals(bk)).Name;
                    }

                    if (f.FieldName == fInstructionalLevelKey)
                    {
                        Guid ik = new Guid(f.Value.ToString());
                        v = DbContext.InstructionalLevels.First(x => x.InstructionalLevelKey.Equals(ik)).InstructionalLevelName;
                    }

                    if (IndexOf(f) > 0)
                        if (f.Conjunction != DynamicQueryConjunctions.None)
                            q += f.Conjunction.ToString() + " ";


                    if (f.Operator == DynamicQueryOperators.Contains)
                        q += "[" + f.FieldName + "] Contains \"" + v + "\" ";
                    else
                        q += "[" + f.FieldName + "] " + lib.GetOperator(f.Operator) + " \"" + v + " \" ";
                }
            }

            return q + ")";
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
