using System;

namespace StudentMaster.ViewModels.AdminViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public DateTime RegistredDate { get; set; }
        public DateTime StudyDate { get; set; }
    }
}
