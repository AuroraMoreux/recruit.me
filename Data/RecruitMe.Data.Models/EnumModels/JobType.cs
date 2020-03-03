namespace RecruitMe.Data.Models.EnumModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RecruitMe.Data.Common.Models;

    [Table("JobTypes", Schema = "enum")]

    public class JobType : BaseDeletableModel<int>
    {
        public JobType()
        {
            this.JobOffers = new HashSet<JobOffer>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<JobOffer> JobOffers { get; set; }
    }
}
