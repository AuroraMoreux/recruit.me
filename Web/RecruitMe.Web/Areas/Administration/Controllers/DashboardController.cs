namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly IApplicationUsersService applicationUsersService;
        private readonly ICandidatesService candidatesService;
        private readonly IEmployersService employersService;
        private readonly IJobOffersService jobOffersService;
        private readonly IJobApplicationService jobApplicationService;

        public DashboardController(IApplicationUsersService applicationUsersService, ICandidatesService candidatesService, IEmployersService employersService, IJobOffersService jobOffersService, IJobApplicationService jobApplicationService)
        {
            this.applicationUsersService = applicationUsersService;
            this.candidatesService = candidatesService;
            this.employersService = employersService;
            this.jobOffersService = jobOffersService;
            this.jobApplicationService = jobApplicationService;
        }

        public IActionResult Index()
        {
            int totalUsers = this.applicationUsersService.GetCount();
            int usersRegisteredToday = this.applicationUsersService.GetNewUsersCount();
            int candidatesCount = this.candidatesService.GetCount();
            int candidatesRegisteredToday = this.candidatesService.GetNewCandidatesCount();
            int employersCount = this.employersService.GetCount();
            int employersRegisteredToday = this.employersService.GetNewEmployersCount();
            int jobOffersCount = this.jobOffersService.GetCount();
            int newJobOffers = this.jobOffersService.GetNewOffersCount();
            int jobApplicationsCount = this.jobApplicationService.GetCount();
            int newApplications = this.jobApplicationService.GetNewApplicationsCount();
            IndexViewModel indexModel = new IndexViewModel
            {
                TotalUsersCount = totalUsers,
                UsersRegisteredToday = usersRegisteredToday,
                TotalCandidatesCount = candidatesCount,
                CandidatesRegisteredToday = candidatesRegisteredToday,
                TotalEmployersCount = employersCount,
                EmployersRegisteredToday = employersRegisteredToday,
                TotalJobOffersCount = jobOffersCount,
                JobOffersPostedToday = newJobOffers,
                TotalJobApplicationsCount = jobApplicationsCount,
                JobApplicationsSubmittedToday = newApplications,
            };
            return this.View(indexModel);
        }
    }
}
