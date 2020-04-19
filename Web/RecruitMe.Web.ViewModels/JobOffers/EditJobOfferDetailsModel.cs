namespace RecruitMe.Web.ViewModels.JobOffers
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

    public class EditJobOfferDetailsModel : IMapFrom<JobOffer>, IMapTo<JobOffer>, IHaveCustomMappings, IValidatableObject
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Position { get; set; }

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

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<EditJobOfferDetailsModel, JobOffer>()
                .ForMember(jo => jo.Description, options =>
                {
                    options.MapFrom(ejodm => ejodm.SanitizedDescription);
                })
                .ForMember(jo => jo.Salary, options =>
                {
                    options.MapFrom(ejodm => Math.Round(ejodm.Salary, 2));
                });

            configuration.CreateMap<JobOffer, EditJobOfferDetailsModel>()
                .ForMember(ejodm => ejodm.SkillsIds, options =>
                   {
                       options.MapFrom(jo => jo.Skills.Select(jos => jos.SkillId).ToList());
                   })
                .ForMember(ejodm => ejodm.LanguagesIds, options =>
                {
                    options.MapFrom(jo => jo.Languages.Select(jos => jos.LanguageId).ToList());
                })
                .ForMember(ejodm => ejodm.JobTypesIds, options =>
                   {
                       options.MapFrom(jo => jo.JobTypes.Select(jojt => jojt.JobTypeId).ToList());
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
