namespace StudentMaster.ViewModels.AdminViewModels
{
    public class SortViewModel
    {
        public SortState AgeSort { get; private set; }
        public SortState FirstNameSort { get; private set; }
        public SortState LastNameSort { get; private set; }
        public SortState RegistredDateSort { get; private set; }
        public SortState StudyDateSort { get; private set; }
        public SortState Current { get; private set; }


        public SortViewModel(SortState sortOrder)
        {
            AgeSort = sortOrder == SortState.AgeAsc ? SortState.AgeDesc : SortState.AgeAsc;
            FirstNameSort = sortOrder == SortState.FirstNameAsc ? SortState.FirstNameDesc : SortState.FirstNameAsc;
            LastNameSort = sortOrder == SortState.LastNameAsc ? SortState.LastNameDesc : SortState.LastNameAsc;
            RegistredDateSort = sortOrder == SortState.RegistredDateAsc ? SortState.RegistredDateDesc : SortState.RegistredDateAsc;
            StudyDateSort = sortOrder == SortState.StudyDateAsc ? SortState.StudyDateDesc : SortState.StudyDateAsc;

            Current = sortOrder;
        }
    }
}
