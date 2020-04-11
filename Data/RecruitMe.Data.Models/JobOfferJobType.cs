namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.EnumModels;

    public class JobOfferJobType : BaseDeletableModel<string>
    {
        public JobOfferJobType()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string JobOfferId { get; set; }

        public virtual JobOffer JobOffer { get; set; }

        [Required]
        public int JobTypeId { get; set; }

        public virtual JobType JobType { get; set; }
    }
}
