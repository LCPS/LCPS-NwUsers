using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LCPS.v2015.v001.NwUsers.Students;
using PagedList;
using PagedList.Mvc;

using LCPS.v2015.v001.NwUsers.Filters;
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using LCPS.v2015.v001.WebUI.Infrastructure;


namespace LCPS.v2015.v001.WebUI.Areas.Students.Models
{
    public class StudentFilterListModel
    {
        PagedList<StudentRecord> _studentsPgd;
        int _currentPage = 1;

        public StudentFilterListModel(Guid filterId)
        {
            this.FilterId = FilterId;
        }

        public string PageHeader { get; set;}

        public string PageTitle { get; set; }

        public string Layout { get; set; }

        public Guid FilterId { get; set; }

        public int Total { get; set; }

        public int DefaultPageSize { get; set; }

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
        }

        

    }
}