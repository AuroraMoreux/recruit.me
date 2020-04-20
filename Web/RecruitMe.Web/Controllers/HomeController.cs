namespace RecruitMe.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Web.ViewModels;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
                {
                    return this.RedirectToAction("Index", "Dashboard", new { area = "Administration" });
                }
                else if (this.User.IsInRole(GlobalConstants.CandidateRoleName))
                {
                    return this.RedirectToAction("Index", "Candidates");
                }
                else if (this.User.IsInRole(GlobalConstants.EmployerRoleName))
                {
                    return this.RedirectToAction("Index", "Employers");
                }
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
