namespace RecruitMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using RecruitMe.Data.Common.Models;

    public class JobSector : BaseDeletableModel<string>
    {
        public JobSector()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Employers = new HashSet<Employer>();
            this.JobOffers = new HashSet<JobOffer>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Employer> Employers { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }

    }
}
