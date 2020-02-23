namespace RecruitMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;

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

        public string Logo { get; set; }

        public string WebsiteAddress { get; set; }

        [Required]
        public bool IsPublicSector { get; set; }

        [Required]
        public bool IsHiringAgency { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string JobSectorId { get; set; }

        public JobSector JobSector { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
