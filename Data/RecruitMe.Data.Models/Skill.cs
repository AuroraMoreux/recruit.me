namespace RecruitMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;

    public class Skill : BaseDeletableModel<string>
    {
        public Skill()
        {
            this.Id = Guid.NewGuid().ToString();
            this.JobOffers = new HashSet<JobOfferSkill>();
            this.Candidates = new HashSet<CandidateSkill>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<JobOfferSkill> JobOffers { get; set; }

        public virtual ICollection<CandidateSkill> Candidates { get; set; }
    }
}
