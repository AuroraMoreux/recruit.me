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
    using RecruitMe.Web.ViewModels.JobOffers;
    using RecruitMe.Web.ViewModels.Shared;

    [AuthorizeRoles(GlobalConstants.CandidateRoleName, GlobalConstants.AdministratorRoleName)]
    public class CandidatesController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICandidatesService candidatesService;
        private readonly IFileExtensionsService fileExtensionsService;
        private readonly ILanguagesService languagesService;
        private readonly ISkillsService skillsService;
        private readonly IJobOffersService jobOffersService;
        private readonly IEmployersService employersService;
        private readonly IEnumerable<string> allowedExtensions;
        private readonly IEnumerable<LanguagesDropDownCheckboxListViewModel> languages;
        private readonly IEnumerable<SkillsDropDownCheckboxListViewModel> skills;

        public CandidatesController(
            UserManager<ApplicationUser> userManager,
            ICandidatesService candidatesService,
            IFileExtensionsService fileExtensionsService,
            ILanguagesService languagesService,
            ISkillsService skillsService,
            IJobOffersService jobOffersService,
            IEmployersService employersService)
        {
            this.userManager = userManager;
            this.candidatesService = candidatesService;
            this.fileExtensionsService = fileExtensionsService;
            this.languagesService = languagesService;
            this.skillsService = skillsService;
            this.jobOffersService = jobOffersService;
            this.employersService = employersService;
            this.allowedExtensions = this.fileExtensionsService.GetImageExtensions();
            this.languages = this.languagesService.GetAll<LanguagesDropDownCheckboxListViewModel>();
            this.skills = this.skillsService.GetAll<SkillsDropDownCheckboxListViewModel>();
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                this.HttpContext.Session.SetString("UserRole", GlobalConstants.CandidateRoleName);
            }

            var viewModel = new IndexViewModel();
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId != null)
            {
                viewModel.IsProfileCreated = true;
            }

            var lastTenOffers = this.jobOffersService.GetLastTenOffers<IndexJobOffersModel>();
            var topEmployers = this.employersService.GetTopFiveEmployers<IndexTopEmployersModel>();
            viewModel.LastTenJobOffers = lastTenOffers;
            viewModel.TopEmployers = topEmployers;
            return this.View(viewModel);
        }

        public IActionResult MyProfile()
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction(nameof(this.CreateProfile));
            }

            var viewModel = this.candidatesService.GetProfileDetails<ProfileViewModel>(candidateId);

            return this.View(viewModel);
        }

        public IActionResult CreateProfile()
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId != null)
            {
                return this.RedirectToAction(nameof(this.UpdateProfile));
            }

            var viewModel = new CreateCandidateProfileInputModel
            {
                ImageExtensions = this.allowedExtensions,
                Languages = this.languages,
                Skills = this.skills,
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
                input.Languages = this.languages;
                input.Skills = this.skills;
                return this.View(input);
            }

            if (input.ProfilePicture != null && !this.allowedExtensions.Any(ae => input.ProfilePicture.FileName.EndsWith(ae)))
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.FileExtensionNotSupported);
                input.ImageExtensions = this.allowedExtensions;
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            input.ApplicationUserId = user.Id;

            var candidateId = await this.candidatesService.CreateProfileAsync(input);
            if (candidateId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            user.CandidateId = candidateId;
            await this.userManager.UpdateAsync(user);
            this.TempData["Success"] = GlobalConstants.ProfileSuccessfullyCreated;
            return this.RedirectToAction("Upload", "Documents");
        }

        public IActionResult UpdateProfile()
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction(nameof(this.CreateProfile));
            }

            var details = this.candidatesService.GetProfileDetails<UpdateCandidateProfileViewModel>(candidateId);
            if (details == null)
            {
                return this.NotFound();
            }

            details.ImageExtensions = this.allowedExtensions;
            details.LanguagesList = this.languages;
            details.SkillsList = this.skills;
            return this.View(details);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateCandidateProfileViewModel input)
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction(nameof(this.CreateProfile));
            }

            if (!this.ModelState.IsValid)
            {
                input.ImageExtensions = this.allowedExtensions;
                input.LanguagesList = this.languages;
                input.SkillsList = this.skills;
                return this.View(input);
            }

            if (input.ProfilePicture != null && !this.allowedExtensions.Any(ae => input.ProfilePicture.FileName.EndsWith(ae)))
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.FileExtensionNotSupported);
                input.ImageExtensions = this.allowedExtensions;
                return this.View(input);
            }

            var updateResult = await this.candidatesService.UpdateProfileAsync(candidateId, input);

            if (updateResult == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            this.TempData["Success"] = GlobalConstants.ProfileSuccessfullyUpdated;
            return this.RedirectToAction(nameof(this.MyProfile));
        }
    }
}
