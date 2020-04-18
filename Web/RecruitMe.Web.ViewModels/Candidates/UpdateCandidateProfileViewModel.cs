namespace RecruitMe.Web.ViewModels.Candidates
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;

    public class UpdateCandidateProfileViewModel : IMapFrom<Candidate>, IMapTo<Candidate>
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

        [Display(Name = "Upload Profile Picture")]
        [FileValidatior]
        public IFormFile ProfilePicture { get; set; }
    }
}
