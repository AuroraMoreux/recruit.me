namespace RecruitMe.Web.Controllers
{
    using System.Collections.Generic;
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

            string jobOfferBaseUrl = $"{this.Request.Scheme}://{this.Request.Host}/JobApplications/Details/";
            string jobApplicationId = await this.jobApplicationService.Apply(input, jobOfferBaseUrl);

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
            if (!isUserRelatedToApplication || !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
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

        [HttpPost]
        public async Task<IActionResult> StatusChange(int statusId, string jobApplicationId)
        {
            string userId = this.userManager.GetUserId(this.User);

            bool isUserRelatedToApplication = this.jobApplicationService.IsUserRelatedToJobApplication(jobApplicationId, userId);
            if (!isUserRelatedToApplication || !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
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
                int newStatus = await this.jobApplicationService.ChangeJobApplicationStatus(jobApplicationId, statusId);

                if (newStatus < 0)
                {
                    return this.RedirectToAction("Error", "Home");
                }
            }

            this.TempData["Success"] = GlobalConstants.JobApplicationStatusSuccessfullyChanged;
            return this.RedirectToAction(nameof(this.Details), new { id = jobApplicationId });
        }
    }
}
