using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMaster.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Can't be empty")]
        [EmailAddress(ErrorMessage = "Not valid email")]
        public string Email { get; set; }

    }
}
