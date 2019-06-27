using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMaster.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime StudyDate { get; set; }
        public DateTime BirthDate { get; set; }


    }
}
