﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.Students;
using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.NwUsers.Importing;
using System.IO;
using LCPS.v2015.v001.WebUI.Areas.Import.Models;
using PagedList;
using Anvil.v2015.v001.Domain.Entities;
using System.Linq.Dynamic;
using LCPS.v2015.v001.WebUI.Areas.Students.Models;
using LCPS.v2015.v001.WebUI.Areas.Filters.Models;
using LCPS.v2015.v001.NwUsers.HumanResources;

namespace LCPS.v2015.v001.WebUI.Areas.Students.Controllers
{
    public class StudentController : Controller
    {
        private LcpsDbContext db = new LcpsDbContext();
        //public static StudentViewModel  _studentmodel;


        public List<SelectListItem> BuildingList
        {
            get
            {
                if (Session["buildings"] == null)
                {
                    List<HRBuilding> bb = Student.GetBuildings();


                    Session["buildings"] = (from HRBuilding b in bb
                                                     select new SelectListItem()
                                                     {
                                                         Text = b.Name,
                                                         Value = b.BuildingKey.ToString()
                                                     }).ToList();
                }

                return Session["buildings"] as List<SelectListItem>;
            }
        }

        public StudentFilterModel StudentFilterModel
        {
            get
            {
                if (Session["sf"] == null)
                {
                    StudentFilterModel f = new StudentFilterModel()
                    {
                        FormArea = "Students",
                        FormController = "Student",
                        FormAction = "StudentFilter",
                        Buildings = this.BuildingList,
                    };

                    if (f.Buildings.Count() > 0)
                        f.InstructionalLevels = GetLevelList(new Guid(f.Buildings[0].Value));

                    Session["sf"] = f;
                }
                return (StudentFilterModel) Session["sf"];
            }
            set
            {
                Session["sf"] = value;
            }
        }

        private List<SelectListItem> GetLevelList(Guid bId)
        {

            List<InstructionalLevel> il = Student.GetInstructionalLevels(bId);

            return (from InstructionalLevel j in il
                    select new SelectListItem()
                    {
                        Text = j.InstructionalLevelName,
                        Value = j.InstructionalLevelKey.ToString()
                    }).ToList();

        }

        public StudentViewModel StudentModel
        {
            get
            {
                if (Session["studentModel"] == null)
                {
                    StudentViewModel _model = new StudentViewModel();
                    _model.FilterClause = StudentFilterModel;
                    
                    Session["studentmodel"] = _model;
                }

                return (StudentViewModel)Session["studentModel"];
            }
            set
            {
                Session["studentModel"] = value;
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
                ImportSession dbs = db.ImportSessions.First(x => x.SessionId.Equals(s.SessionId));
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


        public ActionResult Students()
        {
            StudentViewModel m = this.StudentModel;
            if(m.FilterClause == null)
                m.FilterClause = this.StudentFilterModel;



            return View(StudentModel);
        }

        [HttpPost]
        public ActionResult StudentFilter(StudentFilterModel f)
        {
            StudentViewModel _model = this.StudentModel;

            if (f.Buildings == null)
            {
                f.Buildings = this.BuildingList;
                if (!f.BuildingValue.Equals(Guid.Empty))
                    f.InstructionalLevels = this.GetLevelList(f.BuildingValue);
                else
                    f.InstructionalLevels = this.GetLevelList(new Guid(f.Buildings[0].Value));
            }


            _model.FilterClause = f;

            this.StudentModel = _model;


            return View("Students", StudentModel);
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
        public ActionResult Index(int? page, int? pageSize, string currentSearch, string searchString, string filter)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentSearch;
            }

            ViewBag.CurrentFilter = searchString;

            List<Student> students;
            if (String.IsNullOrEmpty(searchString))
                students = db.Students.OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial).ToList();
            else
                students = db.Students.
                    Where
                    (
                        x => x.FirstName.Contains(searchString) |
                            x.LastName.Contains(searchString) |
                            x.StudentId.Contains(searchString)
                    ).ToList();


            if (!string.IsNullOrEmpty(filter))
            {
                List<object> parms = new List<object>();
                List<string> qry = new List<string>();
                foreach (string pr in filter.Split(','))
                {
                    string n = pr.Split(':')[0];
                    Guid g = new Guid(pr.Split(':')[1]);

                    string q = n + " = @" + parms.Count().ToString();
                    qry.Add(q);
                    parms.Add(g);
                }

                string qs = string.Join(" AND ", qry.ToArray());

                students = students.AsQueryable().Where(qs, parms.ToArray()).ToList();
            }

            students = students.OrderBy(x => x.LastName + x.FirstName + x.MiddleInitial).ToList();

            ViewBag.Total = students.Count();

            if (pageSize == null)
                pageSize = 12;

            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize.Value));
        }

        // GET: /Students/Student/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
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
                db.Students.Add(student);
                db.SaveChanges();
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
            Student student = db.Students.Find(id);
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
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
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
            Student student = db.Students.Find(id);
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
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
