using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.Importing2;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Students;
using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Exceptions;

namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapAccounts
{

    public class LdapAccountImportRecord : IImportFileRecord
    {
        Guid _userKey = Guid.Empty;
        
        #region Import Properties

        public string EntityId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Active { get; set; }


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
            // ---------- Vadliate User key
            HRStaff staff = context.StaffMembers.FirstOrDefault(x => x.StaffId.ToLower() == this.EntityId.ToLower());
            if (staff == null)
            {
                Student stu = context.Students.FirstOrDefault(x => x.StudentId.ToLower() == this.EntityId.ToLower());
                if (stu == null)
                {
                    ValidationReport = string.Format("{0} is not a valida student or staff id", this.EntityId);
                    ValidationStatus = ImportRecordStatus.danger;
                    return;
                }
                else
                    _userKey = stu.StudentKey;

            }
            else _userKey = staff.StaffKey;

            // ---------- Validate Fields
            try
            {
                AnvilEntity e = new AnvilEntity(this);
                e.RequiredFields.Add("StaffId");
                e.RequiredFields.Add("UserName");
                e.RequiredFields.Add("Password");
                e.Validate();
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                ValidationReport = ec.ToLineBreakString();
                ValidationStatus = ImportRecordStatus.danger;
                return;
            }

            // ---------- Validate Password
            if(this.Password.Length < 6)
            {
                ValidationReport = "The password must contain at least 6 characters";
                ValidationStatus = ImportRecordStatus.danger;
                return;
            }

            ValidationReport = "Validated";
            ValidationStatus = ImportRecordStatus.success;
        }

        public void GetCrudStatus(Infrastructure.LcpsDbContext context)
        {
            CrudStatus = ImportCrudStatus.None;

            LdapAccount ac = context.LdapAccounts
                .FirstOrDefault(
                    x => x.UserKey.Equals(_userKey)
                    & x.UserName.ToLower() == this.UserName.ToLower()
                );

            if (ac == null)
                CrudStatus = ImportCrudStatus.InsertMember;
            else
            {
                AnvilEntity e = new AnvilEntity(this);
                e.CompareFields.Add("Active");
                if(e.Compare(ac).Count() > 0)
                    CrudStatus = ImportCrudStatus.UpdateMembership;
            }
                
        }

        public void Import(Infrastructure.LcpsDbContext context)
        {
            try
            {
                if (this.CrudStatus == ImportCrudStatus.InsertMember)
                {
                    LdapAccount ac = new LdapAccount()
                    {
                        AccountId = Guid.NewGuid(),
                        UserKey = _userKey,
                        UserName = this.UserName,
                        InitialPassword = this.Password,
                        Active = this.Active
                    };

                    context.LdapAccounts.Add(ac);
                }
                else
                {
                    LdapAccount ac = context.LdapAccounts
                    .FirstOrDefault(
                        x => x.UserKey.Equals(_userKey)
                        & x.UserName.ToLower() == this.UserName.ToLower()
                    );
                    ac.UserName = this.UserName;
                    ac.Active = this.Active;
                    context.Entry(ac).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();

                ImportReport = "Success";
                ImportStatus = ImportRecordStatus.success;

            }
            catch(Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                ImportReport = ec.ToLineBreakString();
                ImportStatus = ImportRecordStatus.danger;
            }
        }
    }
}
