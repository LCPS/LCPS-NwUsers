using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace LCPS.v2015.v001.NwUsers.Security
{
    public class EmailImportModel
    {
        [Required]
        [MaxLength(128)]
        public string EntityId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string InitialPassword { get; set; }

        public override string ToString()
        {
            return "(" + EntityId + ") " + Email;
        }
    }
}
