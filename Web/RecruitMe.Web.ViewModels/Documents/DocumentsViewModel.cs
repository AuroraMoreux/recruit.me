
namespace RecruitMe.Web.ViewModels.Documents
{
    using System;

    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class DocumentsViewModel : IMapFrom<Document>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DocumentCategoryName { get; set; }

        public string FileExtensionName => this.Name.Split(".", StringSplitOptions.None)[1];

        public long Size { get; set; }

        public DateTime UploadedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Document, DocumentsViewModel>()
                .ForMember(dvm => dvm.UploadedOn, options =>
                {
                    options.MapFrom(d => d.CreatedOn);
                });
        }
    }
}
