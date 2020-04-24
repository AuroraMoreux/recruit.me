namespace RecruitMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;

    public class Candidate : BaseDeletableModel<string>
    {
        public Candidate()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Documents = new HashSet<Document>();
            this.JobApplications = new HashSet<JobApplication>();
            this.Skills = new HashSet<CandidateSkill>();
            this.Languages = new HashSet<CandidateLanguage>();
        }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public string ProfilePictureUrl { get; set; }

        [MaxLength(16)]
        [RegularExpression(@"\+[0-9]+")]
        public string PhoneNumber { get; set; }

        [MaxLength(80)]
        public string ContactAddress { get; set; }

        public string Education { get; set; }

        [MaxLength(1000)]
        public string AboutMe { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; }

        public virtual ICollection<CandidateSkill> Skills { get; set; }

        public virtual ICollection<CandidateLanguage> Languages { get; set; }
    }
}
