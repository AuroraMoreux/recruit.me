namespace RecruitMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Common.Models;

    public class JobApplicationDocument : BaseDeletableModel<string>
    {
        public JobApplicationDocument()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string JobApplicationId { get; set; }

        public virtual JobApplication JobApplication { get; set; }

        [Required]
        public string DocumentId { get; set; }

        public virtual Document Document { get; set; }
    }
}
