namespace RecruitMe.Web.ViewModels.Documents
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Http;
    using RecruitMe.Common;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class UploadInputModel : IMapTo<Document>, IHaveCustomMappings
    {
        [Display(Name = "Upload file")]
        [FileValidatior(false)]
        [Required]
        public IFormFile File { get; set; }

        public long Size => this.File.Length / 1024;

        public string SanitizedFileName => new HtmlSanitizer().Sanitize(this.File.FileName);

        [Display(Name = "File category")]
        [Required]
        public int DocumentCategoryId { get; set; }

        public IEnumerable<CategoriesDropDownViewModel> Categories { get; set; }

        public IEnumerable<string> FileExtensions { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UploadInputModel, Document>()
               .ForMember(d => d.Name, options =>
               {
                   options.MapFrom(i => i.SanitizedFileName);
               });
        }
    }
}
