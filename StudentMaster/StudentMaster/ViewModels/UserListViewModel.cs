using StudentMaster.ViewModels.AdminViewModels;
using System.Collections.Generic;

namespace StudentMaster.ViewModels
{
    public class UserListViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
    }
}
