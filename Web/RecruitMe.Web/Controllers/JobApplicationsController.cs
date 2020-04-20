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
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            CandidateContactDetailsViewModel candidateDetails = this.candidatesService.GetProfileDetails<CandidateContactDetailsViewModel>(candidateId);
            IEnumerable<CandidateDocumentsDropDownViewModel> candidateDocumentDetails = this.documentsService.GetAllDocumentsForCandidate<CandidateDocumentsDropDownViewModel>(candidateId);
            JobApplicationJobOfferDetailsViewModel jobOfferDetails = this.jobOffersService.GetDetails<JobApplicationJobOfferDetailsViewModel>(jobOfferId);

            if (candidateDetails == null || candidateDocumentDetails == null || jobOfferDetails == null)
            {
                return this.NotFound();
            }

            ApplyViewModel viewModel = new ApplyViewModel
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
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            if (!this.ModelState.IsValid)
            {
                CandidateContactDetailsViewModel candidateDetails = this.candidatesService.GetProfileDetails<CandidateContactDetailsViewModel>(input.CandidateId);
                IEnumerable<CandidateDocumentsDropDownViewModel> candidateDocumentDetails = this.documentsService.GetAllDocumentsForCandidate<CandidateDocumentsDropDownViewModel>(input.CandidateId);
                JobApplicationJobOfferDetailsViewModel jobOfferDetails = this.jobOffersService.GetDetails<JobApplicationJobOfferDetailsViewModel>(input.JobOfferId);

                input.CandidateDetails = candidateDetails;
                input.Documents = candidateDocumentDetails;
                input.JobOfferDetails = jobOfferDetails;
                return this.View(input);
            }

            if (this.jobApplicationService.HasCandidateAppliedForOffer(input.CandidateId, input.JobOfferId))
            {
                this.TempData["AlreadyAppliedMessage"] = GlobalConstants.CandidateAlreadyApplied;
                return this.RedirectToAction("Details", "JobOffers", new { id = input.JobOfferId });
            }

            string jobApplicationBaseUrl = $"{this.Request.Scheme}://{this.Request.Host}/JobApplications/Details/";
            string jobApplicationId = await this.jobApplicationService.Apply(input, jobApplicationBaseUrl);

            if (jobApplicationId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }
            else
            {
                this.TempData["SuccessfulApplication"] = GlobalConstants.JobApplicationSuccessfullySubmitted;
                return this.RedirectToAction("Details", "JobOffers", new { id = input.JobOfferId });
            }
        }

        public IActionResult Details(string id)
        {
            string userId = this.userManager.GetUserId(this.User);

            bool isUserRelatedToApplication = this.jobApplicationService.IsUserRelatedToJobApplication(id, userId);
            if (!isUserRelatedToApplication && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Forbid();
            }

            DetailsViewModel jobApplicationDetails = this.jobApplicationService.GetJobApplicationDetails<DetailsViewModel>(id);

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
            string userId = this.userManager.GetUserId(this.User);

            bool isUserRelatedToApplication = this.jobApplicationService.IsUserRelatedToJobApplication(jobApplicationId, userId);
            if (!isUserRelatedToApplication && !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Forbid();
            }

            int currentStatusId = this.jobApplicationService.GetJobApplicationStatusId(jobApplicationId);

            if (currentStatusId == statusId)
            {
                return this.RedirectToAction(nameof(this.Details), new { id = jobApplicationId });
            }
            else
            {
                string jobApplicationBaseUrl = $"{this.Request.Scheme}://{this.Request.Host}/JobApplications/Details/";
                int newStatus = await this.jobApplicationService.ChangeJobApplicationStatus(jobApplicationId, statusId, jobApplicationBaseUrl);

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
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            IEnumerable<CandidateJobApplicationsViewModel> candidateApplications = this.jobApplicationService.GetCandidateApplications<CandidateJobApplicationsViewModel>(candidateId);

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

            int pagesCount = (int)Math.Ceiling(candidateApplications.Count() / (decimal)perPage);

            IEnumerable<CandidateJobApplicationsViewModel> paginatedApplications = candidateApplications
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            AllViewModel viewModel = new AllViewModel
            {
                JobApplications = paginatedApplications,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        [AuthorizeRoles(GlobalConstants.EmployerRoleName, GlobalConstants.AdministratorRoleName)]
        public IActionResult ByOffer(string jobOfferId, string sortOrder,int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            string employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            if (employerId == null)
            {
                return this.RedirectToAction("CreateProfile", "Employers");
            }

            bool isOfferPostedByEmployer = this.jobOffersService.IsOfferPostedByEmployer(jobOfferId, employerId);
            if (!isOfferPostedByEmployer)
            {
                return this.Forbid();
            }

            IEnumerable<JobOfferJobApplicationsViewModel> jobOfferApplications = this.jobApplicationService.GetApplicationsForOffer<JobOfferJobApplicationsViewModel>(jobOfferId);
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

            int pagesCount = (int)Math.Ceiling(jobOfferApplications.Count() / (decimal)perPage);

            IEnumerable<JobOfferJobApplicationsViewModel> paginatedApplications = jobOfferApplications
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            string getOfferPosition = this.jobOffersService.GetOfferPositionById(jobOfferId);

            JobOfferAllApplicationsModel viewModel = new JobOfferAllApplicationsModel
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
