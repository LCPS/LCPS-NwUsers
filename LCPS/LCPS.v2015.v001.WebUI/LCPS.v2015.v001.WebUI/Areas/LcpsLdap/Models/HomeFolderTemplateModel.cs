using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PagedList;
using PagedList.Mvc;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.NwUsers.LcpsLdap;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates;
using LCPS.v2015.v001.WebUI.Areas.Students.Models;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;
using LCPS.v2015.v001.NwUsers.Students;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models
{
    public class HomeFolderTemplateModel
    {
          #region Fields

        private LcpsDbContext _dbContext;
        private HomeFolderTemplate _currentTemplate;

        #endregion

        #region Constructors

        public HomeFolderTemplateModel(LcpsDbContext context)
        {
            _dbContext = context;

            LcpsAdsDomain dom = LcpsAdsDomain.Default;
            string d = dom.Name;

        }

        public HomeFolderTemplateModel(LcpsDbContext context, Guid id)
        {
            _dbContext = context;
            _currentTemplate = _dbContext.HomeFolderTemplates.Find(id);

            LcpsAdsDomain dom = LcpsAdsDomain.Default;
            string d = dom.Name;
        }

        #endregion

        #region Properties

        public Exception Exception { get; set; }

        public HomeFolderTemplate CurrentTemplate
        {
            get { return _currentTemplate; }
        }

       

        #endregion

        #region HomeFolder Templates

        public List<HomeFolderTemplate> GetTemplates()
        {
            try
            {
                List<HomeFolderTemplate> tt = _dbContext.HomeFolderTemplates.OrderBy(x => x.TemplateName).ToList();
                return tt;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
                return new List<HomeFolderTemplate>();
            }

        }
        #endregion

        #region Student Filters

        public StudentFilterClauseModel GetStudentClauseModel()
        {
            if (CurrentTemplate != null)
            {
                StudentFilterClause c = DynamicStudentClause.GetDefaultStudentClause(CurrentTemplate.HomeFolderId);
                return new StudentFilterClauseModel(c)
                {
                    FormAction = "AddStudentClause",
                    FormController = "HomeFolder",
                    FormArea = "LcpsLdap",
                    SubmitText = "Add Clause"
                };
            }
            else
            {
                throw new Exception("Could not get student filter. The HomeFolder has not been set");
            }
        }

        public DynamicStudentFilter GetStudentFilter()
        {
            if (CurrentTemplate != null)
            {
                DynamicStudentFilter f = new DynamicStudentFilter(CurrentTemplate.HomeFolderId);
                f.Refresh();
                return f;
            }
            else
            {
                throw new Exception("Could not get student filter. The HomeFolder has not been set");
            }
        }

        public PagedList<StudentRecord> GetStudents(Guid id, int? page)
        {
            LcpsDbContext db = new LcpsDbContext();

            MemberFilter f = db.MemberFilters.Find(id);

            if (page == null)
                page = 1;
            else
            {
                if (page == 0)
                    page = 1;
            }

            DynamicStudentFilter stu = new DynamicStudentFilter(id);
            stu.Refresh();
            List<StudentRecord> ss = stu.Execute();

            PagedList<StudentRecord> pl = new PagedList<StudentRecord>(ss, page.Value, 12);

            return pl;
        }

        #endregion

        #region Staff Filters

        public PagedList<HRStaffRecord> GetStaffMembers(Guid id, int? page)
        {
            LcpsDbContext db = new LcpsDbContext();

            MemberFilter f = db.MemberFilters.Find(id);

            if (page == null)
                page = 1;
            else
            {
                if (page == 0)
                    page = 1;
            }

            DynamicStaffFilter stf = new DynamicStaffFilter(id);
            stf.Refresh();
            List<HRStaffRecord> ss = stf.Execute();

            PagedList<HRStaffRecord> pl = new PagedList<HRStaffRecord>(ss, page.Value, 12);

            return pl;
        }

        public StaffFilterClauseModel GetStaffFilterClause(Guid id)
        {
            if (CurrentTemplate != null)
            {
                StaffFilterClauseModel m = new StaffFilterClauseModel(DynamicStaffClause.GetDefault(id))
                {
                    FormAction = "AddStaffClause",
                    FormController = "HomeFolder",
                    FormArea = "LcpsLdap",
                    SubmitText = "Add Clause"
                };
                return m;
            }
            else
                throw new Exception("Could not get student filter. The OU has not been set");
        }

        public DynamicStaffFilter GetStaffFilter()
        {
            if (CurrentTemplate != null)
            {
                DynamicStaffFilter stu = new DynamicStaffFilter(CurrentTemplate.HomeFolderId);
                stu.Refresh();
                return stu;
            }
            else
            {
                throw new Exception("Could not get the staff filter. The OU has not been set");
            }
        }

        #endregion
    }
}