﻿namespace RecruitMe.Web.ViewModels.Administration.DocumentCategories
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class EditViewModel : IMapFrom<DocumentCategory>, IMapTo<DocumentCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name="Deleted")]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
