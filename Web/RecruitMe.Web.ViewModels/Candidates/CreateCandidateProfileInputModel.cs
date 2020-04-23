namespace RecruitMe.Web.ViewModels.Candidates
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Http;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using RecruitMe.Web.ViewModels.JobOffers;

    public class CreateCandidateProfileInputModel : IMapTo<Candidate>, IHaveCustomMappings
    {
        public IEnumerable<string> ImageExtensions { get; set; }

        public string ApplicationUserId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
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

        [FileValidatior(true)]
        [Display(Name = "Upload Profile Picture")]
        public IFormFile ProfilePicture { get; set; }

        [Display(Name = "My Skills")]
        [IntArrayLength("My Skills", 10, 1)]
        public IEnumerable<int> SkillsIds { get; set; } = new List<int>();

        [Display(Name = "My Languages")]
        [IntArrayLength("My Languages", 5, 1)]
        public IEnumerable<int> LanguagesIds { get; set; } = new List<int>();

        public IEnumerable<SkillsDropDownCheckboxListViewModel> Skills { get; set; }

        public IEnumerable<LanguagesDropDownCheckboxListViewModel> Languages { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CreateCandidateProfileInputModel, Candidate>()
                .ForMember(c => c.AboutMe, options =>
                {
                    options.MapFrom(ccpim => ccpim.SanitizedAboutMe);
                });
        }
    }
}
