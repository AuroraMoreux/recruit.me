﻿namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

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
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        [Range(0, int.MaxValue)]
        public decimal Salary { get; set; }

        [Required]
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
        [Display(Name = "Part-Time")]
        public bool IsFullTime { get; set; }

        [Required]
        [Display(Name = "Remote")]
        public bool IsRemote { get; set; }

        [Required]
        [Display(Name = "Job Sector")]
        public int JobSectorId { get; set; }

        [Required]
        [Display(Name = "Job Level")]
        public int JobLevelId { get; set; }

        [ArrayLength("Required Skills", 10)]
        [Display(Name = "Required Skills")]
        public List<int> SkillsIds { get; set; }

        [ArrayLength("Required Languages", 5)]
        [Display(Name = "Required Languages")]
        public List<int> LanguagesIds { get; set; }

        [Display(Name = "Job Types")]
        public List<JobTypesCheckboxViewModel> JobTypesOptions { get; set; }

        public IEnumerable<JobSectorsDropDownViewModel> JobSectors { get; set; }

        public IEnumerable<JobLevelsCheckboxViewModel> JobLevels { get; set; }

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

            if (this.JobTypesOptions.Count(jto => jto.Selected) == 0)
            {
                yield return new ValidationResult(errorMessage: GlobalConstants.SelectionListCannotBeNull, memberNames: new[] { "JobTypesOptions" });
            }
        }
    }
}
