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
    using RecruitMe.Web.ViewModels.Employers;

    public class EmployersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmployersService employerService;

        public IJobSectorsService JobSectorsService { get; }

        public EmployersController(UserManager<ApplicationUser> userManager, IEmployersService employerService, IJobSectorsService jobSectorsService)
        {
            this.userManager = userManager;
            this.employerService = employerService;
            this.JobSectorsService = jobSectorsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            this.HttpContext.Session.SetString("UserRole", GlobalConstants.EmployerRoleName);

            return this.View();
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public async Task<IActionResult> CreateProfile()
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);

            if (user.EmployerId != null)
            {
                return this.RedirectToAction(nameof(this.UpdateProfile));
            }

            var jobSectors = await this.JobSectorsService.GetAllAsync<JobSectorsDropDownViewModel>();

            var model = new CreateEmployerProfileInputModel
            {
                JobSectors = jobSectors,
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public async Task<IActionResult> CreateProfile(CreateEmployerProfileInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            input.ApplicationUserId = user.Id;

            string employerId = await this.employerService.CreateProfileAsync(input);

            if (employerId != null)
            {
                user.EmployerId = employerId;
                await this.userManager.UpdateAsync(user);

                return this.RedirectToAction(nameof(HomeController.Index));
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

            var details = await this.employerService.GetProfileDetailsAsync<CreateEmployerProfileInputModel>(user.EmployerId);

            return this.View(details);
        }
    }
}
