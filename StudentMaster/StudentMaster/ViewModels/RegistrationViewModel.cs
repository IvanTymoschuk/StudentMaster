using System;
using System.ComponentModel.DataAnnotations;

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

        //[Required(ErrorMessage = "Can't be empty")]
        [DataType(DataType.Date)]
        [DateAttribute(FromYears: -70, ToYears: -6, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Cant't be empty")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,20}$", ErrorMessage = "Password must be at least 6 characters and contain digits, upper or lower case")]
        public string Password { get; set; }

    }
}
