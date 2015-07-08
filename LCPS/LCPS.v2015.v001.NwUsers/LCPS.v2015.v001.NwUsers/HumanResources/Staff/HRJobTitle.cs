using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using System.Xml.Serialization;

using LCPS.v2015.v001.NwUsers.Infrastructure;
using System.Web.Mvc;

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{

    [Serializable]
    [Table("HRJobTitle", Schema = "HumanResources")]
    public class HRJobTitle : IJobTitle
    {
        [NonSerialized]
        LCPS.v2015.v001.NwUsers.Infrastructure.LcpsDbContext db = new Infrastructure.LcpsDbContext();

        [Key]
        [Display(Name = "Record ID", Description = "GUID for indexing the record")]
        public Guid JobTitleKey  { get; set; }
        
        [Display(Name = "ID", Description = "An ID that uniquely identifies the job title in the division")]
        [Required]
        [Index("IX_JobTitleName", IsUnique = true)]
        [MaxLength(30)]
        public string JobTitleId  { get; set; }

        [Display(Name = "Job Title", Description = "A descriptive name for the job title")]
        [Required]
        [MaxLength(128)]
        public string JobTitleName  { get; set; }




        public static IEnumerable<SelectListItem> GetJobTitleList()
        {
            LcpsDbContext db = new LcpsDbContext();

            List<SelectListItem> items = (from HRJobTitle x in db.JobTitles.OrderBy(b => b.JobTitleName)
                                          select new SelectListItem() { Text = x.JobTitleName, Value = x.JobTitleKey.ToString() }).ToList();

            return items;

        }





        
    }
}
