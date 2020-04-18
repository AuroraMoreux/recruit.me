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
        public List<JobTypesDropDownCheckboxListViewModel> JobTypesOptions { get; set; }

        public IEnumerable<JobSectorsDropDownViewModel> JobSectors { get; set; }

        public IEnumerable<JobLevelsDropDownViewModel> JobLevels { get; set; }

        public IEnumerable<SkillsDropDownCheckboxListViewModel> Skills { get; set; }

        public IEnumerable<LanguagesDropDownCheckboxListViewModel> Languages { get; set; }
    }
}
