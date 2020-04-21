namespace RecruitMe.Web.ViewModels.Administration.JobLevels
{
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class CreateViewModel : IMapTo<JobLevel>
    {
        [Required]
        public string Name { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }
    }
}
