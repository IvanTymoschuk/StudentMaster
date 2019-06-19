using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMaster.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string  Password { get; set; }
    }
}
