namespace RecruitMe.Web.ViewModels.Administration.FileExtensions
{
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class CreateViewModel : IMapTo<FileExtension>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "File Type")]
        public string FileType { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }
    }
}
