namespace RecruitMe.Web.ViewModels.JobApplications
{
    using System;

    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class JobOfferJobApplicationsViewModel : IMapFrom<JobApplication>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string CandidateName { get; set; }

        public string CandidateEducation { get; set; }

        public string ApplicationStatusName { get; set; }

        public int ApplicationStatusId { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<JobApplication, JobOfferJobApplicationsViewModel>()
                 .ForMember(jojavm => jojavm.CandidateName, options =>
                    {
                        options.MapFrom(ja => ja.Candidate.FirstName + " " + ja.Candidate.LastName);
                    })
                 .ForMember(jojavm => jojavm.CreatedOn, options =>
                    {
                        options.MapFrom(ja => ja.CreatedOn.Date);
                    });
        }
    }
}
