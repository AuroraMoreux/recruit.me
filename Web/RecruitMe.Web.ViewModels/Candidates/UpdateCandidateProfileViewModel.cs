namespace RecruitMe.Web.ViewModels.Candidates
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Http;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using RecruitMe.Web.ViewModels.JobOffers;

    public class UpdateCandidateProfileViewModel : IMapFrom<Candidate>, IMapTo<Candidate>, IMapFrom<CandidateLanguage>, IMapTo<CandidateLanguage>, IHaveCustomMappings
    {
        public IEnumerable<string> ImageExtensions { get; set; }

        public string ApplicationUserId { get; set; }

        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(12)]
        [RegularExpression("[0-9]+")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [MaxLength(80)]
        [Display(Name = "Contact Address")]
        public string ContactAddress { get; set; }

        [Display(Name = "Education")]
        public string Education { get; set; }

        [MaxLength(800)]
        [Display(Name = "About Me")]
        public string AboutMe { get; set; }

        public string SanitizedAboutMe => new HtmlSanitizer().Sanitize(this.AboutMe);

        [Display(Name = "Upload Profile Picture")]
        [FileValidatior(true)]
        public IFormFile ProfilePicture { get; set; }

        [Display(Name = "My Skills")]
        [IntArrayLength("My Skills", 10, 1)]
        public List<int> SkillsIds { get; set; }

        [Display(Name = "My Languages")]
        [IntArrayLength("My Languages", 5, 1)]
        public List<int> LanguagesIds { get; set; }

        public IEnumerable<SkillsDropDownCheckboxListViewModel> SkillsList { get; set; }

        public IEnumerable<LanguagesDropDownCheckboxListViewModel> LanguagesList { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Candidate, UpdateCandidateProfileViewModel>()
                 .ForMember(ucpvm => ucpvm.SkillsIds, options =>
                 {
                     options.MapFrom(c => c.Skills.Select(jos => jos.SkillId).ToList());
                 })
                 .ForMember(ucpvm => ucpvm.LanguagesIds, options =>
                 {
                     options.MapFrom(c => c.Languages.Select(jos => jos.LanguageId).ToList());
                 });

            configuration.CreateMap<UpdateCandidateProfileViewModel, Candidate>()
           .ForMember(c => c.AboutMe, options =>
           {
               options.MapFrom(ucpvm => ucpvm.SanitizedAboutMe);
           });
        }
    }
}
