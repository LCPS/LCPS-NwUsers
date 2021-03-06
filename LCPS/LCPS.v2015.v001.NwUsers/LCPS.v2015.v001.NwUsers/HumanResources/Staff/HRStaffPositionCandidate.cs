﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCPS.v2015.v001.NwUsers.Importing;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Exceptions;

using LCPS.v2015.v001.NwUsers.Infrastructure;


namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    [Serializable]
    public class HRStaffPositionCandidate : IImportStatus, IImportEntity
    {

        #region Fields

        byte[] _serialized;

        [NonSerialized]
        LcpsDbContext db = new LcpsDbContext();

        #endregion

        #region Item Properties

        public string StaffId { get; set; }
        
        public string BuildingId { get; set; }
        
        public string EmployeeTypeId { get; set; }
        
        public string JobTitleId { get; set; }
        
        public HRStaffPositionQualifier Status { get; set; }
        
        public string FiscalYear { get; set;}

        #endregion

        #region Import Item

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
            HRStaffPositionCandidate t = (HRStaffPositionCandidate)ImportFileTSV.DeserializeItem(this.GetType(), _serialized);
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

            HRStaff _staff = db.StaffMembers.FirstOrDefault(x => x.StaffId == StaffId);

            HRBuilding _building = db.Buildings.FirstOrDefault(x => x.BuildingId == BuildingId);

            HREmployeeType _employeeType = db.EmployeeTypes.FirstOrDefault(x => x.EmployeeTypeId == EmployeeTypeId);

            HRJobTitle _jobTitle = db.JobTitles.FirstOrDefault(x => x.JobTitleId == JobTitleId);

            if (_staff == null)
                throw new Exception(string.Format("{0} is an invalid staff Id", StaffId));

            if (_building == null)
                throw new Exception(string.Format("{0} is an invalid building Id", BuildingId));

            if (_employeeType == null)
                throw new Exception(String.Format("{0} is an invalid employee type Id", EmployeeTypeId));

            if (_jobTitle == null)
                throw new Exception(string.Format("{0} is an invalid job title Id", JobTitleId));

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


        public HRStaffPosition ToStaffPosition()
        {
            LcpsDbContext db = new LcpsDbContext();
            HRStaffPosition p = HRStaffPosition.Load(StaffId, BuildingId, EmployeeTypeId, JobTitleId, db);
            p.Status = Status;
            p.FiscalYear = FiscalYear;
            return p;
        }
            

        public override string ToString()
        {
            return StaffId + " - " + BuildingId + " - " + EmployeeTypeId + " - " + JobTitleId + " Active: " + Status.ToString() + " - " + FiscalYear;
        }

        #endregion


        

        public bool TargetExists()
        {
            LcpsDbContext db = new LcpsDbContext();

            HRStaffPosition p = HRStaffPosition.Load(StaffId, BuildingId, EmployeeTypeId, JobTitleId, db);

            int count = db.StaffPositions.Where(x => x.StaffMemberId.Equals(p.StaffMemberId) &
                x.BuildingKey.Equals(p.BuildingKey) &
                x.EmployeeTypeKey.Equals(p.EmployeeTypeKey) &
                x.JobTitleKey.Equals(p.JobTitleKey)).Count();

            return (count > 0);
        }

        public void Create()
        {
            try
            {
                LcpsDbContext db = new LcpsDbContext();

                HRStaffPosition p = HRStaffPosition.Load(StaffId, BuildingId, EmployeeTypeId, JobTitleId, db);
                p.PositionKey = Guid.NewGuid();
                p.Status = this.Status;
                p.FiscalYear = this.FiscalYear;

                db.StaffPositions.Add(p);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception("Could not create position", ex);
            }
        }

        public void Update()
        {
            try
            {
                LcpsDbContext db = new LcpsDbContext();

                HRStaffPosition p = HRStaffPosition.Load(StaffId, BuildingId, EmployeeTypeId, JobTitleId, db);

                HRStaffPosition tmp = db.StaffPositions.First(x => x.StaffMemberId.Equals(p.StaffMemberId) &
                    x.BuildingKey.Equals(p.BuildingKey) &
                    x.EmployeeTypeKey.Equals(p.EmployeeTypeKey) &
                    x.JobTitleKey.Equals(p.JobTitleKey));

                tmp.Status = this.Status;
                tmp.FiscalYear = this.FiscalYear;

                db.Entry(tmp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update position", ex);
            }
        }

        public bool IsSyncJustified()
        {
            try
            {
                LcpsDbContext db = new LcpsDbContext();

                HRStaffPosition p = HRStaffPosition.Load(StaffId, BuildingId, EmployeeTypeId, JobTitleId, db);

                HRStaffPosition tmp = db.StaffPositions.First(x => x.StaffMemberId.Equals(p.StaffMemberId) &
                    x.BuildingKey.Equals(p.BuildingKey) &
                    x.EmployeeTypeKey.Equals(p.EmployeeTypeKey) &
                    x.JobTitleKey.Equals(p.JobTitleKey));

                if (tmp.Status == this.Status & tmp.FiscalYear == this.FiscalYear)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not search fpr position in the database", ex);
            }
        }
    }
}
