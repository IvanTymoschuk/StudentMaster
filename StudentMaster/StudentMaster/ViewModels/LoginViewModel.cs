using System.ComponentModel.DataAnnotations;

namespace StudentMaster.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Can't be empty")]
        [EmailAddress(ErrorMessage = "Not valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Cant't be empty")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,20}$", ErrorMessage = "Password must be at least 6 characters and contain digits, upper or lower case")]
        public string  Password { get; set; }
    }
}
