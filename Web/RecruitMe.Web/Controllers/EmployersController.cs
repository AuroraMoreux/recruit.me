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

    [AuthorizeRoles(GlobalConstants.EmployerRoleName, GlobalConstants.AdministratorRoleName)]
    public class EmployersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmployersService employerService;
        private readonly IJobSectorsService jobSectorsService;
        private readonly IFileExtensionsService fileExtensionsService;
        private IEnumerable<JobSectorsDropDownViewModel> jobSectors;
        private IEnumerable<string> allowedExtensions;

        public EmployersController(
            UserManager<ApplicationUser> userManager,
            IEmployersService employerService,
            IJobSectorsService jobSectorsService,
            IFileExtensionsService fileExtensionsService)
        {
            this.userManager = userManager;
            this.employerService = employerService;
            this.jobSectorsService = jobSectorsService;
            this.fileExtensionsService = fileExtensionsService;
            this.jobSectors = this.jobSectorsService.GetAll<JobSectorsDropDownViewModel>();

            this.allowedExtensions = this.fileExtensionsService.GetImageExtensions();
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            this.HttpContext.Session.SetString("UserRole", GlobalConstants.EmployerRoleName);

            return this.View();
        }

        public IActionResult CreateProfile()
        {
            string employerId = this.employerService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId != null)
            {
                return this.RedirectToAction(nameof(this.UpdateProfile));
            }

            CreateEmployerProfileInputModel model = new CreateEmployerProfileInputModel
            {
                ImageExtensions = this.fileExtensionsService.GetImageExtensions(),
                JobSectors = this.jobSectors,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateEmployerProfileInputModel input)
        {
            if (this.employerService.GetEmployerIdByUsername(this.User.Identity.Name) != null)
            {
                return this.RedirectToAction(nameof(this.UpdateProfile));
            }

            if (!this.ModelState.IsValid)
            {
                input.ImageExtensions = this.fileExtensionsService.GetImageExtensions();
                input.JobSectors = this.jobSectors;
                return this.View(input);
            }

            if (!this.allowedExtensions.Any(ae => input.Logo.FileName.EndsWith(ae)))
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.FileExtensionNotSupported);
                input.ImageExtensions = this.allowedExtensions;
                input.JobSectors = this.jobSectors;
                return this.View(input);
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            input.ApplicationUserId = user.Id;

            string employerId = await this.employerService.CreateProfileAsync(input);

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
            string employerId = this.employerService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction(nameof(this.CreateProfile));
            }

            UpdateEmployerProfileViewModel details = this.employerService.GetProfileDetails<UpdateEmployerProfileViewModel>(employerId);
            if (details == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            details.JobSectors = this.jobSectors;
            details.ImageExtensions = this.fileExtensionsService.GetImageExtensions();
            return this.View(details);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateEmployerProfileViewModel input)
        {
            string employerId = this.employerService.GetEmployerIdByUsername(this.User.Identity.Name);
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

            if (!this.allowedExtensions.Any(ae => input.Logo.FileName.EndsWith(ae)))
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.FileExtensionNotSupported);
                input.ImageExtensions = this.allowedExtensions;
                input.JobSectors = this.jobSectors;
                return this.View(input);
            }

            string updateResult = await this.employerService.UpdateProfileAsync(employerId, input);

            if (employerId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            this.TempData["Success"] = GlobalConstants.ProfileSuccessfullyUpdated;
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
