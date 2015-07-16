using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;

using Anvil.v2015.v001.Domain.Exceptions;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    public class HumanResourcesController : Controller
    {
        // GET: HumanResources/HumanResources
        public ActionResult Backup(string u)
        {
            try
            {
                HRStaffBackup b = new HRStaffBackup();
                b.Backup("Lcps-HumanResources.bck");
                return Redirect(u);
            }
            catch (Exception ex)
            {
                return View("Error", new AnvilExceptionModel(ex, "backup Human Resources", "HumanResources", "HREmployeeTypes", "Index"));
            }
        }

        public ActionResult Restore()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Restore(HRRestoreModel m)
        {
            
            MemoryStream ms = new MemoryStream();
            m.Source.InputStream.CopyTo(ms);
            ms.Position = 0;
            XmlSerializer xml = new XmlSerializer(typeof(HRStaffBackup));
            HRStaffBackup b = xml.Deserialize(ms) as HRStaffBackup;
            
            LcpsDbContext db = new LcpsDbContext();
            db.Buildings.RemoveRange(db.Buildings);
            db.EmployeeTypes.RemoveRange(db.EmployeeTypes);
            db.JobTitles.RemoveRange(db.JobTitles);
            db.StaffMembers.RemoveRange(db.StaffMembers);
            db.StaffPositions.RemoveRange(db.StaffPositions);
            db.InstructionalLevels.RemoveRange(db.InstructionalLevels);
            db.Students.RemoveRange(db.Students);

            db.SaveChanges();

            db.Buildings.AddRange(b.Buildings);
            db.EmployeeTypes.AddRange(b.EmployeeTypes);
            db.JobTitles.AddRange(b.JobTitles);
            db.StaffMembers.AddRange(b.Staff);
            db.StaffPositions.AddRange(b.StaffPositions);
            db.InstructionalLevels.AddRange(b.InstructionalLevels);
            db.Students.AddRange(b.Students);


            db.SaveChanges();

            return View();
        }
    }
}