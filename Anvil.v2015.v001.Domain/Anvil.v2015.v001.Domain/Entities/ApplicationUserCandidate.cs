using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anvil.v2015.v001.Domain.Entities
{
    public class ApplicationUserCandidate : ApplicationUser
    {
        #region Constructors

        public ApplicationUserCandidate()
        { }

        public ApplicationUserCandidate(string userName, string emailAddress, string password)
        {
            this.UserName = userName;
            this.Email = emailAddress;
            this.Password = password;
        }

        #endregion

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}
