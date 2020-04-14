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

        [Authorize(Roles = GlobalConstants.CandidateRoleName)]
        public IActionResult Apply(string jobOfferId)
        {
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            CandidateContactDetailsViewModel candidateDetails = this.candidatesService.GetProfileDetails<CandidateContactDetailsViewModel>(candidateId);
            IEnumerable<CandidateDocumentsDropDownViewModel> candidateDocumentDetails = this.documentsService.GetAllDocumentsForCandidate<CandidateDocumentsDropDownViewModel>(candidateId);
            JobApplicationJobOfferDetailsViewModel jobOfferDetails = this.jobOffersService.GetOfferDetails<JobApplicationJobOfferDetailsViewModel>(jobOfferId);

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
        [Authorize(Roles = GlobalConstants.CandidateRoleName)]
        public async Task<IActionResult> Apply(ApplyViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                CandidateContactDetailsViewModel candidateDetails = this.candidatesService.GetProfileDetails<CandidateContactDetailsViewModel>(input.CandidateId);
                IEnumerable<CandidateDocumentsDropDownViewModel> candidateDocumentDetails = this.documentsService.GetAllDocumentsForCandidate<CandidateDocumentsDropDownViewModel>(input.CandidateId);
                JobApplicationJobOfferDetailsViewModel jobOfferDetails = this.jobOffersService.GetOfferDetails<JobApplicationJobOfferDetailsViewModel>(input.JobOfferId);

                return this.View(input);
            }

            if (this.jobApplicationService.HasCandidateAppliedForOffer(input.CandidateId, input.JobOfferId))
            {
                this.TempData["AlreadyAppliedMessage"] = GlobalConstants.CandidateAlreadyApplied;
                return this.RedirectToAction("Details", "JobOffers", new { jobOfferId = input.JobOfferId });
            }

            string jobOfferBaseUrl = $"{this.Request.Scheme}://{this.Request.Host}/JobApplications/Details/";
            string jobApplicationId = await this.jobApplicationService.Apply(input, jobOfferBaseUrl);

            if (jobApplicationId == null)
            {
                return this.View("Error");
            }
            else
            {
                this.TempData["SuccessfulApplication"] = GlobalConstants.JobApplicationSuccessfullySubmitted;
                return this.RedirectToAction("Details", "JobOffers", new { id = input.JobOfferId });
            }
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            string userId = this.userManager.GetUserId(this.User);

            var isUserRelatedToApplication = this.jobApplicationService.IsUserRelatedToJobApplication(id, userId);
            if (!isUserRelatedToApplication)
            {
                return this.Forbid();
            }

            var jobApplicationDetails = this.jobApplicationService.GetJobApplicationDetails<DetailsViewModel>(id);
            jobApplicationDetails.JobApplicationStatusChangeList = this.jobApplicationStatusesService.GetAll<JobApplicationStatusDetailsView>();
            return this.View(jobApplicationDetails);
        }

        public async Task<IActionResult> StatusChange(int statusId, string jobApplicationId)
        {
            var currentStatusId = this.jobApplicationService.GetJobApplicationStatusId(jobApplicationId);

            if (currentStatusId == statusId)
            {
                return this.RedirectToAction(nameof(this.Details), new { id = jobApplicationId });
            }
            else
            {
                var newStatus = await this.jobApplicationService.ChangeJobApplicationStatus(jobApplicationId, statusId);

                if (newStatus == currentStatusId)
                {
                    return this.View("Error");
                }
            }

            return this.RedirectToAction(nameof(this.Details), new { id = jobApplicationId });
        }
    }
}
