namespace RecruitMe.Web.ViewModels.Administration.Dashboard
{
    public class IndexViewModel
    {
        public int TotalUsersCount { get; set; }

        public int UsersRegisteredToday { get; set; }

        public int TotalCandidatesCount { get; set; }

        public int CandidatesRegisteredToday { get; set; }

        public int TotalEmployersCount { get; set; }

        public int EmployersRegisteredToday { get; set; }

        public int TotalJobOffersCount { get; set; }

        public int JobOffersPostedToday { get; set; }

        public int TotalJobApplicationsCount { get; set; }

        public int JobApplicationsSubmittedToday { get; set; }
    }
}
