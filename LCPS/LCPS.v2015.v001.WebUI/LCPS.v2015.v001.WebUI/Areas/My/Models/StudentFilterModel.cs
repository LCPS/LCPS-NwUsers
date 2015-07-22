using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using Anvil.v2015.v001.Domain.Exceptions;
using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Html;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.WebUI.Areas.Students.Models;
using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.My.Models
{
    public class StudentFilterModel : IAnvilFormHandler
    {
        #region Fields

        private StudentClauseFilterModel _clauseModel;
        private MemberFilter _memberFilter;
        private DynamicStudentFilter _studentFilter;

        #endregion

        #region Constructors

        public StudentFilterModel(Guid filterId, string formArea, string formController, string formAction, string submitText, string onErrorActionName)
        {
            this.FilterId = filterId;
            this.FormArea = formArea;
            this.FormController = formController;
            this.FormAction = formAction;
            this.SubmitText = submitText;
            this.OnErrorActionName = onErrorActionName;
        }

        #endregion

        #region Properties

        public string FormArea { get; set; }

        public string FormController { get; set; }

        public string FormAction { get; set; }

        public string SubmitText { get; set; }

        public string OnErrorActionName { get; set; }

        public Guid FilterId { get; set; }

        public MemberFilter MemberFilter
        {
            get
            {
                if (_memberFilter == null)
                {
                    LcpsDbContext db = new LcpsDbContext();
                    _memberFilter = db.MemberFilters.Find(FilterId);
                }
                return _memberFilter;
            }
        }

        public DynamicStudentFilter StudentFilter
        {
            get
            {
                if (_studentFilter == null)
                {
                    _studentFilter = new DynamicStudentFilter(FilterId);
                    _studentFilter.Refresh();
                }
                return _studentFilter;
            }
        }

        public StudentClauseFilterModel GetClauseModel(Guid filterId)
        {
            if (_clauseModel == null)
                _clauseModel = new StudentClauseFilterModel(DynamicStudentClause.GetDefaultStudentClause(filterId));

            _clauseModel.FormArea = this.FormArea;
            _clauseModel.FormController = this.FormController;
            _clauseModel.FormAction = this.FormAction;
            _clauseModel.OnErrorActionName = this.OnErrorActionName;

            return _clauseModel;
        }

        #endregion

    }
}