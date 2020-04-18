﻿namespace RecruitMe.Web.ViewModels.Employers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using RecruitMe.Web.ValidationAttributes;
    using RecruitMe.Web.ViewModels.Candidates;

    public class UpdateEmployerProfileViewModel : IMapFrom<Employer>, IMapTo<Employer>
    {
        public IEnumerable<string> ImageExtensions { get; set; }

        public string ApplicationUserId { get; set; }

        public string Name { get; set; }

        [UicValidator]
        [Display(Name = "Unique Identification Code")]
        public string UniqueIdentificationCode { get; set; }

        [MaxLength(12)]
        [RegularExpression("[0-9]+")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [MaxLength(80)]
        public string Address { get; set; }

        // TODO: split it into a mini section, partial view possibly? or with a line
        [MaxLength(100)]
        [Display(Name = "Contact Person Names")]
        public string ContactPersonNames { get; set; }

        [EmailAddress]
        [Display(Name = "Contact Person Email")]
        public string ContactPersonEmail { get; set; }

        [MaxLength(12)]
        [RegularExpression("[0-9]+")]
        [Display(Name = "Contact Person Phone Number")]
        public string ContactPersonPhoneNumber { get; set; }

        [Display(Name = "Contact Person Position")]
        public string ContactPersonPosition { get; set; }

        [Display(Name = "Upload Logo")]
        [FileValidatior]
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
