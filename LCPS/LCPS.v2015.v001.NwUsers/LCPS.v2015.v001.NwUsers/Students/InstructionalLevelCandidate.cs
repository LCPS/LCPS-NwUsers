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

using Anvil.v2015.v001.Domain.Entities;

#endregion

namespace LCPS.v2015.v001.NwUsers.Students
{
    [Serializable]
    public class InstructionalLevelCandidate : IInstructionalLevel, IImportStatus, IImportEntity
    {

        #region Fields

        byte[] _serialized;

        [NonSerialized]
        LcpsDbContext db = new LcpsDbContext();

        #endregion

        #region Instructional Level

        public Guid InstructionalLevelKey { get; set; }

        public string InstructionalLevelId { get; set; }

        public string InstructionalLevelName { get; set; }

        #endregion

        #region Import Status
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
            InstructionalLevelCandidate t = (InstructionalLevelCandidate)ImportFileTSV.DeserializeItem(this.GetType(), _serialized);
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

        public InstructionalLevel ToInstructionalLevel()
        {
            InstructionalLevel et = new InstructionalLevel();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(et);
            return et;
        }

        public void Record()
        {
            ImportItem i = this.ToImportItem();
            i.Description = ToInstructionalLevel().ToString();
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

        public bool TargetExists()
        {
            int count = DbContext.InstructionalLevels.Where(x => x.InstructionalLevelId == this.InstructionalLevelId).Count();
            return (count > 0);
        }

        public void Create()
        {
            InstructionalLevel i = ToInstructionalLevel();
            i.InstructionalLevelKey = Guid.NewGuid();
            DbContext.InstructionalLevels.Add(i);
            DbContext.SaveChanges();
        }

        public void Update()
        {
            InstructionalLevel i = ToInstructionalLevel();
            InstructionalLevel t = DbContext.InstructionalLevels.First(x => x.InstructionalLevelId == this.InstructionalLevelId);
            AnvilEntity e = new AnvilEntity(i);
            e.CopyTo(t);

            DbContext.Entry(t).State = System.Data.Entity.EntityState.Modified;
            DbContext.SaveChanges();
        }

        public bool IsSyncJustified()
        {
            InstructionalLevel i = ToInstructionalLevel();
            InstructionalLevel t = DbContext.InstructionalLevels.First(x => x.InstructionalLevelId == this.InstructionalLevelId);
            AnvilEntity e = new AnvilEntity(i);
            return (e.Compare(t).Count() > 0);
        }
    }
}
