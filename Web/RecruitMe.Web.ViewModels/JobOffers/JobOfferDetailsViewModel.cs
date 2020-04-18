namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class JobOfferDetailsViewModel : IMapFrom<JobOffer>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string OfficeAddress { get; set; }

        public string City { get; set; }

        public decimal? Salary { get; set; }

        public string EmployerName { get; set; }

        public string JobLevelName { get; set; }

        public string JobSectorName { get; set; }

        public IEnumerable<string> JobTypes { get; set; }

        public IEnumerable<string> Skills { get; set; }

        public IEnumerable<string> Languages { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<JobOffer, JobOfferDetailsViewModel>()
                .ForMember(jodvm => jodvm.JobTypes, options =>
                {
                    options.MapFrom(jo => jo.JobTypes.Select(jt => jt.JobType.Name).ToList());
                })
                .ForMember(jodvm => jodvm.Skills, options =>
                   {
                       options.MapFrom(jo => jo.Skills.Select(js => js.Skill.Name).ToList());
                   })
                 .ForMember(jodvm => jodvm.Languages, options =>
                 {
                     options.MapFrom(jo => jo.Languages.Select(jl => jl.Language.Name).ToList());
                 });
        }
    }
}
