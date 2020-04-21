namespace RecruitMe.Web.ViewModels.Employers
{

    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class ProfileViewModel : IMapFrom<Employer>
    {
        public bool IsProfileCreated { get; set; }

        public string LogoUrl { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string ApplicationUserEmail { get; set; }

        public string Address { get; set; }

        public string WebsiteAddress { get; set; }

        public string JobSectorName { get; set; }

        public string ContactPersonNames { get; set; }

        public string ContactPersonEmail { get; set; }

        public string ContactPersonPhoneNumber { get; set; }

        public string ContactPersonPosition { get; set; }
    }
}
