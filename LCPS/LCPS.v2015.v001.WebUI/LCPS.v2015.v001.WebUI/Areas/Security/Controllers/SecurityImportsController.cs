using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.WebUI.Areas.Import.Models;
using LCPS.v2015.v001.NwUsers.Security;
using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

using Anvil.v2015.v001.Domain.Exceptions;

namespace LCPS.v2015.v001.WebUI.Areas.Security.Controllers
{
    public class SecurityImportsController : Controller
    {
        LcpsDbContext db = new LcpsDbContext();

        //
        // GET: /Security/SecurityImports/
        public ActionResult ImportStaffEmail()
        {
            System.Type it = typeof(EmailImportModel);

            ImportSessionModel s = new ImportSessionModel()
            {
                SessionDate = DateTime.Now,
                SessionId = Guid.NewGuid(),
                Author = User.Identity.Name,
                FullAssemblyTypeName = it.AssemblyQualifiedName,
                TypeName = it.Name,
                Area = "Security",
                Controller = "SecurityImports",
                Action = "ImportStaffEmail",
                ViewLayoutPath = "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml"

            };

            string p = "~/Areas/Import/Views/ImportFile/index.cshtml";

            return View(p, s);
        }

        [HttpPost]
        public ActionResult ImportStaffEmail(ImportSessionModel s)
        {
            ImportItem[] items = db.ImportItems.Where(x => x.SessionId.Equals(s.SessionId)).ToArray();

            foreach (ImportItem ii in items)
            {
                EmailImportModel eml = (EmailImportModel)ii.GetDeserialized(typeof(EmailImportModel).AssemblyQualifiedName);

                HRStaff staff = db.StaffMembers.FirstOrDefault(x => x.StaffId == eml.EntityId);

                if (staff == null)
                {
                    ii.ImportStatus = ImportItem.LcpsImportStatus.danger;
                    ii.Comment = "Staff ID was not found.";
                    db.Entry(ii).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    LcpsStaffEmail se = db.StaffEmails.FirstOrDefault(x => x.StaffLinkId.Equals(staff.StaffLinkId));
                    if (se == null)
                    {
                        se = new LcpsStaffEmail()
                            {
                                EmailId = Guid.NewGuid(),
                                StaffLinkId = staff.StaffLinkId,
                                Email = eml.Email,
                                InitialPassword = eml.InitialPassword
                            };

                        db.StaffEmails.Add(se);

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            db.StaffEmails.Remove(se);
                            AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                            ii.Comment = string.Join("\n", ec.ToArray());
                            ii.ImportStatus = ImportItem.LcpsImportStatus.danger;
                            db.Entry(ii).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

            }


            return RedirectToAction("ImportResults", "ImportFile", new { @area = "Import", @id = s.SessionId });
        }
    }
}