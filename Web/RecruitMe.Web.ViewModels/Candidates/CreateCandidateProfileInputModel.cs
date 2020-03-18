namespace RecruitMe.Web.ViewModels.Candidates
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class CreateCandidateProfileInputModel : IMapFrom<Candidate>, IMapTo<Candidate>, IHaveCustomMappings
    {
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

        public IFormFile ProfilePicture { get; set; }

        public string ProfilePictureUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CreateCandidateProfileInputModel, Candidate>()
                .ForMember(
                    s => s.ProfilePictureUrl,
                    d => d.MapFrom(c => c.ProfilePictureUrl));
        }
    }
}
