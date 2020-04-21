namespace RecruitMe.Web.ViewModels.Administration.DocumentCategories
{
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class CreateViewModel : IMapTo<DocumentCategory>
    {
        [Required]
        public string Name { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }
    }
}
