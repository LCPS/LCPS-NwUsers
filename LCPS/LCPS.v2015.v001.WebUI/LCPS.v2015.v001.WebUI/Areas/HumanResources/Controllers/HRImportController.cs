using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using Anvil.v2015.v001.Domain.Exceptions;
using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.WebUI.Areas.HumanResources.Models;
using LCPS.v2015.v001.WebUI.Areas.Import.Models;

using System.IO;
using System.Xml.Serialization;

namespace LCPS.v2015.v001.WebUI.Areas.HumanResources.Controllers
{
    [Authorize(Roles = "APP-Admins,HR-Managers")]
    public class HRImportController : Controller
    {
        LcpsDbContext db = new LcpsDbContext();

        #region Employee Types

        
        public ActionResult ImportEmployeeTypes()
        {
            System.Type it = typeof(HREmployeeType);

            ImportSessionModel s = new ImportSessionModel()
            {
                SessionDate = DateTime.Now,
                SessionId = Guid.NewGuid(),
                Author = User.Identity.Name,
                FullAssemblyTypeName = it.AssemblyQualifiedName,
                TypeName = it.Name,
                Area = "HumanResources",
                Controller = "HRImport",
                Action = "ImportEmployeeTypes",
                ViewLayoutPath = "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml"

            };

            string p = "~/Areas/Import/Views/ImportFile/index.cshtml";

            return View(p, s);
        }

        [HttpPost]
        public ActionResult ImportEmployeeTypes(ImportSession s)
        {
            try
            {

                ImportItem[] items = db.ImportItems.Where(x => x.SessionId.Equals(s.SessionId)).ToArray();
                foreach (ImportItem item in items)
                {
                    HREmployeeType et = ImportItem.Deserialize(item.SerializedItem, typeof(HREmployeeType)) as HREmployeeType;

                    int count = db.EmployeeTypes.Where(x => x.EmployeeTypeId.Equals(et.EmployeeTypeId)).Count();
                    if (count == 0)
                    {
                        try
                        {
                            et.EmployeeTypeLinkId = Guid.NewGuid();
                            db.EmployeeTypes.Add(et);
                            db.SaveChanges();
                            item.ImportStatus = ImportItem.LcpsImportStatus.@default;
                            item.Comment = null;
                        }
                        catch (Exception ex)
                        {
                            AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                            item.Comment = string.Join("\n", ec.ToArray());
                            item.ImportStatus = ImportItem.LcpsImportStatus.danger;
                            db.EmployeeTypes.Remove(et);
                        }
                    }
                    else
                    {
                        item.ImportStatus = ImportItem.LcpsImportStatus.info;
                        item.Comment = "The entry already exists. No changes will be made";
                    }

                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }



                return RedirectToAction("ImportResults", "ImportFile", new { @area = "Import", @id = s.SessionId });
            }
            catch (Exception ex)
            {
                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ex, "Error Importing Employee Types", "HumanResources", "EmployeeType", "Index"));
            }
        }

        #endregion

        #region Job Titles
        public ActionResult ExportJobTitles()
        {
            List<HRJobTitle> items = db.JobTitles.ToList();

            XmlSerializer xml = new XmlSerializer(items.GetType());
            MemoryStream ms = new MemoryStream();
            xml.Serialize(ms, items);

            byte[] fileBytes = ms.ToArray();

            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "HR-JobTitles.xml");
        }


        public ActionResult ImportJobTitles()
        {
            System.Type it = typeof(HRJobTitleImportModel);

            ImportSessionModel s = new ImportSessionModel()
            {
                SessionDate = DateTime.Now,
                SessionId = Guid.NewGuid(),
                Author = User.Identity.Name,
                FullAssemblyTypeName = it.AssemblyQualifiedName,
                TypeName = it.Name,
                Area = "HumanResources",
                Controller = "HRImport",
                Action = "ImportJobTitles",
                ViewLayoutPath = "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml"

            };

            string p = "~/Areas/Import/Views/ImportFile/index.cshtml";

            return View(p, s);
        }

        [HttpPost]
        public ActionResult ImportJobTitles(ImportSession s)
        {
            HRJobTitleImportModel o = null;
            try
            {
                LcpsDbContext db = new LcpsDbContext();

                ImportItem[] items = db.ImportItems.Where(x => x.SessionId.Equals(s.SessionId) & x.ImportStatus == ImportItem.LcpsImportStatus.@default).ToArray();
                foreach (ImportItem item in items)
                {

                    o = ImportItem.Deserialize(item.SerializedItem, typeof(HRJobTitleImportModel)) as HRJobTitleImportModel;

                    if (o.ToString() == "ADM - DirSpEd - DirSpEd")
                    {
                        int x = 0;
                        x++;
                    }

                    HREmployeeType et = db.EmployeeTypes.FirstOrDefault(x => x.EmployeeTypeId == o.EmployeeTypeId);
                    if (et == null)
                    {
                        item.ImportStatus = ImportItem.LcpsImportStatus.danger;
                        item.Comment = string.Format("{0} is not a vaild employee type", o.EmployeeTypeId);
                    }
                    else
                    {
                        int count = db.JobTitles.Where(x => x.JobTitleId == o.JobTitleId & x.EmployeeTypeLinkId.Equals(et.EmployeeTypeLinkId)).Count();

                        HRJobTitle jt = null;
                        if (count == 0)
                        {
                            try
                            {
                                jt = new HRJobTitle()
                                {
                                    RecordId = Guid.NewGuid(),
                                    JobTitleId = o.JobTitleId,
                                    JobTitleName = o.JobTitleName,
                                    EmployeeTypeLinkId = et.EmployeeTypeLinkId
                                };

                                db.JobTitles.Add(jt);
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                db.JobTitles.Remove(jt);

                                item.ImportStatus = ImportItem.LcpsImportStatus.danger;
                                item.Comment = string.Join("\n", (new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionCollector(ex)).ToArray());
                            }
                        }
                        else
                        {
                            item.ImportStatus = ImportItem.LcpsImportStatus.info;
                            item.Comment = "The item exists and will not be modified";
                        }
                    }

                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }
                return RedirectToAction("ImportResults", "ImportFile", new { @area = "Import", @id = s.SessionId });
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                if (o != null)
                {
                    ec.Insert(0, string.Format("Last item was {0}", o.ToString()));
                }

                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ec.ToException(), "Error Importing Job Titles", "HumanResources", "JobTitle", "Index"));
            }
        }

        #endregion

        #region Buildings

        public ActionResult ImportBuildings()
        {
            System.Type it = typeof(HRBuilding);

            ImportSessionModel s = new ImportSessionModel()
            {
                SessionDate = DateTime.Now,
                SessionId = Guid.NewGuid(),
                Author = User.Identity.Name,
                FullAssemblyTypeName = it.AssemblyQualifiedName,
                TypeName = it.Name,
                Area = "HumanResources",
                Controller = "HRImport",
                Action = "ImportBuildings",
                ViewLayoutPath = "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml"

            };

            string p = "~/Areas/Import/Views/ImportFile/index.cshtml";

            return View(p, s);
        }

        [HttpPost]
        public ActionResult ImportBuildings(ImportSession s)
        {
            HRBuilding o = null;
            try
            {
                LcpsDbContext db = new LcpsDbContext();

                ImportItem[] items = db.ImportItems.Where(x => x.SessionId.Equals(s.SessionId) & x.ImportStatus == ImportItem.LcpsImportStatus.@default).ToArray();
                foreach (ImportItem item in items)
                {

                    o = ImportItem.Deserialize(item.SerializedItem, typeof(HRBuilding)) as HRBuilding;

                    int count = db.Buildings.Where(x => x.BuildingId == o.BuildingId).Count();

                    HRBuilding ni = null;
                    if (count == 0)
                    {
                        try
                        {
                            ni = new HRBuilding()
                            {
                                BuildingKey = Guid.NewGuid(),
                                BuildingId = o.BuildingId,
                                Name = o.Name
                            };

                            db.Buildings.Add(ni);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            db.Buildings.Remove(ni);

                            item.ImportStatus = ImportItem.LcpsImportStatus.danger;
                            item.Comment = string.Join("\n", (new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionCollector(ex)).ToArray());
                        }
                    }
                    else
                    {
                        item.ImportStatus = ImportItem.LcpsImportStatus.info;
                        item.Comment = "The item exists and will not be modified";
                    }

                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }



                return RedirectToAction("ImportResults", "ImportFile", new { @area = "Import", @id = s.SessionId });
            }
            catch (Exception ex)
            {
                AnvilExceptionCollector ec = new AnvilExceptionCollector(ex);
                if (o != null)
                {
                    ec.Insert(0, string.Format("Last item was {0}", o.ToString()));
                }

                return View("Error", new Anvil.v2015.v001.Domain.Exceptions.AnvilExceptionModel(ec.ToException(), "Error Importing Job Titles", "HumanResources", "JobTitle", "Index"));
            }
        }


        #endregion

        #region Staff

        public ActionResult ImportStaff()
        {
            System.Type it = typeof(HRStaffImportModel);

            ImportSessionModel s = new ImportSessionModel()
            {
                SessionDate = DateTime.Now,
                SessionId = Guid.NewGuid(),
                Author = User.Identity.Name,
                FullAssemblyTypeName = it.AssemblyQualifiedName,
                TypeName = it.Name,
                Area = "HumanResources",
                Controller = "HRImport",
                Action = "ImportStaff",
                ViewLayoutPath = "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml"

            };

            string p = "~/Areas/Import/Views/ImportFile/index.cshtml";

            return View(p, s);
        }


        [HttpPost]
        public ActionResult ImportStaff(ImportSession s)
        {
            // -- A dictionary of objects to that are created
            Dictionary<string, HRStaff> _justCreated = new Dictionary<string, HRStaff>();

            // -- A list of import items that were not marked as "danger" when read
            ImportItem[] candidates = db.ImportItems.Where(x => x.SessionId.Equals(s.SessionId) & x.ReadStatus == ImportItem.LcpsImportStatus.@default).OrderBy(x => x.EntryDate).ToArray();


            // -- Collect all of the position identifiers so that idivudual trips to the database
            //    Don't have to be made to resolve the string id to guid id
            var employeeTypes =
                    db.EmployeeTypes.ToDictionary
                    (
                        mc => mc.EmployeeTypeId,
                        mc => mc.EmployeeTypeLinkId
                    );

            var jobTitles =
                    db.JobTitles.ToDictionary
                    (
                        x => x.HREmployeeType.EmployeeTypeId + x.JobTitleId,
                        x => x.RecordId
                    );

            var buildings =
                    db.Buildings.ToDictionary
                    (
                        mc => mc.BuildingId,
                        mc => mc.BuildingKey
                    );

            // -- Deserialize all of the HRStaffIMportModel objects
            HRStaffImportModel[] models = candidates.Select(x => x.GetDeserialized(typeof(HRStaffImportModel).AssemblyQualifiedName) as HRStaffImportModel).ToArray();
            var staffs = models.Select(x => new
            {
                x.StaffId,
                x.FirstName,
                x.LastName,
                x.MiddleInitial,
                x.Birthdate,
                x.GenderQualifier
            });

            // Iterate through the list 
            int index = 0;
            foreach (ImportItem candidate in candidates)
            {
                // -- Deserialize the HR Staff Import Model
                HRStaffImportModel model = (HRStaffImportModel)candidate.GetDeserialized(typeof(HRStaffImportModel).AssemblyQualifiedName);

                // -- Null dates are reported as 1/1/0001 which is an invalid date
                //    I'll correct the null date to DateTime.MaxValue representing that since the date
                //    is yet to occur, it must be an invalid birthdate
                if (model.Birthdate.Year == 1)
                    model.Birthdate = DateTime.MaxValue;

                // -- Validate there are not multiple records that have this StaffId with different staff info
                // -- If false all records with the Staff ID will be marked as danger and no sync will be attempted
                if (!StaffHasConflicts(model, models, ref candidates))
                {
                    // -- Check for the staff member in the database
                    HRStaff staffMember = db.StaffMembers.SingleOrDefault(x => x.StaffId == model.StaffId);

                    // -- If the staffMember is null the item does not exist in the database
                    if (staffMember == null)
                    {
                        staffMember = CreateStaffMember(model, ref candidates, index);
                        if (staffMember != null)
                            _justCreated.Add(model.StaffId, staffMember);
                    }
                    else
                    {
                        // -- The Staff ID exists in the database. In case this is an staff member with 
                        //    multiple positions, make sure the member hasn't already been created
                        if (!_justCreated.ContainsKey(model.StaffId))
                        {
                            SyncStaffMember(model, ref candidates, index);
                        }
                    }

                    // -- Get the position info from the model

                    var positionDef = new
                    {
                        StaffId = staffMember.StaffLinkId,
                        BuildingId = model.BuildingId,
                        EmployeeId = model.EmployeeTypeId,
                        JobTitleId = model.JobTitleId,
                        BuildingKey = buildings[model.BuildingId],
                        EmployeeTypeKey = employeeTypes[model.EmployeeTypeId],
                        JobTitleKey = jobTitles[model.EmployeeTypeId + model.JobTitleId]
                    };


                    // -- Try to get the position from the database. The object will be null if the 
                    //    key does not exist

                    HRStaffPosition position = db.StaffPositions.FirstOrDefault
                        (
                            x => x.StaffLinkId.Equals(positionDef.StaffId) &
                                x.BuildingId.Equals(positionDef.BuildingKey) &
                                x.EmployeeTypeId.Equals(positionDef.EmployeeTypeKey) &
                                x.JobTitleId.Equals(positionDef.JobTitleKey)
                        );

                    if (position == null)
                    {
                        try
                        {
                            db.StaffPositions.Add(new HRStaffPosition()
                                {
                                    StaffPositionLinkId = Guid.NewGuid(),
                                    BuildingId = positionDef.BuildingKey,
                                    EmployeeTypeId = positionDef.EmployeeTypeKey,
                                    JobTitleId = positionDef.JobTitleKey,
                                    StaffLinkId = positionDef.StaffId,
                                    Active = model.Active

                                });

                            db.SaveChanges();

                            candidate.ImportStatus = ImportItem.LcpsImportStatus.@default;
                            candidate.Comment += "\n" + string.Format("Position {0} - {1} - {2} - {3} (Active: {4} was added", model.StaffId, model.BuildingId, model.EmployeeTypeId, model.JobTitleId, model.Active.ToString());
                            UpdateStatus(candidate);
                        }
                        catch (Exception ex)
                        {
                            candidate.ImportStatus = ImportItem.LcpsImportStatus.danger;
                            candidate.Comment = string.Join("\n", (new AnvilExceptionCollector(ex)).ToArray());
                            UpdateStatus(candidate);
                        }
                    }
                    else
                    { 
                        if(model.Active == position.Active)
                        {
                            candidate.ImportStatus = ImportItem.LcpsImportStatus.@default;
                            candidate.Comment += "\n" + string.Format("No sync was justified on position {0} - {1} - {2} - {3} (Active: {4} was added", model.StaffId, model.BuildingId, model.EmployeeTypeId, model.JobTitleId, model.Active.ToString());
                            UpdateStatus(candidate);
                        }
                        else
                        {
                            try
                            {
                                position.Active = model.Active;
                                db.Entry(position).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();

                                string activated = "Inactivated";
                                if (model.Active)
                                    activated = "Activated";
                                candidate.Comment = string.Format("The position was {0}", activated);
                                candidate.ImportStatus = ImportItem.LcpsImportStatus.@default;
                                UpdateStatus(candidate);
                            }
                            catch (Exception ex)
                            {
                                candidate.ImportStatus = ImportItem.LcpsImportStatus.danger;
                                candidate.Comment = string.Join("\n", (new AnvilExceptionCollector(ex)).ToArray());
                                UpdateStatus(candidate);
                            }
                        }
                    }


                }

                index++;
            }

            return RedirectToAction("ImportResults", "ImportFile", new { @area = "Import", @id = s.SessionId });
        }

        private void UpdateStatus(ImportItem item)
        {
            item.EntryDate = DateTime.Now;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        private HRStaff CreateStaffMember(HRStaffImportModel model, ref ImportItem[] candidates, int index)
        {
            HRStaff tmpStaff = new HRStaff()
            {
                StaffLinkId = Guid.NewGuid(),
                StaffId = model.StaffId,
                FirstName = model.FirstName,
                MiddleInitial = model.MiddleInitial,
                LastName = model.LastName,
                Birthdate = model.Birthdate,
                Gender = model.GenderQualifier
            };

            try
            {
                db.StaffMembers.Add(tmpStaff);
                db.SaveChanges();
                candidates[index].ImportStatus = ImportItem.LcpsImportStatus.@default;
                candidates[index].Comment = string.Format("Staff ({0}) {1}, {2} was created", model.StaffId, model.FirstName, model.LastName);
                UpdateStatus(candidates[index]);
                return tmpStaff;
            }
            catch (Exception ex)
            {
                candidates[index].ImportStatus = ImportItem.LcpsImportStatus.danger;
                candidates[index].Comment = string.Join("\n", (new AnvilExceptionCollector(ex)).ToArray());
                UpdateStatus(candidates[index]);
                return null;
            }
        }

        private void SyncStaffMember(HRStaffImportModel model, ref ImportItem[] candidates, int index)
        {
            try
            {
                HRStaff staff = db.StaffMembers.First(x => x.StaffId == model.StaffId);
                if (
                    staff.FirstName != model.FirstName |
                    staff.MiddleInitial != model.MiddleInitial |
                    staff.LastName != model.LastName |
                    staff.Gender != model.GenderQualifier |
                    staff.Birthdate != model.Birthdate
                  )
                {
                    // -- There is a change that needs to made
                    staff.FirstName = model.FirstName;
                    staff.MiddleInitial = model.MiddleInitial;
                    staff.LastName = model.LastName;
                    staff.Gender = model.GenderQualifier;
                    staff.Birthdate = model.Birthdate;

                    db.Entry(staff);
                    db.SaveChanges();

                    candidates[index].Comment += "Staff member was update";
                    UpdateStatus(candidates[index]);
                }
                else
                {
                    candidates[index].Comment += "Staff member did not justify sync.";
                    UpdateStatus(candidates[index]);
                }
            }
            catch (Exception ex)
            {
                candidates[index].ImportStatus = ImportItem.LcpsImportStatus.danger;
                candidates[index].Comment = string.Join("\n", (new AnvilExceptionCollector(ex)).ToArray());
                UpdateStatus(candidates[index]);
            }
        }

        private Boolean StaffHasConflicts(HRStaffImportModel stf, HRStaffImportModel[] models, ref ImportItem[] candidates)
        {
            int count = models.Where(x => x.StaffId == stf.StaffId
                        & (x.LastName != stf.LastName |
                            x.FirstName != stf.FirstName |
                            x.MiddleInitial != stf.MiddleInitial |
                            x.Birthdate != stf.Birthdate |
                            x.GenderQualifier != stf.GenderQualifier)).Count();

            if (count > 0)
            {
                foreach (ImportItem i in candidates)
                {
                    HRStaffImportModel m = (HRStaffImportModel)i.GetDeserialized(typeof(HRStaffImportModel).AssemblyQualifiedName);
                    if (m.StaffId == stf.StaffId)
                    {
                        i.ImportStatus = ImportItem.LcpsImportStatus.danger;
                        i.Comment = string.Format("Conflicts detected for this id {0}", stf.StaffId);
                        db.Entry(i).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                db.SaveChanges();

                return true;
            }
            else
                return false;
        }


        private HRStaffPosition GetPosition(string staffId, string buildingId, string employeeTypeId, string jobTitleId)
        {
            HRStaffPosition item = (from HRStaffPosition stf in db.StaffPositions
                                    join s in db.StaffMembers on stf.StaffLinkId equals s.StaffLinkId
                                    join b in db.Buildings on stf.BuildingId equals b.BuildingKey
                                    join e in db.EmployeeTypes on stf.EmployeeTypeId equals e.EmployeeTypeLinkId
                                    join j in db.JobTitles on stf.JobTitleId equals j.RecordId
                                    where s.StaffId == staffId &
                                        b.BuildingId == buildingId &
                                        e.EmployeeTypeId == employeeTypeId &
                                        j.JobTitleId == jobTitleId
                                    select stf).FirstOrDefault();

            return item;
        }

        private void SetStaffImportError(ref ImportItem[] importItems, List<HRStaffImportModel> models, string staffId, Exception ex)
        {
            foreach (HRStaffImportModel m in models.Where(x => x.StaffId == staffId))
            {
                int index = models.IndexOf(m);
                importItems[index].ImportStatus = ImportItem.LcpsImportStatus.danger;
                importItems[index].Comment = string.Join("\n", (new AnvilExceptionCollector(ex)).ToArray());
                db.Entry(importItems[index]).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

        }

        private void SetStaffImportStatus(ref ImportItem[] importItems, List<HRStaffImportModel> models, string staffId, string message, ImportItem.LcpsImportStatus status)
        {

            foreach (HRStaffImportModel m in models.Where(x => x.StaffId == staffId))
            {
                int index = models.IndexOf(m);
                importItems[index].ReadStatus = status;
                importItems[index].Comment += "\n" + message;
                db.Entry(importItems[index]).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

        }
        #endregion
    }
}