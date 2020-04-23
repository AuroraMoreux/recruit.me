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
    using RecruitMe.Web.ViewModels.Employers;
    using RecruitMe.Web.ViewModels.Shared;

    [AuthorizeRoles(GlobalConstants.EmployerRoleName, GlobalConstants.AdministratorRoleName)]
    public class EmployersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmployersService employersService;
        private readonly IJobSectorsService jobSectorsService;
        private readonly IFileExtensionsService fileExtensionsService;
        private readonly IJobOffersService jobOffersService;
        private IEnumerable<JobSectorsDropDownViewModel> jobSectors;
        private IEnumerable<string> allowedExtensions;

        public EmployersController(
            UserManager<ApplicationUser> userManager,
            IEmployersService employerService,
            IJobSectorsService jobSectorsService,
            IFileExtensionsService fileExtensionsService,
            IJobOffersService jobOffersService)
        {
            this.userManager = userManager;
            this.employersService = employerService;
            this.jobSectorsService = jobSectorsService;
            this.fileExtensionsService = fileExtensionsService;
            this.jobOffersService = jobOffersService;
            this.jobSectors = this.jobSectorsService.GetAll<JobSectorsDropDownViewModel>();

            this.allowedExtensions = this.fileExtensionsService.GetImageExtensions();
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                this.HttpContext.Session.SetString("UserRole", GlobalConstants.EmployerRoleName);
            }

            var viewModel = new IndexViewModel();

            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);

            if (employerId != null)
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
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var viewModel = this.employersService.GetProfileDetails<ProfileViewModel>(employerId);

            return this.View(viewModel);
        }

        public IActionResult CreateProfile()
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId != null)
            {
                return this.RedirectToAction(nameof(this.UpdateProfile));
            }

            var model = new CreateEmployerProfileInputModel
            {
                ImageExtensions = this.fileExtensionsService.GetImageExtensions(),
                JobSectors = this.jobSectors,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateEmployerProfileInputModel input)
        {
            if (this.employersService.GetEmployerIdByUsername(this.User.Identity.Name) != null)
            {
                return this.RedirectToAction(nameof(this.UpdateProfile));
            }

            if (!this.ModelState.IsValid)
            {
                input.ImageExtensions = this.fileExtensionsService.GetImageExtensions();
                input.JobSectors = this.jobSectors;
                return this.View(input);
            }

            if (input.Logo != null && !this.allowedExtensions.Any(ae => input.Logo.FileName.EndsWith(ae)))
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.FileExtensionNotSupported);
                input.ImageExtensions = this.allowedExtensions;
                input.JobSectors = this.jobSectors;
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            input.ApplicationUserId = user.Id;

            var employerId = await this.employersService.CreateProfileAsync(input);

            if (employerId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            user.EmployerId = employerId;
            await this.userManager.UpdateAsync(user);
            this.TempData["Success"] = GlobalConstants.ProfileSuccessfullyCreated;
            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult UpdateProfile()
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction(nameof(this.CreateProfile));
            }

            var details = this.employersService.GetProfileDetails<UpdateEmployerProfileViewModel>(employerId);
            if (details == null)
            {
                return this.NotFound();
            }

            details.JobSectors = this.jobSectors;
            details.ImageExtensions = this.fileExtensionsService.GetImageExtensions();
            return this.View(details);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateEmployerProfileViewModel input)
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction(nameof(this.CreateProfile));
            }

            if (!this.ModelState.IsValid)
            {
                input.ImageExtensions = this.fileExtensionsService.GetImageExtensions();
                input.JobSectors = this.jobSectors;
                return this.View(input);
            }

            if (input.Logo != null && !this.allowedExtensions.Any(ae => input.Logo.FileName.EndsWith(ae)))
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.FileExtensionNotSupported);
                input.ImageExtensions = this.allowedExtensions;
                input.JobSectors = this.jobSectors;
                return this.View(input);
            }

            var updateResult = await this.employersService.UpdateProfileAsync(employerId, input);

            if (employerId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            this.TempData["Success"] = GlobalConstants.ProfileSuccessfullyUpdated;
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
