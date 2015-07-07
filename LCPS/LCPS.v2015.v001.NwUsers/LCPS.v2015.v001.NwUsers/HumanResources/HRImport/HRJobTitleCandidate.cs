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

using Anvil.v2015.v001.Domain.Entities;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;


#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.HRImport
{
    [Serializable]
    public class HRJobTitleCandidate : IJobTitle, IImportStatus, IImportEntity
    {

        private byte[] _serialized;

        [NonSerialized]
        LcpsDbContext db = new LcpsDbContext();

        public Guid JobTitleKey { get; set; }

        public string EmployeeTypeId { get; set; }

        public Guid EmployeeTypeLinkId { get; set; }

        public string JobTitleId { get; set; }

        public string JobTitleName { get; set; }

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
            HRJobTitleCandidate t = (HRJobTitleCandidate)ImportFileTSV.DeserializeItem(this.GetType(), _serialized);
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
            HREmployeeType et = db.EmployeeTypes.FirstOrDefault(x => x.EmployeeTypeId == this.EmployeeTypeId);
            if (et == null)
                throw new Exception(string.Format("{0} is not a valid employee type", this.EmployeeTypeId));

            ImportFileTSV.ValidateItem(this);
        }

        public void Record()
        {
            ImportItem i = this.ToImportItem();
            i.Description = ToString();
            ImportFileTSV.Record(i, db);
        }

        public override string ToString()
        {
            return EmployeeTypeId + " - " + JobTitleId + " - " + JobTitleName;
        }

        public ImportItem ToImportItem()
        {
            ImportItem i = new ImportItem();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(i);
            return i;
        }

        public HRJobTitle ToJobTitle()
        {
            HRJobTitle et = new HRJobTitle();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(et);
            return et;
        }

        public bool TargetExists()
        {
            try
            {
                HREmployeeType et = db.EmployeeTypes.FirstOrDefault(x => x.EmployeeTypeId == this.EmployeeTypeId);
                if (et == null)
                    throw new Exception(string.Format("{0} is not a valid employee type", this.EmployeeTypeId));

                HRJobTitle jt = db.JobTitles.FirstOrDefault(x => x.EmployeeTypeLinkId.Equals(et.EmployeeTypeLinkId) &
                    x.JobTitleId == this.JobTitleId);

                return (jt != null);
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching for job title in database", ex);
            }
        }


        public void Create()
        {
            try 
            {
                HRJobTitle jt = ToJobTitle();
                LcpsDbContext db = new LcpsDbContext();
                jt.JobTitleKey = Guid.NewGuid();
                jt.EmployeeTypeLinkId = db.EmployeeTypes.First(x => x.EmployeeTypeId == this.EmployeeTypeId).EmployeeTypeLinkId;
                db.JobTitles.Add(jt);
                db.SaveChanges();
            } 
            catch(Exception ex) 
            {
                throw new Exception("Could not add job title", ex);
            }

        }

        public void Update()
        {
            try
            {
                HRJobTitle jt = db.JobTitles.First(x => x.JobTitleKey.Equals(this.JobTitleKey));
                AnvilEntity e = new AnvilEntity(this);
                e.CopyTo(jt);
                db.Entry(jt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update job title", ex);
            }
        }

        public bool IsSyncJustified()
        {
            AnvilEntity e = new AnvilEntity(this);
            HRJobTitle jt = db.JobTitles.First(x => x.JobTitleId == this.JobTitleId);
            return (e.Compare(jt).Count() > 0);
        }


        public string EmployeeTypeName
        {
            get { throw new NotImplementedException(); }
        }
    }
}
