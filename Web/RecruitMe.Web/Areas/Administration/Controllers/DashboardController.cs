namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public IActionResult Index()
        {
            IndexViewModel viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }
    }
}
