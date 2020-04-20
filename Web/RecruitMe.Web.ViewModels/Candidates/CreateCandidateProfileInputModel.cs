namespace RecruitMe.Web.ViewModels.Candidates
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using RecruitMe.Web.ViewModels.JobOffers;

    public class CreateCandidateProfileInputModel : IMapTo<Candidate>
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

        [Display(Name ="Education")]
        public string Education { get; set; }

        [FileValidatior(true)]
        [Display(Name = "Upload Profile Picture")]
        public IFormFile ProfilePicture { get; set; }

        [Display(Name = "My Skills")]
        [IntArrayLength("My Skills", 10, 1)]
        public List<int> SkillsIds { get; set; }

        [Display(Name = "My Languages")]
        [IntArrayLength("My Languages", 5, 1)]
        public List<int> LanguagesIds { get; set; }

        public IEnumerable<SkillsDropDownCheckboxListViewModel> Skills { get; set; }

        public IEnumerable<LanguagesDropDownCheckboxListViewModel> Languages { get; set; }
    }
}
