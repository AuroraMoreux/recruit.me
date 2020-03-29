namespace RecruitMe.Data.Models.EnumModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RecruitMe.Data.Common.Models;

    [Table("JobSectors", Schema = "enum")]

    public class JobSector : BaseModel<int>
    {
        public JobSector()
        {
            this.Employers = new HashSet<Employer>();
            this.JobOffers = new HashSet<JobOffer>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Employer> Employers { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
