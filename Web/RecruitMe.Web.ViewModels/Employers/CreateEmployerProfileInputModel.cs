namespace RecruitMe.Web.ViewModels.Employers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;

    public class CreateEmployerProfileInputModel : IMapTo<Employer>
    {
        public string ApplicationUserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [UicValidation]
        [Display(Name = "Unique Identification Code")]
        public string UniqueIdentificationCode { get; set; }

        [MaxLength(12)]
        [RegularExpression("[0-9]+")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [MaxLength(80)]
        public string Address { get; set; }

        // TODO: split it into a mini section, partial view possibly? or with a line
        [Required]
        [MaxLength(100)]
        [Display(Name = "Contact Person Names")]
        public string ContactPersonNames { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Contact Person Email")]
        public string ContactPersonEmail { get; set; }

        [MaxLength(12)]
        [RegularExpression("[0-9]+")]
        [Display(Name = "Contact Person Phone Number")]
        public string ContactPersonPhoneNumber { get; set; }

        [Display(Name = "Contact Person Position")]
        public string ContactPersonPosition { get; set; }

        public IFormFile Logo { get; set; }

        [Url]
        [Display(Name = "Website Address")]
        public string WebsiteAddress { get; set; }

        [Display(Name = "Public Sector")]
        public bool IsPublicSector { get; set; }

        [Display(Name = "Hiring Agency")]
        public bool IsHiringAgency { get; set; }

        [Display(Name = "Job Sector")]
        public int JobSectorId { get; set; }

        public IEnumerable<JobSectorsDropDownViewModel> JobSectors { get; set; }
    }
}
