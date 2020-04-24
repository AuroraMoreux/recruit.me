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
    using RecruitMe.Web.Infrastructure.Attributes;
    using RecruitMe.Web.ViewModels.Employers;
    using RecruitMe.Web.ViewModels.JobOffers;

    [Authorize]
    public class JobOffersController : BaseController
    {
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

        public async Task<IActionResult> All([FromQuery]FilterModel filters, string sortOrder, int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Search));
            }

            var totalJobOffersCount = this.jobOffersService.GetCount();
            var filteredJobOffers = await this.jobOffersService.GetAllValidFilteredOffers<JobOffersViewModel>(filters);

            if (filteredJobOffers == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            if (filteredJobOffers.Count() != totalJobOffersCount)
            {
                this.ViewData["IsFromSearch"] = "Search Results";
            }

            this.ViewData["DateSortParam"] = string.IsNullOrEmpty(sortOrder) ? "date_asc" : string.Empty;
            this.ViewData["LevelSortParam"] = sortOrder == "max_level" ? "min_level" : "max_level";

            filteredJobOffers = sortOrder switch
            {
                "max_level" => filteredJobOffers.OrderByDescending(jo => jo.JobLevelId),
                "min_level" => filteredJobOffers.OrderBy(jo => jo.JobLevelId),
                "date_asc" => filteredJobOffers.OrderBy(jo => jo.CreatedOn),
                _ => filteredJobOffers.OrderByDescending(jo => jo.CreatedOn)
            };

            var pagesCount = (int)Math.Ceiling(filteredJobOffers.Count() / (decimal)perPage);

            var paginatedOffers = filteredJobOffers
             .Skip(perPage * (page - 1))
             .Take(perPage)
             .ToList();

            var viewModel = new AllViewModel
            {
                JobOffers = paginatedOffers,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        public IActionResult Search()
        {
            var filterModel = new FilterModel
            {
                Levels = this.jobLevels,
                Sectors = this.jobSectors,
                Types = this.jobTypes,
                Skills = this.skills,
                Languages = this.languages,
            };

            return this.View(filterModel);
        }

        public IActionResult Details(string id)
        {
            var jobOfferDetails = this.jobOffersService.GetDetails<JobOfferDetailsViewModel>(id);

            if (jobOfferDetails == null)
            {
                return this.NotFound();
            }

            return this.View(jobOfferDetails);
        }

        [AuthorizeRoles(GlobalConstants.EmployerRoleName, GlobalConstants.AdministratorRoleName)]
        public IActionResult Post()
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            var model = new PostViewModel
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
        [AuthorizeRoles(GlobalConstants.EmployerRoleName, GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Post(PostViewModel input)
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            var isOfferPositionDuplicate = this.jobOffersService.IsPositionDuplicate(employerId, input.Position);
            if (isOfferPositionDuplicate)
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

            var jobOfferId = await this.jobOffersService.AddAsync(input, employerId);

            if (jobOfferId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            this.TempData["Success"] = GlobalConstants.JobOfferSuccessfullyPosted;
            return this.RedirectToAction(nameof(this.All));
        }

        [AuthorizeRoles(GlobalConstants.EmployerRoleName, GlobalConstants.AdministratorRoleName)]
        public IActionResult Edit(string id)
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            var isOfferPostedByEmployer = this.jobOffersService.IsOfferPostedByEmployer(id, employerId);
            if (!isOfferPostedByEmployer)
            {
                return this.Forbid();
            }

            var jobOffer = this.jobOffersService.GetDetails<EditJobOfferDetailsModel>(id);

            if (jobOffer == null)
            {
                return this.NotFound();
            }

            var viewModel = new EditViewModel
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
        [AuthorizeRoles(GlobalConstants.EmployerRoleName, GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(EditViewModel input)
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            var isOfferPostedByEmployer = this.jobOffersService.IsOfferPostedByEmployer(input.JobOfferDetails.Id, employerId);
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

            var jobOfferId = await this.jobOffersService.Update(input, employerId);

            if (jobOfferId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            this.TempData["Success"] = GlobalConstants.JobOfferSuccessfullyUpdated;

            return this.RedirectToAction(nameof(this.All));
        }

        [AuthorizeRoles(GlobalConstants.EmployerRoleName, GlobalConstants.AdministratorRoleName)]
        public IActionResult MyOffers(string sortOrder, int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            var employerOffers = this.jobOffersService.GetEmployerJobOffers<EmployerJobOffersViewModel>(employerId);

            this.ViewData["DateFromSortParam"] = string.IsNullOrEmpty(sortOrder) ? "dateFrom_asc" : string.Empty;
            this.ViewData["DateUntilSortParam"] = sortOrder == "dateUntil_asc" ? "dateUntil" : "dateUntil_asc";
            this.ViewData["IsActiveSortParam"] = sortOrder == "is_inactive" ? "is_active" : "is_inactive";
            this.ViewData["LevelSortParam"] = sortOrder == "max_level" ? "min_level" : "max_level";
            this.ViewData["CountSortParam"] = sortOrder == "max_count" ? "min_count" : "max_count";
            this.ViewData["PositionSortParam"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";

            employerOffers = sortOrder switch
            {
                "name_desc" => employerOffers.OrderByDescending(jo => jo.Position),
                "name_asc" => employerOffers.OrderBy(jo => jo.Position),
                "max_count" => employerOffers.OrderByDescending(jo => jo.JobApplicationsCount),
                "min_count" => employerOffers.OrderBy(jo => jo.JobApplicationsCount),
                "max_level" => employerOffers.OrderByDescending(jo => jo.JobLevelId),
                "min_level" => employerOffers.OrderBy(jo => jo.JobLevelId),
                "is_inactive" => employerOffers.OrderByDescending(jo => jo.IsActive),
                "is_active" => employerOffers.OrderBy(jo => jo.IsActive),
                "dateUntil_asc" => employerOffers.OrderBy(jo => jo.ValidUntil),
                "dateUntil" => employerOffers.OrderByDescending(jo => jo.ValidUntil),
                "dateFrom_asc" => employerOffers.OrderBy(jo => jo.ValidFrom),
                _ => employerOffers.OrderByDescending(jo => jo.ValidFrom),
            };

            var pagesCount = (int)Math.Ceiling(employerOffers.Count() / (decimal)perPage);

            IEnumerable<EmployerJobOffersViewModel> paginatedOffers = employerOffers
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            var viewModel = new AllEmployerOffersViewModel
            {
                Offers = paginatedOffers,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        public IActionResult ByEmployer(string id, string sortOrder, int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            var offers = this.jobOffersService.GetEmployerJobOffers<JobOffersViewModel>(id);
            this.ViewData["DateSortParam"] = string.IsNullOrEmpty(sortOrder) ? "date_asc" : string.Empty;

            offers = sortOrder switch
            {
                "date_asc" => offers.OrderBy(jo => jo.CreatedOn),
                _ => offers.OrderByDescending(jo => jo.CreatedOn)
            };

            var pagesCount = (int)Math.Ceiling(offers.Count() / (decimal)perPage);
            IEnumerable<JobOffersViewModel> paginatedOffers = offers
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();
            var employerName = this.employersService.GetEmployerNameById(id);

            var viewModel = new ByEmployerViewModel
            {
                Name = employerName,
                JobOffers = paginatedOffers,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [AuthorizeRoles(GlobalConstants.EmployerRoleName, GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(string id)
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            var isOfferPostedByEmployer = this.jobOffersService.IsOfferPostedByEmployer(id, employerId);
            if (!isOfferPostedByEmployer)
            {
                return this.Forbid();
            }

            var deleteResult = await this.jobOffersService.DeleteAsync(id);
            if (deleteResult == false)
            {
                return this.NotFound();
            }

            this.TempData["Success"] = GlobalConstants.JobOfferSuccessfullyUpdated;
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
