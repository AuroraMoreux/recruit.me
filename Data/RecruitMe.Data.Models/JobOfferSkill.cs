namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;

    public class JobOfferSkill : BaseDeletableModel<string>
    {
        public JobOfferSkill()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string JobOfferId { get; set; }

        public JobOffer JobOffer { get; set; }

        [Required]
        public string SkillId { get; set; }

        public Skill Skill { get; set; }
    }
}
