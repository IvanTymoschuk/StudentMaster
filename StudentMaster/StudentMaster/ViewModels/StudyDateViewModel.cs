using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMaster.ViewModels
{
    public class DateAttribute : RangeAttribute
    {
        public DateAttribute(int FromYears,int ToYears)
          : base(typeof(DateTime), DateTime.Now.AddYears(FromYears).ToShortDateString(), DateTime.Now.AddYears(ToYears).ToShortDateString()) { }
    }
    public class StudyDateViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        [DataType(DataType.Date)]
        [DateAttribute(FromYears:0,ToYears:10,ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime StudyDate { get; set; }
    }

    public class StudyInfo
    {

        public string UserId { get; set; }

    }
}
