namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.EnumModels;

    public class Document : BaseDeletableModel<string>
    {
        public Document()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public int DocumentCategoryId { get; set; }

        public DocumentCategory DocumentCategory { get; set; }

        [Required]
        public int FileExtensionId { get; set; }

        public FileExtension FileExtension { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string CandidateId { get; set; }

        public Candidate Candidate { get; set; }
    }
}
