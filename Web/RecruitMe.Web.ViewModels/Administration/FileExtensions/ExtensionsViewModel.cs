namespace RecruitMe.Web.ViewModels.Administration.FileExtensions
{
    using System;

    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class ExtensionsViewModel : IMapFrom<FileExtension>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FileType { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
