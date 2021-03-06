﻿using System.ComponentModel.DataAnnotations;

namespace StudentMaster.ViewModels
{
    public class EditUserViewModel
    {

        [Required(ErrorMessage = "Can't be empty")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        [EmailAddress(ErrorMessage = "Not valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public string LastName { get; set; }

    }
}
