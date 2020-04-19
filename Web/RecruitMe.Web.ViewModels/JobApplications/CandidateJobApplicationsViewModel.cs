namespace RecruitMe.Web.ViewModels.JobApplications
{
    using System;

    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class CandidateJobApplicationsViewModel : IMapFrom<JobApplication>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string JobOfferPosition { get; set; }

        public string ApplicationStatusName { get; set; }

        public DateTime ApplicationDate { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<JobApplication, CandidateJobApplicationsViewModel>()
                .ForMember(cjavm => cjavm.ApplicationDate, options =>
                   {
                       options.MapFrom(ja => ja.CreatedOn.Date);
                   });
        }
    }
}
