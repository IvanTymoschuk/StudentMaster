using System.ComponentModel.DataAnnotations;

namespace StudentMaster.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "Cant't be empty")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,20}$", ErrorMessage = "Password must be at least 6 characters and contain digits, upper or lower case")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Cant't be empty")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,20}$", ErrorMessage = "Password must be at least 6 characters and contain digits, upper or lower case")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
