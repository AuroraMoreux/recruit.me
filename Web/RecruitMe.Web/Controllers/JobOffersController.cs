namespace RecruitMe.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Employers;
    using RecruitMe.Web.ViewModels.JobOffers;

    public class JobOffersController : BaseController
    {
        private const int OffersPerPageDefaultCount = 10;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJobOffersService jobOffersService;
        private readonly IEmployersService employersService;
        private readonly IJobSectorsService jobSectorsService;
        private readonly IJobLevelsService jobLevelsService;
        private readonly IJobTypesService jobTypesService;
        private readonly ILanguagesService languagesService;
        private readonly ISkillsService skillsService;

        private readonly IEnumerable<JobSectorsDropDownViewModel> jobSectors;
        private readonly IEnumerable<JobLevelsDropDownViewModel> jobLevels;
        private readonly List<JobTypesDropDownCheckboxListViewModel> jobTypes;
        private readonly IEnumerable<LanguagesDropDownCheckboxListViewModel> languages;
        private readonly IEnumerable<SkillsDropDownCheckboxListViewModel> skills;

        public JobOffersController(
            UserManager<ApplicationUser> userManager,
            IJobOffersService jobOffersService,
            IEmployersService employersService,
            IJobSectorsService jobSectorsService,
            IJobLevelsService jobLevelsService,
            IJobTypesService jobTypesService,
            ILanguagesService languagesService,
            ISkillsService skillsService)
        {
            this.userManager = userManager;
            this.jobOffersService = jobOffersService;
            this.employersService = employersService;
            this.jobSectorsService = jobSectorsService;
            this.jobLevelsService = jobLevelsService;
            this.jobTypesService = jobTypesService;
            this.languagesService = languagesService;
            this.skillsService = skillsService;

            this.jobSectors = this.jobSectorsService.GetAll<JobSectorsDropDownViewModel>();
            this.jobLevels = this.jobLevelsService.GetAll<JobLevelsDropDownViewModel>();
            this.jobTypes = this.jobTypesService.GetAll<JobTypesDropDownCheckboxListViewModel>().ToList();
            this.languages = this.languagesService.GetAll<LanguagesDropDownCheckboxListViewModel>();
            this.skills = this.skillsService.GetAll<SkillsDropDownCheckboxListViewModel>();
        }

        [Authorize]
        public IActionResult All([FromQuery]FilterModel filters, string dateSortOrder, int page = 1, int perPage = OffersPerPageDefaultCount)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Search));
            }

            int totalJobOffersCount = this.jobOffersService.GetCount();
            IEnumerable<JobOffersViewModel> filteredJobOffers = this.jobOffersService.GetAllValidFilteredOffers<JobOffersViewModel>(filters);

            if (filteredJobOffers.Count() != totalJobOffersCount)
            {
                this.ViewData["IsFromSearch"] = "Search Results";
            }

            switch (dateSortOrder)
            {
                case "date_asc":
                    filteredJobOffers = filteredJobOffers.OrderBy(jo => jo.CreatedOn);
                    this.ViewData["DateSortParam"] = "date";
                    break;
                default:
                    filteredJobOffers = filteredJobOffers.OrderByDescending(jo => jo.CreatedOn);
                    this.ViewData["DateSortParam"] = "date_asc";
                    break;
            }

            int pagesCount = (int)Math.Ceiling(filteredJobOffers.Count() / (decimal)perPage);

            List<JobOffersViewModel> paginatedOffers = filteredJobOffers
             .Skip(perPage * (page - 1))
             .Take(perPage)
             .ToList();

            AllViewModel viewModel = new AllViewModel
            {
                JobOffers = paginatedOffers,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Search()
        {
            FilterModel filterModel = new FilterModel
            {
                Levels = this.jobLevels,
                Sectors = this.jobSectors,
                Types = this.jobTypes,
                Skills = this.skills,
                Languages = this.languages,
            };

            return this.View(filterModel);
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            JobOfferDetailsViewModel jobOfferDetails = this.jobOffersService.GetDetails<JobOfferDetailsViewModel>(id);
            return this.View(jobOfferDetails);
        }

        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public IActionResult Post()
        {
            string employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            PostViewModel model = new PostViewModel
            {
                JobSectors = this.jobSectors,
                JobLevels = this.jobLevels,
                JobTypesOptions = this.jobTypes,
                Languages = this.languages,
                Skills = this.skills,
            };
            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public async Task<IActionResult> Post(PostViewModel input)
        {
            string employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            bool isOfferTitleDuplicate = this.jobOffersService.IsTitleDuplicate(employerId, input.Title);
            if (isOfferTitleDuplicate)
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.JobOfferWithSameNameAlreadyExists);
            }

            if (!this.ModelState.IsValid)
            {
                input.JobSectors = this.jobSectors;
                input.JobLevels = this.jobLevels;
                input.JobTypesOptions = this.jobTypes;
                input.Languages = this.languages;
                input.Skills = this.skills;

                return this.View(input);
            }

            string jobOfferId = await this.jobOffersService.Add(input, employerId);

            if (jobOfferId == null)
            {
                return this.View("Error");
            }

            this.TempData["JobOfferPosted"] = GlobalConstants.JobOfferSuccessfullyPosted;
            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public IActionResult Edit(string id)
        {
            string employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            bool isOfferPostedByEmployer = this.jobOffersService.IsOfferPostedByEmployer(id, employerId);
            if (!isOfferPostedByEmployer)
            {
                return this.Forbid();
            }

            EditJobOfferDetailsModel jobOffer = this.jobOffersService.GetDetails<EditJobOfferDetailsModel>(id);
            EditViewModel viewModel = new EditViewModel
            {
                JobOfferDetails = jobOffer,
                JobSectors = this.jobSectors,
                JobLevels = this.jobLevels,
                JobTypesOptions = this.jobTypes,
                Languages = this.languages,
                Skills = this.skills,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public async Task<IActionResult> Edit(EditViewModel input)
        {
            string employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            bool isOfferPostedByEmployer = this.jobOffersService.IsOfferPostedByEmployer(input.JobOfferDetails.Id, employerId);
            if (!isOfferPostedByEmployer)
            {
                return this.Forbid();
            }

            if (!this.ModelState.IsValid)
            {
                input.JobSectors = this.jobSectors;
                input.JobLevels = this.jobLevels;
                input.JobTypesOptions = this.jobTypes;
                input.Languages = this.languages;
                input.Skills = this.skills;
                return this.View(input);
            }

            string jobOfferId = await this.jobOffersService.Update(input, employerId);

            if (jobOfferId == null)
            {
                return this.View("Error");
            }

            this.TempData["JobOfferUpdated"] = GlobalConstants.JobOfferSuccessfullyUpdated;

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public async Task<IActionResult> Delete(string id)
        {
            string employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            bool isOfferPostedByEmployer = this.jobOffersService.IsOfferPostedByEmployer(id, employerId);
            if (!isOfferPostedByEmployer)
            {
                return this.Forbid();
            }

            await this.jobOffersService.Delete(id);

            this.TempData["JobOfferDeleted"] = GlobalConstants.JobOfferSuccessfullyUpdated;
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
