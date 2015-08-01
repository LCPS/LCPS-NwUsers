using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.NwUsers.HumanResources
{
    [Serializable]
    [Table("HRBuilding", Schema = "HumanResources")]
    public class HRBuilding : IBuilding
    {
        [Key]
        public Guid BuildingKey { get; set; }

        [Display(Name = "BuildingId", Description = "An ID that uniquely identifies the building in the division")]
        [Required(ErrorMessage = "The building ID is a required field", AllowEmptyStrings = false)]
        [Index("IX_BuildingId", IsUnique = true)]
        [MaxLength(50)]
        public string BuildingId { get; set; }

        [Display(Name = "Building")]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public override string ToString()
        {
            return BuildingId + " - " + Name;
        }

        public static IEnumerable<SelectListItem> GetBuildingList()
        {
            LcpsDbContext db = new LcpsDbContext();

            List<SelectListItem> items = (from HRBuilding x in db.Buildings.OrderBy(b => b.Name)
                                          select new SelectListItem() { Text = x.Name, Value = x.BuildingKey.ToString() }).ToList();

            return items;
                                          
        }

        public static string GetBuildingName(Guid id)
        {
            if (id.Equals(Guid.Empty))
                return "";
            else
            {
                LcpsDbContext db = new LcpsDbContext();
                return db.Buildings.Find(id).Name;
            }
        }

    }
}
