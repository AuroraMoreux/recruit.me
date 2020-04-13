namespace RecruitMe.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.JobApplications;

    public class JobApplicationsController : BaseController
    {
        private readonly IJobApplicationService jobApplicationService;
        private readonly IJobOffersService jobOffersService;
        private readonly ICandidatesService candidatesService;
        private readonly IEmployersService employersService;
        private readonly IDocumentsService documentsService;

        public JobApplicationsController(IJobApplicationService jobApplicationService, IJobOffersService jobOffersService, ICandidatesService candidatesService, IEmployersService employersService, IDocumentsService documentsService)
        {
            this.jobApplicationService = jobApplicationService;
            this.jobOffersService = jobOffersService;
            this.candidatesService = candidatesService;
            this.employersService = employersService;
            this.documentsService = documentsService;
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

        [Authorize(Roles = GlobalConstants.EmployerRoleName)]
        public IActionResult Details(string id)
        {
            var employerId = this.employersService.GetEmployerIdByUsername(this.User.Identity.Name);
            var jobApplicationJobOfferId = this.jobApplicationService.GetJobOfferIdForApplication(id);
            var isJobOfferPostedByEmployer = this.jobOffersService.IsJobOfferPostedByEmployer(jobApplicationJobOfferId, employerId);

            if (!isJobOfferPostedByEmployer)
            {
                return this.Forbid();
            }

            var jobApplicationDetails = this.jobApplicationService.GetJobApplicationDetails<DetailsViewModel>(id);
            return this.View(jobApplicationDetails);
        }

        public IActionResult Download()
        {
            // TODO
            return this.View();
        }
    }
}
