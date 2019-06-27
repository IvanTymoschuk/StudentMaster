using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMaster.ViewModels
{
    public class EditUserViewModel
    {

        [Required(ErrorMessage = "Can't be empty")]
        public string UserId { get; set; }

        [EmailAddress(ErrorMessage = "Not valid email")]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DateAttribute(FromYears: -70, ToYears: -6, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime BirthDate { get; set; }
    }
}
