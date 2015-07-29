using System;
using System.DirectoryServices;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects;
using LCPS.v2015.v001.WebUI.Infrastructure;
using LCPS.v2015.v001.NwUsers.LcpsLdap.LdapTemplates;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Students;



namespace LCPS.v2015.v001.WebUI.Areas.LcpsLdap.Models
{
    public class LcpsAdsUserModel
    {
        private LcpsDbContext db = new LcpsDbContext();

        public LcpsAdsUserModel(string userName, Guid userKey)
        {
            this.User = LcpsAdsUser.Get(userName);
            this.ExistsInAds = (this.User != null);
            this.UserKey = userKey;
            this.StaffMember = db.StaffMembers.FirstOrDefault(x => x.StaffKey.Equals(userKey));
            this.Student = db.Students.FirstOrDefault(x => x.StudentKey.Equals(userKey));
        }

        public Guid UserKey { get; set; }

        public LcpsAdsUser User { get; set; }

        public bool ExistsInAds { get; set; }

        public HRStaff StaffMember { get; set; }

        public Student Student { get; set; }

        public LcpsAdsOu ParentOu
        {
            get
            {
                if (!ExistsInAds)
                    return null;

                DirectoryEntry ou = User.DirectoryEntry.Parent;
                return new LcpsAdsOu(ou);
            }
        }

        public IEnumerable<OUTemplate> OUTemplates
        {
            get {

                List<OUTemplate> items = new List<OUTemplate>();

                foreach(OUTemplate t in db.OUTemplates.OrderBy(x => x.TemplateName))
                {
                        DynamicStaffFilter sf = new DynamicStaffFilter(t.OUId);
                        sf.Refresh();
                        List<HRStaffRecord> stf = sf.Execute();
                        
                        if (stf.Where(
                            x => x.StaffKey.Equals(this.UserKey)).Count() > 0)
                            items.Add(t);

                        DynamicStudentFilter stuF = new DynamicStudentFilter(t.OUId);
                        stuF.Refresh();
                        List<StudentRecord> stu = stuF.Execute();
                        if (stu.Where(x => x.StudentKey.Equals(this.UserKey)).Count() > 0)
                            items.Add(t);
                }

                return items;
            }
        }

        public IEnumerable<GroupTemplate> GroupTemplates
        {
            get
            {

                List<GroupTemplate> items = new List<GroupTemplate>();

                foreach (GroupTemplate t in db.GroupTemplates.OrderBy(x => x.TemplateName))
                {
                    DynamicStaffFilter sf = new DynamicStaffFilter(t.GroupId);
                    sf.Refresh();
                    List<HRStaffRecord> stf = sf.Execute();

                    if (stf.Where(
                        x => x.StaffKey.Equals(this.UserKey)).Count() > 0)
                        items.Add(t);

                    DynamicStudentFilter stuF = new DynamicStudentFilter(t.GroupId);
                    stuF.Refresh();
                    List<StudentRecord> stu = stuF.Execute();
                    if (stu.Where(x => x.StudentKey.Equals(this.UserKey)).Count() > 0)
                        items.Add(t);
                }

                return items;
            }
        }


    }
}