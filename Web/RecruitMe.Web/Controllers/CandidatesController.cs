namespace RecruitMe.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.Infrastructure.Attributes;
    using RecruitMe.Web.ViewModels.Candidates;

    [AuthorizeRoles(GlobalConstants.CandidateRoleName, GlobalConstants.AdministratorRoleName)]
    public class CandidatesController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICandidatesService candidatesService;
        private readonly IFileExtensionsService fileExtensionsService;
        private IEnumerable<string> allowedExtensions;

        public CandidatesController(
            UserManager<ApplicationUser> userManager,
            ICandidatesService candidatesService,
            IFileExtensionsService fileExtensionsService)
        {
            this.userManager = userManager;
            this.candidatesService = candidatesService;
            this.fileExtensionsService = fileExtensionsService;

            this.allowedExtensions = this.fileExtensionsService.GetImageExtensions();
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            this.HttpContext.Session.SetString("UserRole", GlobalConstants.CandidateRoleName);

            return this.View();
        }

        public IActionResult CreateProfile()
        {
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId != null)
            {
                return this.RedirectToAction(nameof(this.UpdateProfile));
            }

            CreateCandidateProfileInputModel viewModel = new CreateCandidateProfileInputModel
            {
                ImageExtensions = this.allowedExtensions,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateCandidateProfileInputModel input)
        {
            if (this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name) != null)
            {
                return this.RedirectToAction(nameof(this.UpdateProfile));
            }

            if (!this.ModelState.IsValid)
            {
                input.ImageExtensions = this.allowedExtensions;
                return this.View(input);
            }

            if (!this.allowedExtensions.Any(ae => input.ProfilePicture.FileName.EndsWith(ae)))
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.FileExtensionNotSupported);
                input.ImageExtensions = this.allowedExtensions;
                return this.View(input);
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            input.ApplicationUserId = user.Id;

            string candidateId = await this.candidatesService.CreateProfileAsync(input);

            if (candidateId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            user.CandidateId = candidateId;
            await this.userManager.UpdateAsync(user);
            this.TempData["Success"] = GlobalConstants.ProfileSuccessfullyCreated;
            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult UpdateProfile()
        {
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);

            if (candidateId == null)
            {
                return this.RedirectToAction(nameof(this.CreateProfile));
            }

            UpdateCandidateProfileViewModel details = this.candidatesService.GetProfileDetails<UpdateCandidateProfileViewModel>(candidateId);
            if (details == null)
            {
                return this.NotFound();
            }

            details.ImageExtensions = this.allowedExtensions;
            return this.View(details);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateCandidateProfileViewModel input)
        {
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction(nameof(this.CreateProfile));
            }

            if (!this.ModelState.IsValid)
            {
                input.ImageExtensions = this.allowedExtensions;
                return this.View(input);
            }

            if (!this.allowedExtensions.Any(ae => input.ProfilePicture.FileName.EndsWith(ae)))
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.FileExtensionNotSupported);
                input.ImageExtensions = this.allowedExtensions;
                return this.View(input);
            }

            string updateResult = await this.candidatesService.UpdateProfileAsync(candidateId, input);

            if (updateResult == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            this.TempData["Success"] = GlobalConstants.ProfileSuccessfullyUpdated;
            return this.RedirectToAction(nameof(this.Index));
        }

        // TODO: add skills and languages to the models
        // TODO: add job applications review screen
    }
}
