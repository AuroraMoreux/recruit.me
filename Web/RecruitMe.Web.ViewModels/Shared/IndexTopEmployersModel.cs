namespace RecruitMe.Web.ViewModels.Shared
{
    using System;
    using System.Linq;

    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class IndexTopEmployersModel : IMapFrom<Employer>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int EmployerJobOffersCount { get; set; }

        public int NewOffersCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Employer, IndexTopEmployersModel>()
                .ForMember(item => item.NewOffersCount, options =>
                {
                    options.MapFrom(e => e.JobOffers.Count(jo => jo.CreatedOn >= DateTime.UtcNow.AddDays(-1).Date));
                });
        }
    }
}
