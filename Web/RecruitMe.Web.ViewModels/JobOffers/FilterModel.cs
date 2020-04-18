namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Common;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using RecruitMe.Web.ViewModels.Employers;

    public class FilterModel : IValidatableObject
    {
        [MaxLength(100)]
        [Display(Name = "Search For")]
        public string Keywords { get; set; }

        [MaxLength(100)]
        public string Employer { get; set; }

        [MaxLength(80)]
        public string City { get; set; }

        [Display(Name = "Valid From")]
        public DateTime? PublishedFromDate { get; set; }

        [Display(Name = "Valid Until")]
        public DateTime? PublishedToDate { get; set; }

        [Display(Name = "Salary From")]
        public decimal? SalaryFrom { get; set; }

        [Display(Name = "Salary To")]
        public decimal? SalaryTo { get; set; }

        [Display(Name = "Levels")]
        [IntArrayLength("Required Skills", 10, 0)]
        public List<int> LevelsIds { get; set; } = new List<int>();

        [Display(Name = "Sectors")]
        [IntArrayLength("Required Skills", 10, 0)]
        public List<int> SectorsIds { get; set; } = new List<int>();

        [Display(Name = "Types")]
        [IntArrayLength("Required Skills", 10, 0)]
        public List<int> TypesIds { get; set; } = new List<int>();

        [Display(Name = "Languages")]
        [IntArrayLength("Required Skills", 10, 0)]
        public List<int> LanguagesIds { get; set; } = new List<int>();

        [Display(Name = "Skills")]
        [IntArrayLength("Required Skills", 10, 0)]
        public List<int> SkillsIds { get; set; } = new List<int>();

        public IEnumerable<JobLevelsDropDownViewModel> Levels { get; set; }

        public IEnumerable<JobSectorsDropDownViewModel> Sectors { get; set; }

        public IEnumerable<JobTypesDropDownCheckboxListViewModel> Types { get; set; }

        public IEnumerable<LanguagesDropDownCheckboxListViewModel> Languages { get; set; }

        public IEnumerable<SkillsDropDownCheckboxListViewModel> Skills { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.PublishedFromDate < this.PublishedFromDate)
            {
                yield return new ValidationResult(errorMessage: GlobalConstants.ValidUntilDateMustBeCreaterThanValidFromDate, memberNames: new[] { "PublishedFromDate" });
            }

            if (this.SalaryTo < this.SalaryFrom)
            {
                yield return new ValidationResult(errorMessage: GlobalConstants.ValidUntilDateMustBeCreaterThanValidFromDate, memberNames: new[] { "PublishedFromDate" });
            }
        }
    }
}
