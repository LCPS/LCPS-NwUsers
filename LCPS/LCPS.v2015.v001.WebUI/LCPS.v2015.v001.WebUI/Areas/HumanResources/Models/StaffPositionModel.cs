using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.HumanResources.DynamicGroups;

using PagedList;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Models
{
    public class StaffPositionModel
    {

        LcpsDbContext _db = new LcpsDbContext();

        IPagedList<HRStaffPosition> _pagedList;

        HRStaffPositionFilter _filter = new HRStaffPositionFilter();

        int _pageNumber;
        int _pageSize;


        public StaffPositionModel(int? page, int? pageSize)
        {
            _pageNumber = (page ?? 1);
            _pageSize = pageSize.Value;
        }

        public StaffPositionModel(int? page, int? pageSize, HRStaffPositionFilter f)
        {
            _pageNumber = (page ?? 1);
            _pageSize = pageSize.Value;
            _filter = f;
        }

        public HRStaffPositionFilter Filter 
        {
            get { return _filter; }
        }

        public LcpsDbContext DbContext
        {
            get
            {
                if (_db == null)
                    _db = new LcpsDbContext();
                return _db;
            }
        }

        public int PageNumber
        {
            get { return Positions.PageNumber; }
        }

        public int PageSize
        {
            get { return Positions.PageSize; }
        }

        public int PageCount
        {
            get { return Positions.PageCount; }
        }

        public IPagedList<HRStaffPosition> Positions
        {
            get
            {
                if (_pagedList == null)
                    _pagedList = GetPositions();

                return _pagedList;

            }
        }




        private IPagedList<HRStaffPosition> GetPositions()
        {
            IPagedList<HRStaffPosition> pl;
            List<HRStaffPosition> items;

            if (Filter == null)
                items = DbContext.StaffPositions.ToList().OrderBy(x => x.StaffMember.LastName + x.StaffMember.FirstName + x.StaffMember.MiddleInitial).ToList();
            else
                items = Filter.GetStaff();

            pl = items.ToPagedList(_pageNumber, _pageSize);

            return pl;
        }

    }
}