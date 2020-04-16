namespace RecruitMe.Web.ViewModels.JobOffers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using RecruitMe.Common;
    using RecruitMe.Web.ViewModels.Employers;

    public class EditViewModel
    {
        public EditJobOfferDetailsModel JobOfferDetails { get; set; }

        [Display(Name = "Job Types")]
        public List<JobTypesCheckboxViewModel> JobTypesOptions { get; set; }

        public IEnumerable<JobSectorsDropDownViewModel> JobSectors { get; set; }

        public IEnumerable<JobLevelsCheckboxViewModel> JobLevels { get; set; }

        public IEnumerable<SkillsDropDownCheckboxListViewModel> Skills { get; set; }

        public IEnumerable<LanguagesDropDownCheckboxListViewModel> Languages { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (this.JobTypesOptions.Count(jto => jto.Selected) == 0)
            {
                yield return new ValidationResult(errorMessage: GlobalConstants.SelectionListCannotBeNull, memberNames: new[] { "JobTypesOptions" });
            }
        }
    }
}
