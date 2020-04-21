namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System.Collections.Generic;

    public class ByEmployerViewModel : PaginatedModel
    {
        public string Name { get; set; }

        public IEnumerable<JobOffersViewModel> JobOffers { get; set; }
    }
}
