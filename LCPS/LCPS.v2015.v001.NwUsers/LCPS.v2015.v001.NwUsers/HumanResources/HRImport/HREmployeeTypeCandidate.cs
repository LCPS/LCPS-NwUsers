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
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

using Anvil.v2015.v001.Domain.Entities;


#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.HRImport
{
    [Serializable]
    public class HREmployeeTypeCandidate : IEmployeeType, IImportStatus, IImportEntity
    {
        #region Fields

        [NonSerialized]
        private LcpsDbContext db = new LcpsDbContext();
        private byte[] _serialized;

        #endregion

        public HREmployeeTypeCandidate()
        {
            
        }


        public Guid EmployeeTypeLinkId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string EmployeeTypeName { get; set; }

        public HREmployeeTypeCategory Category { get; set; }

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
            HREmployeeTypeCandidate t = (HREmployeeTypeCandidate)ImportFileTSV.DeserializeItem(this.GetType(), _serialized);
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
            ImportFileTSV.ValidateItem(this);
        }

        public void Record()
        {
            ImportItem i = this.ToImportItem();
            i.Description = ToEmployeeType().ToString();
            ImportFileTSV.Record(i, db);
        }

        public ImportItem ToImportItem()
        {
            ImportItem i = new ImportItem();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(i);
            return i;
        }

        public bool TargetExists()
        {
            int count = db.EmployeeTypes.Where(x => x.EmployeeTypeId == this.EmployeeTypeId).Count();
            return (count > 0);
        }

        public void Create()
        {
            if (db == null)
                db = new LcpsDbContext();

            HREmployeeType i = this.ToEmployeeType();
            i.EmployeeTypeLinkId = Guid.NewGuid();

            db.EmployeeTypes.Add(i);
            db.SaveChanges();
        }

        public void Update()
        {
            HREmployeeType et = ToEmployeeType();
            db.Entry(et).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public bool IsSyncJustified()
        {
            AnvilEntity e = new AnvilEntity(this);
            HREmployeeType jt = db.EmployeeTypes.First(x => x.EmployeeTypeId == this.EmployeeTypeId);
            return (e.Compare(jt).Count() > 0);
        }

        public HREmployeeType ToEmployeeType()
        {
            HREmployeeType et = new HREmployeeType();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(et);
            return et;
            
        }

       
    }
}
