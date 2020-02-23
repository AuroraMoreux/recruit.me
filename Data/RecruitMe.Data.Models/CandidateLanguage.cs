namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.Enums;

    public class CandidateLanguage : BaseDeletableModel<string>
    {
        public CandidateLanguage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        [Required]
        public string LanguageId { get; set; }

        public Language Language { get; set; }
    }
}
