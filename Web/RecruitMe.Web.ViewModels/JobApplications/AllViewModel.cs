namespace RecruitMe.Web.ViewModels.JobApplications
{
    using System.Collections.Generic;

    public class AllViewModel : PaginatedModel
    {
        public IEnumerable<CandidateJobApplicationsViewModel> JobApplications { get; set; }
    }
}
