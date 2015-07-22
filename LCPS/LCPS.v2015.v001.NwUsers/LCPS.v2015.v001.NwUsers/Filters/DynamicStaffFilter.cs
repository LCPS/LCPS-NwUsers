using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;


using System.Web.Mvc;

using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using Anvil.v2015.v001.Domain.Exceptions;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

namespace LCPS.v2015.v001.NwUsers.Filters
{
    public class DynamicStaffFilter : DynamicQuery
    {
        #region Fields

        private LcpsDbContext _dbContext;

        #endregion

        public DynamicStaffFilter(Guid filterId)
        {
            this.FilterId = filterId;
        }


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

        #endregion

        #region Load

        public void Refresh()
        {
            this.Clear();

            try
            {
                List<StaffFilterClause> _clauses = DbContext.StaffFilterClauses
                    .Where(x => x.FilterId.Equals(FilterId))
                    .OrderBy(x => x.SortIndex)
                    .ToList();

                int count = _clauses.Count();

                foreach (StaffFilterClause c in _clauses)
                {
                    DynamicStaffClause dsc = new DynamicStaffClause(c);
                    Add(dsc);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get staff clauses from the database", ex);
            }
        }

        #endregion

        #region Database


        public void CreateClause(StaffFilterClause c)
        {
            try
            {

                if (c.StaffFilterClauseId.Equals(Guid.Empty))
                    c.StaffFilterClauseId = Guid.NewGuid();

                if (c.FilterId == Guid.Empty)
                    c.FilterId = this.FilterId;

                int count = DbContext.StudentFilterClauses.Where(x => x.FilterId.Equals(c.FilterId)).Count();

                c.SortIndex = count;
                DbContext.StaffFilterClauses.Add(c);
                DbContext.SaveChanges();


                Add(new DynamicStaffClause(c));
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create staff clause", ex);
            }
        }

        public void DeleteStudentClause(Guid staffFilterClauseId)
        {
            try
            {
                StaffFilterClause sfc = DbContext.StaffFilterClauses.FirstOrDefault(x => x.StaffFilterClauseId.Equals(staffFilterClauseId));
                if (sfc == null)
                    return;

                DbContext.StaffFilterClauses.Remove(sfc);
                DbContext.SaveChanges();

                List<StaffFilterClause> cc = DbContext.StaffFilterClauses
                    .Where(x => x.FilterId.Equals(this.FilterId))
                    .OrderBy(x => x.SortIndex)
                    .ToList();

                foreach (StaffFilterClause c in cc)
                {
                    c.SortIndex = cc.IndexOf(c);

                    DbContext.Entry(c).State = System.Data.Entity.EntityState.Modified;
                    DbContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Could not remove staff clause from filter", ex);
            }
        }

        public List<HRStaffRecord> Execute()
        {
            DynamicQueryStatement dqs = ToDynamicQueryStatement();

            try
            {
                HRStaffContext context = new HRStaffContext();
                if (Parms.Count() == 0)
                    return context.HRStaffRecords.OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial).ToList();
                else
                    return context.HRStaffRecords
                        .Where(dqs.Query, dqs.Parms)
                        .OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial)
                        .ToList();

            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector("Could not get staff records from the database");
                ec.Add(ex);
                ec.Add(dqs.Query);
                throw ec.ToException();
            }
        }

        #endregion
    }
}
