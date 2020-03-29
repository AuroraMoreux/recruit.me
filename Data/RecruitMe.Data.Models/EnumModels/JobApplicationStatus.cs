namespace RecruitMe.Data.Models.EnumModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RecruitMe.Data.Common.Models;

    [Table("ApplicationStatuses", Schema = "enum")]
    public class JobApplicationStatus : BaseModel<int>
    {
        public JobApplicationStatus()
        {
            this.JobApplications = new HashSet<JobApplication>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<JobApplication> JobApplications { get; set; }
    }
}
