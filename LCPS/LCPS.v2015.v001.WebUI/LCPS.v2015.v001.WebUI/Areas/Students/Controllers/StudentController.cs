using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Entities.DynamicFilters;
using Anvil.v2015.v001.Domain.Exceptions;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.NwUsers.Students;
using LCPS.v2015.v001.WebUI.Areas.Import.Models;
using LCPS.v2015.v001.WebUI.Areas.Students.Models;
using LCPS.v2015.v001.WebUI.Infrastructure;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LCPS.v2015.v001.WebUI.Areas.Students.Controllers
{
    public class StudentController : Controller
    {
        private LcpsDbContext _dbContext = new LcpsDbContext();
        private StudentsContext _studentContext = new StudentsContext();

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();
                return _dbContext;
            }
        }

        public StudentFilterClauseModel Clause
        {
            get
            {
                if (Session["StudentClauseFilterModel"] == null)
                {
                    StudentFilterClauseModel m = new StudentFilterClauseModel()
                    {
                        FormAction = "Search",
                        FormController = "Student",
                        FormArea = "Students",
                        StatusInclude = true,
                        StatusConjunction = DynamicQueryConjunctions.And,
                        StatusOperator = DynamicQueryOperators.Equals,
                        StatusValue = StudentEnrollmentStatus.Enrolled
                    };
                    Session["StudentClauseFilterModel"] = m;
                }
                return (StudentFilterClauseModel)Session["StudentClauseFilterModel"];
            }
            set
            {
                Session["StudentClauseFilterModel"] = value;
            }
        }
        
        #region Import

        public ActionResult ImportFile()
        {
            ImportSession i = new StudentSession().ToImportSession();

            return View("~/Areas/Import/Views/ImportFile.cshtml", i);
        }

        [HttpPost]
        public ActionResult Preview(ImportSession s)
        {
            try
            {
                StudentSession ss = new StudentSession();
                ss.SessionId = s.SessionId;

                using (StreamReader sr = new StreamReader(s.ImportFile.InputStream))
                {
                    ss.ParseItems(sr);
                }


                ImportPreviewModel m = new ImportPreviewModel(ss.SessionId);
                return View("~/Areas/Import/Views/Preview.cshtml", m);
            }
            catch (Exception ex)
            {
                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Parse Import File", "Students", "InstructionalLevel", "Index"));
            }
        }


        [HttpPost]
        public ActionResult Import(ImportSession s)
        {
            try
            {
                ImportSession dbs = DbContext.ImportSessions.First(x => x.SessionId.Equals(s.SessionId));
                StudentSession jt = new StudentSession(dbs);
                jt.Import();
                ImportPreviewModel m = new ImportPreviewModel(s.SessionId);
                return View("~/Areas/Import/Views/Import.cshtml", m);
            }
            catch (Exception ex)
            {
                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Import Staff", "Students", "InstructionalLevel", "ImportFile"));
            }
        }

        #endregion

        #region Filter

        [HttpGet]
        public ActionResult DeleteStudentFilterClause(Guid id)
        {
            string result = "Success";
            try
            {
                StudentFilterClause c = DbContext.StudentFilterClauses.Find(id);
                DbContext.StudentFilterClauses.Remove(c);
                DbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                result = ec.ToUL();
            }

            return Content(result, "text/html");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetInstructionalLevels(Guid buildingId)
        {
            List<InstructionalLevel> jt = Student.GetInstructionalLevels(buildingId);

            var result = (from j in jt
                          select new
                          {
                              id = j.InstructionalLevelKey.ToString(),
                              name = j.InstructionalLevelName
                          }).ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Crud

        // GET: /Students/Student/
        public ActionResult Index(int? page, int? pageSize)
        {
            try
            {
                StudentFilterClauseModel m = this.Clause;

                DynamicStudentClause c = new DynamicStudentClause(m);

                List<StudentRecord> ss = c.Execute();

                ViewBag.Total = ss.Count().ToString();

                if (page == null)
                    page = 1;

                if (pageSize == null)
                    pageSize = 12;

                PagedList<StudentRecord> pl = new PagedList<StudentRecord>(ss, page.Value, pageSize.Value);

                return View(pl);
            }
            catch(Exception ex)
            {
                AnvilExceptionModel em = new AnvilExceptionModel(ex, "Get Students", "Public", "Home", "Index");
                this.Clause = null;
                return View("Error", em);
                

            }
        }


        [HttpPost]
        public ActionResult Search(StudentFilterClauseModel m)
        {
            this.Clause = m;
            return RedirectToAction("Index");
        }



        // GET: /Students/Student/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDetailModel student = new StudentDetailModel(id.Value);

            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: /Students/Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Students/Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentKey,FirstName,MiddleInitial,LastName,Gender,Birthdate,StudentId,InstructionalLevelKey,BuildingKey,Status,SchoolYear")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.StudentKey = Guid.NewGuid();
                DbContext.Students.Add(student);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: /Students/Student/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = DbContext.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Students/Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentKey,FirstName,MiddleInitial,LastName,Gender,Birthdate,StudentId,InstructionalLevelKey,BuildingKey,Status,SchoolYear")] Student student)
        {
            if (ModelState.IsValid)
            {
                DbContext.Entry(student).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: /Students/Student/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = DbContext.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Students/Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Student student = DbContext.Students.Find(id);
            DbContext.Students.Remove(student);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
