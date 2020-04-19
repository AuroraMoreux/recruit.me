namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System.Collections.Generic;

    public class AllViewModel : PaginatedModel
    {
        public IEnumerable<JobOffersViewModel> JobOffers { get; set; }
    }
}
