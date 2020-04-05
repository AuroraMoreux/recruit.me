namespace RecruitMe.Data.Models.EnumModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RecruitMe.Data.Common.Models;

    [Table("JobLevels", Schema = "enum")]

    public class JobLevel : BaseDeletableModel<int>
    {
        public JobLevel()
        {
            this.JobOffers = new HashSet<JobOffer>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
