﻿namespace RecruitMe.Web.ViewModels.Administration.Languages
{
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class CreateViewModel : IMapTo<Language>
    {
        [Required]
        public string Name { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }
    }
}
