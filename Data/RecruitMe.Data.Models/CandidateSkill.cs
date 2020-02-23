namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;

    public class CandidateSkill : BaseDeletableModel<string>
    {
        public CandidateSkill()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        [Required]
        public string SkillId { get; set; }

        public Skill Skill { get; set; }
    }
}
