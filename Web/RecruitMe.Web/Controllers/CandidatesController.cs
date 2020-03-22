namespace RecruitMe.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Candidates;

    public class CandidatesController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICandidatesService candidatesService;

        public CandidatesController(UserManager<ApplicationUser> userManager, ICandidatesService candidatesService)
        {
            this.userManager = userManager;
            this.candidatesService = candidatesService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            this.HttpContext.Session.SetString("UserRole", GlobalConstants.CandidateRoleName);

            return this.View();
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.CandidateRoleName)]
        public async Task<IActionResult> CreateProfile()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);

            if (user.CandidateId != null)
            {
                return this.RedirectToAction(nameof(this.UpdateProfile));
            }

            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.CandidateRoleName)]
        public async Task<IActionResult> CreateProfile(CreateCandidateProfileInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);

            input.ApplicationUserId = user.Id;

            string candidateId = await this.candidatesService.CreateProfileAsync(input);

            if (candidateId != null)
            {
                user.CandidateId = candidateId;
                await this.userManager.UpdateAsync(user);
                return this.RedirectToAction(nameof(HomeController.Index));
            }

            return this.View("Error");
        }

        [Authorize(Roles = GlobalConstants.CandidateRoleName)]
        public async Task<IActionResult> UpdateProfile()
        {
            // TODO
            return this.Ok();
        }
    }
}
