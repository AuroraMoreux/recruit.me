namespace RecruitMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.Enums;

    public class JobOffer : BaseDeletableModel<string>
    {
        public JobOffer()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Skills = new HashSet<JobOfferSkill>();
            this.Languages = new HashSet<JobOfferLanguage>();
            this.JobApplications = new HashSet<JobApplication>();
        }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsFullTime { get; set; }

        [Required]
        public bool IsRemote { get; set; }

        public string OfficeAddress { get; set; }

        [Required]
        public string City { get; set; }

        public decimal Salary { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidUntil { get; set; }

        [Required]
        public JobLevel JobLevel { get; set; }

        [Required]
        public JobType JobType { get; set; }

        [Required]
        public string JobSectorId { get; set; }

        public JobSector JobSector { get; set; }

        [Required]
        public string EmployerId { get; set; }

        public Employer Employer { get; set; }

        public virtual ICollection<JobOfferSkill> Skills { get; set; }

        public virtual ICollection<JobOfferLanguage> Languages { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; }
    }
}
