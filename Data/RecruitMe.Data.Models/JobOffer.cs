namespace RecruitMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.EnumModels;

    public class JobOffer : BaseDeletableModel<string>
    {
        public JobOffer()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Skills = new HashSet<JobOfferSkill>();
            this.Languages = new HashSet<JobOfferLanguage>();
            this.JobApplications = new HashSet<JobApplication>();
            this.JobTypes = new HashSet<JobOfferJobType>();
        }

        [Required]
        [MaxLength(150)]
        public string Position { get; set; }

        [Required]
        public string Description { get; set; }

        public string OfficeAddress { get; set; }

        [Required]
        [MaxLength(80)]
        public string City { get; set; }

        public decimal? Salary { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidUntil { get; set; }

        [Required]
        public string EmployerId { get; set; }

        public virtual Employer Employer { get; set; }

        [Required]
        public int JobLevelId { get; set; }

        public virtual JobLevel JobLevel { get; set; }

        [Required]
        public int JobSectorId { get; set; }

        public virtual JobSector JobSector { get; set; }

        public virtual ICollection<JobOfferSkill> Skills { get; set; }

        public virtual ICollection<JobOfferLanguage> Languages { get; set; }

        public virtual ICollection<JobOfferJobType> JobTypes { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; }
    }
}
