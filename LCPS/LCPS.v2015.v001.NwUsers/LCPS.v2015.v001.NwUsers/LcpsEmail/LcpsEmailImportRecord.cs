using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LCPS.v2015.v001.NwUsers.Importing2;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Students;

namespace LCPS.v2015.v001.NwUsers.LcpsEmail
{
    public class LcpsEmailImportRecord : IImportFileRecord
    {
        private Guid _userId = Guid.Empty;

        public string EntityId { get; set; }

        public string Classification { get; set; }

        public string EmailAddress { get; set; }

        public string InitialPassword { get; set; }

        public int Index { get; set; }

        public string Content { get; set; }

        public string[] Fields { get; set; }

        public ImportRecordStatus ValidationStatus { get; set; }

        public string ValidationReport { get; set; }

        public ImportRecordStatus ImportStatus { get; set; }

        public string ImportReport { get; set; }

        public ImportCrudStatus CrudStatus { get; set; }

        public void Validate(LcpsDbContext context)
        {
            // -- Validate the entity id as a student or staff id;


            Student stu = context.Students.FirstOrDefault(x => x.StudentId == EntityId);
            if (stu == null)
            {
                HRStaff stf = context.StaffMembers.FirstOrDefault(x => x.StaffId == EntityId);
                if (stf == null)
                {
                    ValidationStatus = ImportRecordStatus.danger;
                    ValidationReport = string.Format("ID {0} was not found in the database", EntityId);
                    return;
                }
                else
                    _userId = stf.StaffKey;
            }
            else
                _userId = stu.StudentKey;

            if (InitialPassword.Length < 8)
            {
                ValidationStatus = ImportRecordStatus.danger;
                ValidationReport = "The password must be at least 8 characters.";
                return;
            }

            ValidationStatus = ImportRecordStatus.success;

        }

        public void GetCrudStatus(LcpsDbContext context)
        {
            if (_userId.Equals(Guid.Empty))
            {
                CrudStatus = ImportCrudStatus.None;
                return;
            }

            if (ValidationStatus == ImportRecordStatus.danger)
            {
                CrudStatus = ImportCrudStatus.None;
                return;
            }


            LcpsEmail.EmailAccount eml = context.EmailAccounts.FirstOrDefault(x => x.UserLink.Equals(_userId) & x.EmailAddress == EmailAddress);

            if (eml == null)
                CrudStatus = ImportCrudStatus.Insert;
            else
                CrudStatus = ImportCrudStatus.None;

        }

        public void Import(LcpsDbContext context)
        {
            throw new NotImplementedException();
        }

        public void Create(LcpsDbContext context)
        {
            try
            {
                EmailAccount e = new EmailAccount()
                {
                    RecordId = Guid.NewGuid(),
                    EmailAddress = this.EmailAddress,
                    InitialPassword = this.InitialPassword,
                    UserLink = _userId
                };
                context.EmailAccounts.Add(e);
                context.SaveChanges();

                ImportStatus = ImportRecordStatus.success;
            }
            catch(Exception ex)
            {
                Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionCollector ec = new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionCollector(ex);
                ImportStatus = ImportRecordStatus.danger;
                ImportReport = ec.ToUL();
            }
        }

        public void Update(LcpsDbContext context)
        {
            return;
        }
    }
}
