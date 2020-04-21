namespace RecruitMe.Web.ViewModels.Shared
{
    using System;

    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class IndexJobOffersModel : IMapFrom<JobOffer>
    {
        public string Id { get; set; }

        public string Position { get; set; }

        public string City { get; set; }

        public string EmployerName { get; set; }

        public string JobLevelName { get; set; }

        public DateTime ValidFrom { get; set; }
    }
}
