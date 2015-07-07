#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;


#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources
{
    public class HRPerson
    {
        #region Enums

        public enum HRGenders
        {
            Unknown = 0,
            Male = 1,
            Female = 2
        }

        #endregion

        #region Constants
        #endregion

        #region Fields
        #endregion

        #region Constructors

        public HRPerson()
        {

        }

        #endregion

        #region Properties

        
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(75)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Initial")]
        [MaxLength(3)]
        public string MiddleInitial { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(75)]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "Name")]
        public string SortName
        {
            get { return LastName + ", " + FirstName;  }
        }

        [Display(Name = "Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public HRGenders Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        


        #endregion

    }
}
