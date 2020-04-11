namespace RecruitMe.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Employers;

    public class EmployersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmployersService employerService;
        private readonly IJobSectorsService jobSectorsService;

        private IEnumerable<JobSectorsDropDownViewModel> jobSectors;

        public EmployersController(UserManager<ApplicationUser> userManager, IEmployersService employerService, IJobSectorsService jobSectorsService)
        {
            this.userManager = userManager;
            this.employerService = employerService;
            this.jobSectorsService = jobSectorsService;

            this.jobSectors = this.jobSectorsService.GetAll<JobSectorsDropDownViewModel>();
        }

        [HttpGet]
        public IActionResult Index()
        {
            this.HttpContext.Session.SetString("UserRole", GlobalConstants.EmployerRoleName);

            return this.View();
        }

        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public async Task<IActionResult> CreateProfile()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);

            if (user.EmployerId != null)
            {
                return this.RedirectToAction(nameof(this.UpdateProfile));
            }

            CreateEmployerProfileInputModel model = new CreateEmployerProfileInputModel
            {
                JobSectors = this.jobSectors,
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public async Task<IActionResult> CreateProfile(CreateEmployerProfileInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.JobSectors = this.jobSectors;
                return this.View(input);
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            input.ApplicationUserId = user.Id;

            string employerId = await this.employerService.CreateProfileAsync(input);

            if (employerId != null)
            {
                user.EmployerId = employerId;
                await this.userManager.UpdateAsync(user);
                this.TempData["InfoMessage"] = GlobalConstants.ProfileSuccessfullyCreated;
                return this.RedirectToAction("Index");
            }

            return this.View("Error");
        }

        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public async Task<IActionResult> UpdateProfile()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);

            if (user.EmployerId == null)
            {
                return this.RedirectToAction(nameof(this.CreateProfile));
            }

            UpdateEmployerProfileViewModel details = this.employerService.GetProfileDetails<UpdateEmployerProfileViewModel>(user.EmployerId);
            details.JobSectors = this.jobSectors;

            return this.View(details);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public async Task<IActionResult> UpdateProfile(UpdateEmployerProfileViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.JobSectors = this.jobSectors;
                return this.View(input);
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            input.ApplicationUserId = user.Id;
            string employerId = await this.employerService.UpdateProfileAsync(user.EmployerId, input);

            if (employerId != null)
            {
                this.TempData["InfoMessage"] = GlobalConstants.ProfileSuccessfullyUpdated;
                return this.RedirectToAction("Index");
            }

            return this.View("Error");
        }
    }
}
