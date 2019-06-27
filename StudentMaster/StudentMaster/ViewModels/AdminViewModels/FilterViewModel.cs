using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMaster.ViewModels.AdminViewModels
{
    public class FilterViewModel
    {
        public FilterViewModel(string name)
        {
            SelectedName = name;
        }
        public string SelectedName { get; private set; }
    }
}
