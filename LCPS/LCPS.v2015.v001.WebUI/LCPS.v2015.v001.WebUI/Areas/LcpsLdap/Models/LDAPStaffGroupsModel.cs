using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PagedList;
using PagedList.Mvc;

using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.WebUI.Infrastructure;

namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models
{
    public class LDAPStaffGroupsModel
    {
        #region Fields

        LcpsDbContext _dbContext;
        List<StaffFilter> _staffFilters;

        #endregion

        #region Properties

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();

                return _dbContext;
            }
        }

        public List<StaffFilter> StaffFilters
        {
            get
            {
                if (_staffFilters == null)
                    _staffFilters = DbContext.StaffFilters
                        .Where(x => x.Category == FilterCategories.LDAPGroups)
                        .OrderBy(x => x.Title)
                        .ToList();

                return _staffFilters;
            }
        }



        #endregion

        public IPagedList<StaffFilter> GetStaffFilters(int? page, int? pageSize)
        {
            try
            { 
                int _pageNumber = (page ?? 1);
                int _pageSize = (pageSize ?? 12);

                IPagedList<StaffFilter> _filters = _staffFilters.ToPagedList(_pageNumber, _pageSize);
                return _filters;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get a list of staff for LDAP groups", ex);
            }
        }
    }
}