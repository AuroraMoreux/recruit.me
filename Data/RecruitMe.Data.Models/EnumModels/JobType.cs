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
            this.JobOffers = new HashSet<JobOfferJobType>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<JobOfferJobType> JobOffers { get; set; }
    }
}
