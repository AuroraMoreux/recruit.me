namespace RecruitMe.Web.ViewModels.JobApplications
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using Ganss.XSS;
    using RecruitMe.Common;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class ApplyViewModel : IMapTo<JobApplication>,IHaveCustomMappings
    {
        [MaxLength(2000)]
        public string Message { get; set; }

        public string SanitizedMessage => new HtmlSanitizer().Sanitize(this.Message);

        // TODO: Find way to integrate candidate skills and languages
        [Required]
        public string CandidateId { get; set; }

        [Required]
        public string JobOfferId { get; set; }

        public CandidateContactDetailsViewModel CandidateDetails { get; set; }

        public JobApplicationJobOfferDetailsViewModel JobOfferDetails { get; set; }

        [MinLength(1, ErrorMessage = GlobalConstants.AtLeastOneDocumentRequired)] // TODO: write custom validator - at least 1, max 5
        public IEnumerable<string> DocumentIds { get; set; }

        public IEnumerable<CandidateDocumentsDropDownViewModel> Documents { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplyViewModel,JobApplication>()
               .ForMember(ja => ja.Message, options =>
               {
                   options.MapFrom(avm => avm.SanitizedMessage);
               });
        }
    }
}
