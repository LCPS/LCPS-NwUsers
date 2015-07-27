using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.NwUsers.LcpsLdap;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates;
using LCPS.v2015.v001.WebUI.Areas.Students.Models;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models
{
    public class OUTemplateViewModel
    {
        #region Fields

        private LcpsDbContext _dbContext;
        private OUTemplate _currentTemplate;

        #endregion

        #region Constructors

        public OUTemplateViewModel(LcpsDbContext context)
        {
            _dbContext = context;

            LcpsAdsDomain dom = LcpsAdsDomain.Default;
            string d = dom.Name;

        }

        public OUTemplateViewModel(LcpsDbContext context, Guid id)
        {
            _dbContext = context;
            _currentTemplate = _dbContext.OUTemplates.Find(id);

            LcpsAdsDomain dom = LcpsAdsDomain.Default;
            string d = dom.Name;
        }

        #endregion

        #region Properties

        public Exception Exception { get; set; }

        public OUTemplate CurrentTemplate
        {
            get { return _currentTemplate; }
        }

        public OuTreeModel CreateTemplateTree
        {
            get
            {
                OuTreeModel m = new OuTreeModel()
                {
                    FormAction = null,
                    FormController = null,
                    FormArea = null
                };

                return m;
            }
        }

        #endregion

        #region OU Templates

        public List<OUTemplate> GetTemplates()
        {
            try
            {
                List<OUTemplate> tt = _dbContext.OUTemplates.OrderBy(x => x.TemplateName).ToList();
                return tt;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
                return new List<OUTemplate>();
            }

        }

        #endregion

        #region Student Filters

        public StudentFilterClauseModel GetStudentClauseModel()
        {
            if (CurrentTemplate != null)
            {
                StudentFilterClause c = DynamicStudentClause.GetDefaultStudentClause(CurrentTemplate.OUId);
                return new StudentFilterClauseModel(c)
                    {
                        FormAction = "AddStudentClause",
                        FormController = "LdapOuTemplate",
                        FormArea = "LcpsLdap",
                        SubmitText = "Add Clause"
                    };
            }
            else
            {
                throw new Exception("Could not get student filter. The OU has not been set");
            }
        }

        public DynamicStudentFilter GetStudentFilter()
        {
            if (CurrentTemplate != null)
            {
                DynamicStudentFilter f = new DynamicStudentFilter(CurrentTemplate.OUId);
                f.Refresh();
                return f;
            }
            else
            {
                throw new Exception("Could not get student filter. The OU has not been set");
            }
        }

        #endregion

        #region Staff Filters

        public StaffFilterClauseModel GetStaffFilterClause()
        {
            if (CurrentTemplate != null)
            {
                StaffFilterClauseModel m = new StaffFilterClauseModel(DynamicStaffClause.GetDefaultSearch())
                    {
                        FormAction = "AddStaffClause",
                        FormController = "LdapOuTemplate",
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
                DynamicStaffFilter stu = new DynamicStaffFilter(CurrentTemplate.OUId);
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