namespace RecruitMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.EnumModels;

    public class Employer : BaseDeletableModel<string>
    {
        public Employer()
        {
            this.Id = Guid.NewGuid().ToString();
            this.JobOffers = new HashSet<JobOffer>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string UniqueIdentificationCode { get; set; }

        [MaxLength(12)]
        [RegularExpression("[0-9]+")]
        public string PhoneNumber { get; set; }

        [MaxLength(80)]
        public string Address { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContactPersonNames { get; set; }

        [Required]
        [EmailAddress]
        public string ContactPersonEmail { get; set; }

        [MaxLength(12)]
        [RegularExpression("[0-9]+")]
        public string ContactPersonPhoneNumber { get; set; }

        public string ContactPersonPosition { get; set; }

        public string LogoUrl { get; set; }

        [Url]
        public string WebsiteAddress { get; set; }

        [Required]
        public bool IsPublicSector { get; set; }

        [Required]
        public bool IsHiringAgency { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int JobSectorId { get; set; }

        public virtual JobSector JobSector { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
