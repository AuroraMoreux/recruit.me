namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AllEmployerOffersViewModel:PaginatedModel
    {
        public IEnumerable<EmployerJobOffersViewModel> Offers { get; set; }
    }
}
