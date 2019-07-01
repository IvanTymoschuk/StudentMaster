using System.ComponentModel.DataAnnotations;

namespace StudentMaster.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Can't be empty")]
        [EmailAddress(ErrorMessage = "Not valid email")]
        public string Email { get; set; }

    }
}
