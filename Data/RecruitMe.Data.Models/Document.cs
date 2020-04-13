namespace RecruitMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;
    using RecruitMe.Data.Models.EnumModels;

    public class Document : BaseDeletableModel<string>
    {
        public Document()
        {
            this.Id = Guid.NewGuid().ToString();
            this.JobApplications = new HashSet<JobApplicationDocument>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int DocumentCategoryId { get; set; }

        public virtual DocumentCategory DocumentCategory { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string CandidateId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual ICollection<JobApplicationDocument> JobApplications { get; set; }
    }
}
