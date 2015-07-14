using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.Filters.Models
{
    public class FilterViewModel
    {

        #region Fields

        private LcpsDbContext _dbContext;

        #endregion

        #region Constructors

        public FilterViewModel(Guid antecedentId, DynamicQueryClauseFilterCollection filter)
        {
            this.AntecedentId = antecedentId;

            Queries = DbContext.DynamicQueries.Where(x => x.AntecedentId.Equals(antecedentId)).OrderBy(x => x.Name).ToList();

            this.Filter = filter;
        }

        #endregion

        public LcpsDbContext DbContext 
        {
            get 
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();

                return _dbContext;
            }
        }


        public Guid AntecedentId { get; set; }

        public string PageTitle { get; set; }

        public List<DynamicQuery> Queries { get; set; }

        public DynamicQueryClauseFilterCollection Filter { get; set; }

        #region Query Fields


        #endregion
    }
}