using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.Students;
using Anvil.v2015.v001.Domain.Exceptions;


namespace LCPS.v2015.v001.NwUsers.Filters
{
    public class DynamicStudentFilter : DynamicQuery
    {

        #region Fields

        private LcpsDbContext _dbContext;


        #endregion

        #region Constructors

        public DynamicStudentFilter(Guid filterId)
        {
            this.FilterId = filterId;
        }

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

        #endregion

        #region Load

        public void Refresh()
        {
            this.Clear();

            try
            {
                List<StudentFilterClause> _clauses = DbContext.StudentFilterClauses
                    .Where(x => x.FilterId.Equals(FilterId))
                    .OrderBy(x => x.SortIndex)
                    .ToList();

                foreach (StudentFilterClause c in _clauses)
                {
                    DynamicStudentClause dsc = new DynamicStudentClause(c);
                    Add(dsc);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get student clauses from the database", ex);
            }
        }

        #endregion

        #region Database



        public void CreateClause(StudentFilterClause c)
        {
            try
            {
                string[] vff = (from DynamicQueryClauseField x in this
                                where x.FieldName != DynamicStudentClause.fBuildingKey & x.Conjunction == DynamicQueryConjunctions.None
                                select x.FieldName).ToArray();

                if (vff.Count() > 0)
                {
                    AnvilExceptionCollector ec = new AnvilExceptionCollector("The following fields have invalid conjunctions.");
                    foreach (string vf in vff)
                    {
                        ec.Add(vf);
                    }

                    throw ec.ToException();
                }

                if (this.Count() > 0 && c.ClauseConjunction == DynamicQueryConjunctions.None)
                    throw new Exception("Please select a valid clause conjunction.");

                if (c.StudentFilterClauseId.Equals(Guid.Empty))
                    c.StudentFilterClauseId = Guid.NewGuid();

                if (c.FilterId == Guid.Empty)
                    c.FilterId = this.FilterId;

                int count = DbContext.StudentFilterClauses.Where(x => x.FilterId.Equals(c.FilterId)).Count();

                c.SortIndex = count;
                DbContext.StudentFilterClauses.Add(c);
                DbContext.SaveChanges();



                Add(new DynamicStudentClause(c));
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create student clause", ex);
            }
        }

        public void DeleteStudentClause(Guid studentFilterClauseId)
        {
            try
            {
                StudentFilterClause sfc = DbContext.StudentFilterClauses.FirstOrDefault(x => x.StudentFilterClauseId.Equals(studentFilterClauseId));
                if (sfc == null)
                    return;

                DbContext.StudentFilterClauses.Remove(sfc);
                DbContext.SaveChanges();

                List<StudentFilterClause> cc = DbContext.StudentFilterClauses
                    .Where(x => x.FilterId.Equals(this.FilterId))
                    .OrderBy(x => x.SortIndex)
                    .ToList();

                foreach (StudentFilterClause c in cc)
                {
                    if (cc.IndexOf(c) == 0)
                        c.ClauseConjunction = DynamicQueryConjunctions.None;

                    c.SortIndex = cc.IndexOf(c);

                    DbContext.Entry(c).State = System.Data.Entity.EntityState.Modified;
                    DbContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Could not remove student clause from filter", ex);
            }
        }

        public List<StudentRecord> Execute()
        {
            DynamicQueryStatement dqs = ToDynamicQueryStatement();

            try
            {
                StudentsContext context = new StudentsContext();
                if (Parms.Count() == 0)
                    return context.StudentRecords.OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial).ToList();
                else
                    return context.StudentRecords
                        .Where(dqs.Query, dqs.Parms)
                        .OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial)
                        .ToList();

            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector("Could not get student records from the database");
                ec.Add(ex);
                ec.Add(dqs.Query);
                throw ec.ToException();
            }
        }

        

            


        #endregion


    }
}
