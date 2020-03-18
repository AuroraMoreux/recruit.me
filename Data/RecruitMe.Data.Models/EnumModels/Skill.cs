namespace RecruitMe.Data.Models.EnumModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RecruitMe.Data.Common.Models;

    [Table("Skills", Schema = "enum")]

    public class Skill
    {
        public Skill()
        {
            this.JobOffers = new HashSet<JobOfferSkill>();
            this.Candidates = new HashSet<CandidateSkill>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<JobOfferSkill> JobOffers { get; set; }

        public virtual ICollection<CandidateSkill> Candidates { get; set; }
    }
}
