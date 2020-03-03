namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.EnumModels;

    public class JobApplication : BaseDeletableModel<string>
    {
        public JobApplication()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        [Required]
        public string JobOfferId { get; set; }

        public JobOffer JobOffer { get; set; }

        [Required]
        public int ApplicationStatusId { get; set; }

        public JobApplicationStatus ApplicationStatus { get; set; }
    }
}
