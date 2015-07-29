using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCPS.v2015.v001.NwUsers.Importing2;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using Anvil.v2015.v001.Domain.Exceptions;
using Anvil.v2015.v001.Domain.Entities;

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public class HRStaffImportRecord : IImportFileRecord
    {

        #region Fields

        private HRStaffPositionDefinition _staffDefinition;

        #endregion

        #region Import Properties

        public String StaffId { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string BirthdateLabel { get; set; }

        public DateTime Birthdate
        {
            get { return Convert.ToDateTime(this.BirthdateLabel); }
        }

        public string GenderLabel { get; set; }

        public HRGenders Gender
        {
            get { return (HRGenders)System.Enum.Parse(typeof(HRGenders), this.GenderLabel); }
        }

        public string BuildingId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string JobTitleId { get; set; }

        public string StatusLabel { get; set; }

        public HRStaffPositionQualifier Status
        {
            get { return (HRStaffPositionQualifier)System.Enum.Parse(typeof(HRStaffPositionQualifier), this.StatusLabel); }
        }

        public string FiscalYear { get; set; }


        #endregion

        public int Index { get; set; }

        public string Content { get; set; }

        public string[] Fields { get; set; }

        public ImportRecordStatus ValidationStatus { get; set; }

        public string ValidationReport { get; set; }

        public ImportRecordStatus ImportStatus { get; set; }

        public string ImportReport { get; set; }

        public ImportCrudStatus CrudStatus { get; set; }

        public void Validate(Infrastructure.LcpsDbContext context)
        {
            // ---------- Validate Position Info

            try
            {
                _staffDefinition = new HRStaffPositionDefinition(this.BuildingId, this.EmployeeTypeId, this.JobTitleId, context);
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                ValidationReport = ec.ToLineBreakString();
                ValidationStatus = ImportRecordStatus.danger;
                return;
            }

            // ---------- Valdiate Birthdate
            try
            {
                DateTime d = this.Birthdate;
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(new Exception("Invalid bithdate", ex));
                ValidationReport = ec.ToLineBreakString();
                ValidationStatus = ImportRecordStatus.danger;
                return;
            }


            // ---------- Valdiate Status Qualifier
            try
            {
                HRStaffPositionQualifier q = this.Status;
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(new Exception("Invalid status", ex));
                ValidationReport = ec.ToLineBreakString();
                ValidationStatus = ImportRecordStatus.danger;
                return;
            }

            // ---------- Validate gender
            try
            {
                HRGenders g = this.Gender;
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(new Exception("Invalid gender", ex));
                ValidationReport = ec.ToLineBreakString();
                ValidationStatus = ImportRecordStatus.danger;
                return;
            }

            // --------- Validate Required fields
            try
            {
                AnvilEntity e = new AnvilEntity(this);
                e.RequiredFields.Add("StaffId");
                e.RequiredFields.Add("FirstName");
                e.RequiredFields.Add("LastName");

                e.Validate();
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                ValidationReport = ec.ToLineBreakString();
                ValidationStatus = ImportRecordStatus.danger;
                return;

            }

            ValidationStatus = ImportRecordStatus.success;
            ValidationReport = "Validated";
        }

        public void GetCrudStatus(Infrastructure.LcpsDbContext context)
        {
            CrudStatus = ImportCrudStatus.None;

            // ---------- Validate Staff
            HRStaff staff = context.StaffMembers.FirstOrDefault(x => x.StaffId.ToLower() == this.StaffId.ToLower());
            if (staff == null)
                CrudStatus = ImportCrudStatus.InsertMember | ImportCrudStatus.CreateMembership;
            else
            {
                if(staff.LastName == "Stanislas")
                {
                    int z = 0;
                    z++;
                }
                AnvilEntity sm = new AnvilEntity(this);
                sm.CompareFields.Add("StaffId");
                sm.CompareFields.Add("FirstName");
                sm.CompareFields.Add("MiddleInitial");
                sm.CompareFields.Add("LastName");
                sm.CompareFields.Add("Birthdate");
                sm.CompareFields.Add("Gender");
                sm.Compare(staff);

                if (sm.Compare(staff).Count() > 0)
                    CrudStatus = CrudStatus | ImportCrudStatus.UpdateMember;

            }


            if (staff != null)
            {

                // ------- Vadlidate Position
                HRStaffPosition position = context.StaffPositions
                    .FirstOrDefault(x =>
                        x.StaffMemberId.Equals(staff.StaffKey)
                        & x.BuildingKey.Equals(_staffDefinition.Building.BuildingKey)
                        & x.EmployeeTypeKey.Equals(_staffDefinition.EmployeeType.EmployeeTypeLinkId)
                        & x.JobTitleKey.Equals(_staffDefinition.JobTitle.JobTitleKey));

                if (position == null)
                    CrudStatus = CrudStatus | ImportCrudStatus.CreateMembership;
                else
                {
                    AnvilEntity pm = new AnvilEntity(this);
                    pm.CompareFields.Add("FiscalYear");
                    pm.CompareFields.Add("Status");
                    if (pm.Compare(position).Count() > 0)
                        CrudStatus = CrudStatus | ImportCrudStatus.UpdateMembership;
                }
            }



        }

        public void Import(Infrastructure.LcpsDbContext context)
        {
            if (CrudStatus == ImportCrudStatus.None)
                return;

            Guid _staffKey = Guid.Empty;

            HRStaffPosition[] pp = context.StaffPositions.Where(x => x.StaffMemberId.Equals(_staffKey)).ToArray();
            if(pp.Count() > 0)
            {
                context.StaffPositions.RemoveRange(pp);
                context.SaveChanges();
            }
            

            try
            {

                if (CrudStatus.HasFlag(ImportCrudStatus.InsertMember))
                {
                    HRStaff staff = new HRStaff()
                    {
                        StaffKey = Guid.NewGuid(),
                        StaffId = this.StaffId,
                        Birthdate = this.Birthdate,
                        FirstName = this.FirstName,
                        MiddleInitial = this.MiddleInitial,
                        LastName = this.LastName,
                        Gender = this.Gender
                    };

                    _staffKey = staff.StaffKey;

                    context.StaffMembers.Add(staff);
                }

                if (CrudStatus.HasFlag(ImportCrudStatus.UpdateMember))
                {
                    HRStaff staff = context.StaffMembers.First(x => x.StaffId.ToLower() == this.StaffId.ToLower());
                    staff.Birthdate = this.Birthdate;
                    staff.FirstName = this.FirstName;
                    staff.MiddleInitial = this.MiddleInitial;
                    staff.LastName = this.LastName;
                    staff.Gender = this.Gender;

                    _staffKey = staff.StaffKey;

                    context.Entry(staff).State = System.Data.Entity.EntityState.Modified;
                }

                if (CrudStatus.HasFlag(ImportCrudStatus.CreateMembership))
                {
                    if (_staffKey.Equals(Guid.Empty))
                        _staffKey = context.StaffMembers.First(x => x.StaffId.ToLower() == this.StaffId.ToLower()).StaffKey;

                    HRStaffPosition p = new HRStaffPosition()
                    {
                        StaffMemberId = _staffKey,
                        PositionKey = Guid.NewGuid(),
                        BuildingKey = _staffDefinition.Building.BuildingKey,
                        EmployeeTypeKey = _staffDefinition.EmployeeType.EmployeeTypeLinkId,
                        JobTitleKey = _staffDefinition.JobTitle.JobTitleKey,
                        Status = this.Status,
                        FiscalYear = this.FiscalYear
                    };

                    context.StaffPositions.Add(p);
                }

                if (CrudStatus.HasFlag(ImportCrudStatus.UpdateMembership))
                {
                    if (_staffKey.Equals(Guid.Empty))
                        _staffKey = context.StaffMembers.First(x => x.StaffId.ToLower() == this.StaffId.ToLower()).StaffKey;

                    HRStaffPosition p = context.StaffPositions
                        .First(x => x.StaffMemberId.Equals(_staffKey)
                        & x.BuildingKey.Equals(_staffDefinition.Building.BuildingKey)
                        & x.EmployeeTypeKey.Equals(_staffDefinition.EmployeeType.EmployeeTypeLinkId)
                        & x.JobTitleKey.Equals(_staffDefinition.JobTitle.JobTitleKey));

                    p.Status = this.Status;
                    p.FiscalYear = this.FiscalYear;

                    context.Entry(p).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();

                ImportStatus = ImportRecordStatus.success;
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                ImportStatus = ImportRecordStatus.danger;
                ImportReport = ec.ToLineBreakString();
            }
        }


    }
}
