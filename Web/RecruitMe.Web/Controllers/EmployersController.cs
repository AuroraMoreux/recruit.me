namespace RecruitMe.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Data.Models;

    public class EmployersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmployerService employerService;

        public EmployersController(UserManager<ApplicationUser> userManager, IEmployerService employerService)
        {
            this.userManager = userManager;
            this.employerService = employerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            this.HttpContext.Session.SetString("UserRole", GlobalConstants.EmployerRoleName);

            return this.View();
        }

        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        [HttpGet]
        public IActionResult CreateProfile()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateProfileInputModel input)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            input.ApplicationUserId = user.Id;

            string candidateId = await this.employerService.CreateProfile(input);

            if (candidateId != null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View("Error");
        }
    }
}
