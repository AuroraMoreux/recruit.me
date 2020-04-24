namespace RecruitMe.Web.ViewModels.Employers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using RecruitMe.Common;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using RecruitMe.Web.ValidationAttributes;
    using RecruitMe.Web.ViewModels.Candidates;

    public class CreateEmployerProfileInputModel : IMapTo<Employer>
    {
        public IEnumerable<string> ImageExtensions { get; set; }

        public string ApplicationUserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [UicValidator]
        [Display(Name = "Unique Identification Code")]
        public string UniqueIdentificationCode { get; set; }

        [MaxLength(16)]
        [RegularExpression(@"\+[0-9]+", ErrorMessage = GlobalConstants.PhoneNumberMustBeNoLongerThan15Digits)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [MaxLength(80)]
        public string Address { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Contact Person Names")]
        public string ContactPersonNames { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Contact Person Email")]
        public string ContactPersonEmail { get; set; }

        [MaxLength(16)]
        [RegularExpression(@"\+[0-9]+", ErrorMessage = GlobalConstants.PhoneNumberMustBeNoLongerThan15Digits)]
        [Display(Name = "Contact Person Phone Number")]
        public string ContactPersonPhoneNumber { get; set; }

        [Display(Name = "Contact Person Position")]
        public string ContactPersonPosition { get; set; }

        [FileValidatior(true)]
        [Display(Name = "Upload Logo")]
        public IFormFile Logo { get; set; }

        [Url]
        [Display(Name = "Website Address")]
        public string WebsiteAddress { get; set; }

        [Display(Name = "Public Sector")]
        public bool IsPublicSector { get; set; }

        [Display(Name = "Hiring Agency")]
        public bool IsHiringAgency { get; set; }

        [Required]
        [Display(Name = "Job Sector")]
        public int JobSectorId { get; set; }

        public IEnumerable<JobSectorsDropDownViewModel> JobSectors { get; set; }
    }
}
