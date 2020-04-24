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
    using RecruitMe.Web.ViewModels.JobApplications;

    [Authorize]
    public class JobApplicationsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJobApplicationService jobApplicationService;
        private readonly IJobOffersService jobOffersService;
        private readonly ICandidatesService candidatesService;
        private readonly IEmployersService employersService;
        private readonly IDocumentsService documentsService;
        private readonly IJobApplicationStatusesService jobApplicationStatusesService;

        public JobApplicationsController(UserManager<ApplicationUser> userManager, IJobApplicationService jobApplicationService, IJobOffersService jobOffersService, ICandidatesService candidatesService, IEmployersService employersService, IDocumentsService documentsService, IJobApplicationStatusesService jobApplicationStatusesService)
        {
            this.userManager = userManager;
            this.jobApplicationService = jobApplicationService;
            this.jobOffersService = jobOffersService;
            this.candidatesService = candidatesService;
            this.employersService = employersService;
            this.documentsService = documentsService;
            this.jobApplicationStatusesService = jobApplicationStatusesService;
        }

        [AuthorizeRoles(GlobalConstants.CandidateRoleName, GlobalConstants.AdministratorRoleName)]
        public IActionResult Apply(string jobOfferId)
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            var candidateDetails = this.candidatesService.GetProfileDetails<CandidateContactDetailsViewModel>(candidateId);
            var candidateDocumentDetails = this.documentsService.GetAllDocumentsForCandidate<CandidateDocumentsDropDownViewModel>(candidateId);
            var jobOfferDetails = this.jobOffersService.GetDetails<JobApplicationJobOfferDetailsViewModel>(jobOfferId);

            if (candidateDetails == null || candidateDocumentDetails == null || jobOfferDetails == null)
            {
                return this.NotFound();
            }

            var viewModel = new ApplyViewModel
            {
                CandidateDetails = candidateDetails,
                Documents = candidateDocumentDetails,
                JobOfferDetails = jobOfferDetails,
                JobOfferId = jobOfferId,
                CandidateId = candidateId,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [AuthorizeRoles(GlobalConstants.CandidateRoleName, GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Apply(ApplyViewModel input)
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            if (!this.ModelState.IsValid)
            {
                var candidateDetails = this.candidatesService.GetProfileDetails<CandidateContactDetailsViewModel>(input.CandidateId);
                var candidateDocumentDetails = this.documentsService.GetAllDocumentsForCandidate<CandidateDocumentsDropDownViewModel>(input.CandidateId);
                var jobOfferDetails = this.jobOffersService.GetDetails<JobApplicationJobOfferDetailsViewModel>(input.JobOfferId);

                input.CandidateDetails = candidateDetails;
                input.Documents = candidateDocumentDetails;
                input.JobOfferDetails = jobOfferDetails;
                return this.View(input);
            }

            if (this.jobApplicationService.HasCandidateAppliedForOffer(input.CandidateId, input.JobOfferId))
            {
                this.TempData["AlreadyAppliedMessage"] = GlobalConstants.CandidateAlreadyApplied;
                return this.RedirectToAction(nameof(this.All));
            }

            var jobApplicationBaseUrl = $"{this.Request.Scheme}://{this.Request.Host}/JobApplications/Details/";
            var jobApplicationId = await this.jobApplicationService.Apply(input, jobApplicationBaseUrl);

            if (jobApplicationId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }
            else
            {
                this.TempData["SuccessfulApplication"] = GlobalConstants.JobApplicationSuccessfullySubmitted;
                return this.RedirectToAction(nameof(this.All));
            }
        }

        public IActionResult Details(string id)
        {
            var userId = this.userManager.GetUserId(this.User);

            var isUserRelatedToApplication = this.jobApplicationService.IsUserRelatedToJobApplication(id, userId);
            if (!isUserRelatedToApplication && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Forbid();
            }

            var jobApplicationDetails = this.jobApplicationService.GetJobApplicationDetails<DetailsViewModel>(id);

            if (jobApplicationDetails == null)
            {
                return this.NotFound();
            }

            jobApplicationDetails.JobApplicationStatusChangeList = this.jobApplicationStatusesService.GetAll<JobApplicationStatusDetailsView>();
            return this.View(jobApplicationDetails);
        }

        [HttpGet]
        public async Task<IActionResult> StatusChange(int statusId, string jobApplicationId)
        {
            var userId = this.userManager.GetUserId(this.User);

            var isUserRelatedToApplication = this.jobApplicationService.IsUserRelatedToJobApplication(jobApplicationId, userId);
            if (!isUserRelatedToApplication && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Forbid();
            }

            var currentStatusId = this.jobApplicationService.GetJobApplicationStatusId(jobApplicationId);

            if (currentStatusId == statusId)
            {
                return this.RedirectToAction(nameof(this.Details), new { id = jobApplicationId });
            }
            else
            {
                var jobApplicationBaseUrl = $"{this.Request.Scheme}://{this.Request.Host}/JobApplications/Details/";
                var newStatus = await this.jobApplicationService.ChangeJobApplicationStatus(jobApplicationId, statusId, jobApplicationBaseUrl);

                if (newStatus < 0)
                {
                    return this.RedirectToAction("Error", "Home");
                }
            }

            this.TempData["Success"] = GlobalConstants.JobApplicationStatusSuccessfullyChanged;
            return this.RedirectToAction(nameof(this.Details), new { id = jobApplicationId });
        }

        [AuthorizeRoles(GlobalConstants.CandidateRoleName, GlobalConstants.AdministratorRoleName)]
        public IActionResult All(string sortOrder, int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            var candidateApplications = this.jobApplicationService.GetCandidateApplications<CandidateJobApplicationsViewModel>(candidateId);

            this.ViewData["DateSortParam"] = string.IsNullOrEmpty(sortOrder) ? "date_asc" : string.Empty;
            this.ViewData["StatusSortParam"] = sortOrder == "max_status" ? "min_status" : "max_status";
            this.ViewData["PositionSortParam"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";

            candidateApplications = sortOrder switch
            {
                "name_desc" => candidateApplications.OrderByDescending(ja => ja.JobOfferPosition),
                "name_asc" => candidateApplications.OrderBy(ja => ja.JobOfferPosition),
                "max_status" => candidateApplications.OrderByDescending(ja => ja.ApplicationStatusId),
                "min_status" => candidateApplications.OrderBy(ja => ja.ApplicationStatusId),
                "date_asc" => candidateApplications.OrderBy(ja => ja.ApplicationDate),
                _ => candidateApplications.OrderByDescending(ja => ja.ApplicationDate)
            };

            var pagesCount = (int)Math.Ceiling(candidateApplications.Count() / (decimal)perPage);

            IEnumerable<CandidateJobApplicationsViewModel> paginatedApplications = candidateApplications
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            var viewModel = new AllViewModel
            {
                JobApplications = paginatedApplications,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        [AuthorizeRoles(GlobalConstants.EmployerRoleName, GlobalConstants.AdministratorRoleName)]
        public IActionResult ByOffer(string jobOfferId, string sortOrder, int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            var isOfferPostedByEmployer = this.jobOffersService.IsOfferPostedByEmployer(jobOfferId, employerId);
            if (!isOfferPostedByEmployer)
            {
                return this.Forbid();
            }

            var jobOfferApplications = this.jobApplicationService.GetApplicationsForOffer<JobOfferJobApplicationsViewModel>(jobOfferId);
            this.ViewData["DateSortParam"] = string.IsNullOrEmpty(sortOrder) ? "date_asc" : string.Empty;
            this.ViewData["StatusSortParam"] = sortOrder == "max_status" ? "min_status" : "max_status";
            this.ViewData["EducationSortParam"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";

            jobOfferApplications = sortOrder switch
            {
                "name_desc" => jobOfferApplications.OrderByDescending(ja => ja.CandidateEducation),
                "name_asc" => jobOfferApplications.OrderBy(ja => ja.CandidateEducation),
                "max_status" => jobOfferApplications.OrderByDescending(ja => ja.ApplicationStatusId),
                "min_status" => jobOfferApplications.OrderBy(ja => ja.ApplicationStatusId),
                "date_asc" => jobOfferApplications.OrderBy(ja => ja.CreatedOn),
                _ => jobOfferApplications.OrderByDescending(ja => ja.CreatedOn)
            };

            var pagesCount = (int)Math.Ceiling(jobOfferApplications.Count() / (decimal)perPage);

            IEnumerable<JobOfferJobApplicationsViewModel> paginatedApplications = jobOfferApplications
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            var getOfferPosition = this.jobOffersService.GetOfferPositionById(jobOfferId);

            var viewModel = new JobOfferAllApplicationsModel
            {
                JobOfferId = jobOfferId,
                Position = getOfferPosition,
                JobApplications = paginatedApplications,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }
    }
}
