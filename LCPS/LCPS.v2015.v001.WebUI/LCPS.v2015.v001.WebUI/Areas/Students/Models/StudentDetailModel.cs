using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Anvil.v2015.v001.Domain.Entities;

using LCPS.v2015.v001.NwUsers.Students;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.LcpsEmail;

namespace LCPS.v2015.v001.WebUI.Areas.Students.Models
{
    public class StudentDetailModel : StudentRecord
    {
        #region Fields

        private LcpsDbContext _dbContext = new LcpsDbContext();
        private StudentsContext _studentContext = new StudentsContext();

        #endregion

        #region Constructors

        public StudentDetailModel(Guid id)
        {
            StudentRecord stu = _studentContext.StudentRecords.FirstOrDefault(x => x.StudentKey.Equals(id));
            if (stu == null)
                throw new Exception(string.Format("No student with key {0} could be found in the database", id.ToString()));

            AnvilEntity e = new AnvilEntity(stu);
            e.CopyTo(this);
        }

        #endregion


        #region Email

        public List<EmailAccount> EmailAccounts
        {
            get
            {
                List<EmailAccount> _emails = _dbContext.EmailAccounts
                    .Where(x => x.UserLink.Equals(StudentKey))
                    .OrderBy(x => x.EmailAddress)
                    .ToList();

                return _emails;
            }
        }

        #endregion
    }
}