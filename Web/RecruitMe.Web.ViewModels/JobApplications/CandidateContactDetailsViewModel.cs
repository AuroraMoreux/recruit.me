namespace RecruitMe.Web.ViewModels.JobApplications
{
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class CandidateContactDetailsViewModel : IMapFrom<Candidate>
    {
        [Required]
        [Display(Name = "First Name: ")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name: ")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Address: ")]
        public string ApplicationUserEmail { get; set; }

        [Display(Name = "Phone Number: ")]
        public string PhoneNumber { get; set; }
    }
}
