namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.EnumModels;

    public class CandidateLanguage : BaseDeletableModel<string>
    {
        public CandidateLanguage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string CandidateId { get; set; }

        public virtual Candidate Candidate { get; set; }

        [Required]
        public int LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}
