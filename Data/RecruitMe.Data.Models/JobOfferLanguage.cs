namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.Enums;

    public class JobOfferLanguage : BaseDeletableModel<string>
    {
        public JobOfferLanguage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string JobOfferId { get; set; }

        public JobOffer JobOffer { get; set; }

        [Required]
        public string LanguageId { get; set; }

        public Language Language { get; set; }
    }
}
