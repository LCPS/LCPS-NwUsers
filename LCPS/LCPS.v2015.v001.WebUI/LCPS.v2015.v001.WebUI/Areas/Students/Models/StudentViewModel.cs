using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.Students;

using PagedList;


namespace LCPS.v2015.v001.WebUI.Areas.Students.Models
{
    public class StudentViewModel
    {
        private LcpsDbContext _dbContext = new LcpsDbContext();

        private List<Student> _students;

        private StudentFilterModel _filter;
        
        

        public StudentFilterModel FilterClause
        {
            get { return _filter;  }
            set { _filter = value; }
        }

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();

                return _dbContext;
            }
        }

        public int StudentCount
        {
            get { return _students.Count();  }
        }

        public  IPagedList<Student> GetStudents(int?page, int?pageSize)
        {
            IPagedList<Student> pl;
           _students = FilterClause.GetStudents();

            int _pageNumber = (page ?? 1);

            if (_pageNumber < 1)
                _pageNumber = 1;

            int _pageSize = (pageSize ?? 12);


            pl = _students.ToPagedList(_pageNumber, _pageSize);

            return pl;
        }

     
    }
}