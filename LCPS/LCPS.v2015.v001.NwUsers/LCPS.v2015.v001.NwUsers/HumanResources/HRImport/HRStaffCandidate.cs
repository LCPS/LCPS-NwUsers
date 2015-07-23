using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Runtime.Serialization.Formatters.Binary;

using Anvil.v2015.v001.Domain.Exceptions;
using Anvil.v2015.v001.Domain.Entities;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

namespace LCPS.v2015.v001.NwUsers.HumanResources.HRImport
{
    [Serializable]
    public class HRStaffCandidate : IStaff, IPerson, IImportStatus, IImportEntity
    {

        #region Fields

        private byte[] _serialized;

        [NonSerialized]
        LcpsDbContext db = new LcpsDbContext();

        #endregion

        #region Staff

        public Guid StaffKey { get; set; }

        public string StaffId { get; set; }

        #endregion

        #region Person

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string SortName
        {
            get { return LastName + ", " + FirstName; }
        }

        [Display(Name = "Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public HRGenders Gender { get; set; }

        public DateTime Birthdate { get; set; }

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
            get { return Deserialize(); }
            set { _serialized = ImportFileTSV.SerializeItem(value); }
        }

        public void Serialize()
        {
            _serialized = ImportFileTSV.SerializeItem(this);
        }

        public object Deserialize()
        {
            HRStaffCandidate t = (HRStaffCandidate)ImportFileTSV.DeserializeItem(this.GetType(), _serialized);
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

        public HRStaff ToStaff()
        {
            HRStaff b = new HRStaff();
            AnvilEntity e = new AnvilEntity(this);
            e.CopyTo(b);
            return b;
        }

        public override string ToString()
        {
            return ToStaff().ToString();
        }

        #endregion

        #region Entity

        public bool TargetExists()
        {
            try
            {
                HRStaff i = db.StaffMembers.FirstOrDefault(x => x.StaffId == this.StaffId);
                return (i != null);
            }
            catch (Exception ex)
            { 
                throw new Exception("Could not search for staff member", ex);
            }
        }

        public void Create()
        {
            try
            {
                if (db == null)
                    db = new LcpsDbContext();

                HRStaff i = ToStaff();
                i.StaffKey = Guid.NewGuid();
                db.StaffMembers.Add(i);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add staff member", ex);
            }
        }

        public void Update()
        {
            try
            {
                HRStaff i = ToStaff();
                db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update staff member", ex);
            }
        }

        public bool IsSyncJustified()
        {
            AnvilEntity e = new AnvilEntity(this);
            HRStaff b = db.StaffMembers.First(x => x.StaffKey == this.StaffKey);
            return (e.Compare(b).Count() > 0);
        }

        #endregion
    }
}
