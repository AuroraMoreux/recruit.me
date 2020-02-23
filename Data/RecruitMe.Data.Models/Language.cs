namespace RecruitMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;

    public class Language : BaseDeletableModel<string>
    {
        public Language()
        {
            this.Id = Guid.NewGuid().ToString();
            this.JobOffers = new HashSet<JobOfferLanguage>();
            this.Candidates = new HashSet<CandidateLanguage>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<JobOfferLanguage> JobOffers { get; set; }

        public virtual ICollection<CandidateLanguage> Candidates { get; set; }
    }
}
