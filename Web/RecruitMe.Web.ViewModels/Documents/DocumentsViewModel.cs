namespace RecruitMe.Web.ViewModels.Documents
{
    using System;
    using AutoMapper;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class DocumentsViewModel : IMapFrom<Document>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string FileExtension { get; set; }

        public string Size { get; set; }

        public DateTime UploadedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Document, DocumentsViewModel>().ForMember(dvm => dvm.Name, options =>
              {
                  options.MapFrom(d => d.Name + $".{d.FileExtension}");
              }).ForMember(dvm => dvm.Size, options =>
                   {
                       options.MapFrom(d => d.Size < 1024 ? d.Size.ToString() + " KB" : Math.Round(d.Size / 1024m, 2).ToString() + " MB");
                   })
              .ForMember(dvm => dvm.UploadedOn, options =>
              {
                  options.MapFrom(d => d.CreatedOn);
              })
              .ForMember(dvm => dvm.FileExtension, options =>
              {
                  options.MapFrom(d => d.FileExtension.ToString());
              });
        }
    }
}
