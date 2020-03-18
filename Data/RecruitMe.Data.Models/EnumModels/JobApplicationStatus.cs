namespace RecruitMe.Data.Models.EnumModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RecruitMe.Data.Common.Models;

    [Table("ApplicationStatuses", Schema = "enum")]
    public class JobApplicationStatus
    {
        public JobApplicationStatus()
        {
            this.JobApplications = new HashSet<JobApplication>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; }
    }
}
