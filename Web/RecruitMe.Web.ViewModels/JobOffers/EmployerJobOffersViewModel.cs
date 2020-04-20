namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System;

    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class EmployerJobOffersViewModel : IMapFrom<JobOffer>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Position { get; set; }

        public bool IsActive => this.ValidFrom <= DateTime.UtcNow && this.ValidUntil >= DateTime.UtcNow;

        public DateTime ValidFrom { get; set; }

        public DateTime ValidUntil { get; set; }

        public string JobLevelName { get; set; }

        public string JobLevelId { get; set; }

        public int JobApplicationsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<JobOffer, EmployerJobOffersViewModel>()
                .ForMember(ejovm => ejovm.ValidFrom, options =>
                   {
                       options.MapFrom(jo => jo.ValidFrom.Date);
                   })
                .ForMember(ejovm => ejovm.ValidUntil, options =>
                {
                    options.MapFrom(jo => jo.ValidUntil.Date);
                });
        }
    }
}
