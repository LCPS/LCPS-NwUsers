using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources;

using Anvil.v2015.v001.Domain.Entities;


namespace LCPS.v2015.v001.NwUsers.Students
{
    [Serializable]
    public class StudentCandidate : IPerson, IStudent, IImportStatus, IImportEntity
    {
        #region Fields

        [NonSerialized]
        LcpsDbContext db = new LcpsDbContext();

        byte[] _serialized;

        #endregion

        #region Person

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string SortName
        {
            get { return LastName + ", " + FirstName ; }
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public HRGenders Gender { get; set; }

        public DateTime Birthdate { get; set; }

        #endregion

        #region Student

        public Guid StudentKey { get; set; }

        public string StudentId { get; set; }

        public Guid InstructionalLevelKey { get; set; }

        public string InstructionalLevelId { get; set; }

        public Guid BuildingKey { get; set; }

        public string BuildingId { get; set; }

        public StudentEnrollmentStatus Status { get; set; }

        public string SchoolYear { get; set; }

        #endregion

        #region Import  Status

        public Guid ImportItemId { get; set; }

        public Guid SessionId { get; set; }

        public string Comment { get; set; }

        public string Description { get; set; }

        public DateTime EntryDate { get; set; }

        public ImportResultStatus ImportStatus { get; set; }

        public byte[] SerializedData
        {
            get { return _serialized; }
            set { _serialized = value; }
        }

        public object SourceItem
        {
            get
            {
                return Deserialize();
            }
            set
            {
                _serialized = ImportFileTSV.SerializeItem(value);
            }
        }

        public void Serialize()
        {
            _serialized = ImportFileTSV.SerializeItem(this);
        }

        public object Deserialize()
        {
            StudentCandidate t = (StudentCandidate)ImportFileTSV.DeserializeItem(this.GetType(), _serialized);
            return t;
        }

        public object Deserialize(IImportSession session)
        {
            return Deserialize();
        }

        public ImportEntityStatus EntityStatus { get; set; }

        public int LineIndex { get; set; }

        public void Validate()
        {
            HRBuilding b = DbContext.Buildings.FirstOrDefault(x => x.BuildingId == BuildingId);
            if (b == null)
                throw new Exception("Building ID is invalid");
            else
                BuildingKey = b.BuildingKey;

            InstructionalLevel l = DbContext.InstructionalLevels.FirstOrDefault(x => x.InstructionalLevelId == this.InstructionalLevelId);
            if (l == null)
                throw new Exception("Instructional level is invalid");
            else
                InstructionalLevelKey = l.InstructionalLevelKey;


            ImportFileTSV.ValidateItem(this);
        }

        public Student ToStudent()
        {
            Student et = new Student();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(et);
            return et;
        }

        public void Record()
        {
            ImportItem i = this.ToImportItem();
            i.Description = ToStudent().ToString();
            ImportFileTSV.Record(i, db);
        }

        public ImportItem ToImportItem()
        {
            ImportItem i = new ImportItem();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(i);
            return i;
        }

        public LcpsDbContext DbContext
        {
            get
            {
                if (db == null)
                    db = new LcpsDbContext();
                return db;
            }
        }


        #endregion

        #region Import Entity

        public bool TargetExists()
        {
            int count = DbContext.Students.Where(x => x.StudentId == this.StudentId).Count();
            return (count > 0);
        }

        public void Create()
        {
            Student s = ToStudent();
            s.StudentKey = Guid.NewGuid();
            DbContext.Students.Add(s);
            db.SaveChanges();
        }

        public void Update()
        {
            Student s = ToStudent();
            Student t = DbContext.Students.First(x => x.StudentId == this.StudentId);
            AnvilEntity e = new AnvilEntity(s);
            e.CopyTo(t);
            DbContext.Entry(t).State = System.Data.Entity.EntityState.Modified;
            DbContext.SaveChanges();
        }

        public bool IsSyncJustified()
        {
            Student s = ToStudent();
            Student t = DbContext.Students.First(x => x.StudentId == this.StudentId);
            AnvilEntity e = new AnvilEntity(s);
            return (e.Compare(t)).Count() > 0;
        }

        #endregion
    }
}
