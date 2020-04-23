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

        public DashboardController(
            IApplicationUsersService applicationUsersService,
            ICandidatesService candidatesService,
            IEmployersService employersService,
            IJobOffersService jobOffersService,
            IJobApplicationService jobApplicationService)
        {
            this.applicationUsersService = applicationUsersService;
            this.candidatesService = candidatesService;
            this.employersService = employersService;
            this.jobOffersService = jobOffersService;
            this.jobApplicationService = jobApplicationService;
        }

        public IActionResult Index()
        {
            var totalUsers = this.applicationUsersService.GetCount();
            var usersRegisteredToday = this.applicationUsersService.GetNewUsersCount();
            var candidatesCount = this.candidatesService.GetCount();
            var candidatesRegisteredToday = this.candidatesService.GetNewCandidatesCount();
            var employersCount = this.employersService.GetCount();
            var employersRegisteredToday = this.employersService.GetNewEmployersCount();
            var jobOffersCount = this.jobOffersService.GetCount();
            var newJobOffers = this.jobOffersService.GetNewOffersCount();
            var jobApplicationsCount = this.jobApplicationService.GetCount();
            var newApplications = this.jobApplicationService.GetNewApplicationsCount();
            var indexModel = new IndexViewModel
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
