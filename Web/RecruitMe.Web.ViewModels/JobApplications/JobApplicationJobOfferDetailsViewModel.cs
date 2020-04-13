namespace RecruitMe.Web.ViewModels.JobApplications
{
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class JobApplicationJobOfferDetailsViewModel : IMapFrom<JobOffer>
    {
        [Required]
        public string Title { get; set; }
    }
}
