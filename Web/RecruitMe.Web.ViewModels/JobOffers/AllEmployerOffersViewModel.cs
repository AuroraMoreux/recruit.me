namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System.Collections.Generic;

    public class AllEmployerOffersViewModel : PaginatedModel
    {
        public IEnumerable<EmployerJobOffersViewModel> Offers { get; set; }
    }
}
