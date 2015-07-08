using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public override string ToString()
        {
            return BuildingId + " - " + Name;
        }
    }
}
