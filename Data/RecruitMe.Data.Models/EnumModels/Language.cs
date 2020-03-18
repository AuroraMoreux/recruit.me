namespace RecruitMe.Data.Models.EnumModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RecruitMe.Data.Common.Models;

    [Table("Languages", Schema = "enum")]

    public class Language
    {
        public Language()
        {
            this.JobOffers = new HashSet<JobOfferLanguage>();
            this.Candidates = new HashSet<CandidateLanguage>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<JobOfferLanguage> JobOffers { get; set; }

        public virtual ICollection<CandidateLanguage> Candidates { get; set; }
    }
}
