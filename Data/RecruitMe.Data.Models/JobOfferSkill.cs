namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.EnumModels;

    public class JobOfferSkill : BaseDeletableModel<string>
    {
        public JobOfferSkill()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string JobOfferId { get; set; }

        public virtual JobOffer JobOffer { get; set; }

        [Required]
        public int SkillId { get; set; }

        public virtual Skill Skill { get; set; }
    }
}
