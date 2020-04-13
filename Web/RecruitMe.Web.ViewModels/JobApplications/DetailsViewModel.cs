namespace RecruitMe.Web.ViewModels.JobApplications
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class DetailsViewModel : IMapFrom<JobApplication>, IMapFrom<JobApplicationDocument>, IHaveCustomMappings
    {
        public string JobOfferTitle { get; set; }

        public string CandidateName { get; set; }

        public string CandidateApplicationUserEmail { get; set; }

        public string CandidatePhoneNumber { get; set; }

        public string Message { get; set; }

        public IList<JobApplicationDocumentsViewModel> Documents { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<JobApplication, DetailsViewModel>()
                .ForMember(dvm => dvm.CandidateName, options =>
                   {
                       options.MapFrom(ja => ja.Candidate.FirstName + " " + ja.Candidate.LastName);
                   });
        }
    }
}
