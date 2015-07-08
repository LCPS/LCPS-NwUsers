#region Using

using LCPS.v2015.v001.NwUsers.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    [Serializable]
    public class HRStaffBackup
    {
        public List<HRBuilding> Buildings { get; set; }

        public List<HREmployeeType> EmployeeTypes { get; set; }

        public List<HRJobTitle> JobTitles { get; set; }

        public List<HRStaff> Staff { get; set; }

        public List<HRStaffPosition> StaffPositions { get; set; }


        public void Backup(string fileName)
        {
            LcpsDbContext db = new LcpsDbContext();

            Buildings = db.Buildings.ToList();

            EmployeeTypes = db.EmployeeTypes.ToList();

            JobTitles = db.JobTitles.ToList();

            Staff = db.StaffMembers.ToList();

            StaffPositions = db.StaffPositions.ToList();

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, this);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; " + fileName);
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();

        }



    }
}
