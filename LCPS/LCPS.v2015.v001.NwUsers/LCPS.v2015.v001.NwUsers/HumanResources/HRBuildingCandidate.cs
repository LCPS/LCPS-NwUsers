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
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.Infrastructure;

using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Exceptions;

#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources
{
    [Serializable]
    public class HRBuildingCandidate : IBuilding, IImportStatus, IImportEntity
    {
        #region Fields

        [NonSerialized]
        public LcpsDbContext db = new LcpsDbContext();
        private byte[] _serialized;

        #endregion

        // -- --------------------- Building

        #region Building Properties

        public Guid BuildingKey { get; set; }

        public string BuildingId { get; set; }

        public string Name { get; set; }

        #endregion

        // -- --------------------- Candidate

        #region Candidate Properties and Methods

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
            get { return Deserialize(); }
            set { _serialized = ImportFileTSV.SerializeItem(value);  }
        }

        public void Serialize()
        {
            _serialized = ImportFileTSV.SerializeItem(this); 
        }

        public object Deserialize()
        {
            HRBuildingCandidate t = (HRBuildingCandidate)ImportFileTSV.DeserializeItem(this.GetType(), _serialized);
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
            i.Description = ToString();
            ImportFileTSV.Record(i, db);
        }

        public ImportItem ToImportItem()
        {
            ImportItem i = new ImportItem();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(i);
            return i;
        }

        public HRBuilding ToBuilding()
        {
            HRBuilding b = new HRBuilding();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(b);
            return b;
        }

        public override string ToString()
        {
            return BuildingId + " - " + Name;
        }

        #endregion

        #region Entity Properties and Methods

        public bool TargetExists()
        {
            try
            {
                HRBuilding b = db.Buildings.FirstOrDefault(x => x.BuildingKey == this.BuildingKey);
                return (b != null);
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching for building in database", ex);
            }
        }

        public void Create()
        {
            try
            {
                HRBuilding b = ToBuilding();
                LcpsDbContext db = new LcpsDbContext();
                b.BuildingKey = Guid.NewGuid();
                db.Buildings.Add(b);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add builing", ex);
            }
        }

        public void Update()
        {
            try
            {
                HRBuilding b = db.Buildings.First(x => x.BuildingKey.Equals(this.BuildingKey));
                AnvilEntity e = new AnvilEntity(this);
                e.CopyTo(b);
                db.Entry(b).State = System.Data.Entity.EntityState.Modified;
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
            HRBuilding b = db.Buildings.First(x => x.BuildingId == this.BuildingId);
            return (e.Compare(b).Count() > 0);
        }

        #endregion
    }
}
