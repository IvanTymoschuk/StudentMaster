using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMaster.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Can't be empty")]
        [EmailAddress(ErrorMessage = "Not valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Cant't be empty")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,20}$", ErrorMessage = "Password must be at least 6 characters and contain digits, upper or lower case")]
        public string Password { get; set; }

    }
}
