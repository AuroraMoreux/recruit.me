namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Ganss.XSS;
    using RecruitMe.Common;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using RecruitMe.Web.ViewModels.Employers;

    public class PostViewModel : IMapTo<JobOffer>, IHaveCustomMappings, IValidatableObject
    {
        [Required]
        [MaxLength(150)]
        public string Position { get; set; }

        [Required]
        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        [Range(0, int.MaxValue)]
        public decimal? Salary { get; set; }

        [Required]
        [MaxLength(80)]
        public string City { get; set; }

        [Display(Name = "Office Address")]
        public string OfficeAddress { get; set; }

        [Required]
        [Display(Name = "Valid From")]
        public DateTime ValidFrom { get; set; }

        [Required]
        [Display(Name = "Valid Until")]
        public DateTime ValidUntil { get; set; }

        [Required]
        [Display(Name = "Job Sector")]
        public int JobSectorId { get; set; }

        [Required]
        [Display(Name = "Job Level")]
        public int JobLevelId { get; set; }

        [Display(Name = "Required Skills")]
        [IntArrayLength("Required Skills", 10, 1)]
        public List<int> SkillsIds { get; set; }

        [Display(Name = "Required Languages")]
        [IntArrayLength("Required Languages", 5, 1)]
        public List<int> LanguagesIds { get; set; }

        [Display(Name = "Job Types")]
        [IntArrayLength("Job Type", 5, 1)]
        public List<int> JobTypesIds { get; set; }

        public List<JobTypesDropDownCheckboxListViewModel> JobTypesOptions { get; set; }

        public IEnumerable<JobSectorsDropDownViewModel> JobSectors { get; set; }

        public IEnumerable<JobLevelsDropDownViewModel> JobLevels { get; set; }

        public IEnumerable<SkillsDropDownCheckboxListViewModel> Skills { get; set; }

        public IEnumerable<LanguagesDropDownCheckboxListViewModel> Languages { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<PostViewModel, JobOffer>()
                 .ForMember(jo => jo.Description, options =>
                    {
                        options.MapFrom(pvm => pvm.SanitizedDescription);
                    });
        }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (this.ValidUntil < this.ValidFrom)
            {
                yield return new ValidationResult(errorMessage: GlobalConstants.ValidUntilDateMustBeCreaterThanValidFromDate, memberNames: new[] { "ValidUntil" });
            }

            if (this.ValidFrom < DateTime.UtcNow.Date)
            {
                yield return new ValidationResult(errorMessage: GlobalConstants.ValidFromDateMustBeAfterCurrentDate, memberNames: new[] { "ValidFrom" });
            }
        }
    }
}
