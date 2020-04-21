namespace RecruitMe.Web.ViewModels.JobApplications
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class DetailsViewModel : IMapFrom<JobApplication>, IMapFrom<JobApplicationDocument>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string JobOfferPosition { get; set; }

        public string JobOfferId { get; set; }

        public string CandidateName { get; set; }

        public string CandidateApplicationUserEmail { get; set; }

        public string CandidatePhoneNumber { get; set; }

        public string CandidateLanguages { get; set; }

        public string CandidateSkills { get; set; }

        public string CandidateAboutMe { get; set; }

        public string Message { get; set; }

        public string ApplicationStatusName { get; set; }

        public IList<JobApplicationDocumentsViewModel> Documents { get; set; }

        public IEnumerable<JobApplicationStatusDetailsView> JobApplicationStatusChangeList { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<JobApplication, DetailsViewModel>()
                .ForMember(dvm => dvm.CandidateName, options =>
                   {
                       options.MapFrom(ja => ja.Candidate.FirstName + " " + ja.Candidate.LastName);
                   })
                .ForMember(dvm => dvm.CandidateLanguages, options =>
                   {
                       options.MapFrom(ja => string.Join(", ", ja.Candidate.Languages.Select(cl => cl.Language.Name).ToList()));
                   })
                .ForMember(dvm => dvm.CandidateSkills, options =>
                {
                    options.MapFrom(ja => string.Join(", ", ja.Candidate.Skills.Select(cl => cl.Skill.Name).ToList()));
                });
        }
    }
}
