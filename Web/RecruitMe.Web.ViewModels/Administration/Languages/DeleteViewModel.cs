namespace RecruitMe.Web.ViewModels.Administration.Languages
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class DeleteViewModel : IMapFrom<Language>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Deleted On")]
        public DateTime? DeletedOn { get; set; }

        [Display(Name = "Modified On")]
        public DateTime? ModifiedOn { get; set; }
    }
}
