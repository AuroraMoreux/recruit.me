namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.EnumModels;

    public class CandidateSkill : BaseDeletableModel<string>
    {
        public CandidateSkill()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string CandidateId { get; set; }

        public virtual Candidate Candidate { get; set; }

        [Required]
        public int SkillId { get; set; }

        public virtual Skill Skill { get; set; }
    }
}
