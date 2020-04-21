namespace RecruitMe.Web.ViewModels.JobApplications
{
    using System.Collections.Generic;

    public class JobOfferAllApplicationsModel : PaginatedModel
    {
        public IEnumerable<JobOfferJobApplicationsViewModel> JobApplications { get; set; }

        public string Position { get; set; }

        public string JobOfferId { get; set; }
    }
}
