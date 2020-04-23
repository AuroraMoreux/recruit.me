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
    using RecruitMe.Services;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.Infrastructure.Attributes;
    using RecruitMe.Web.ViewModels.Documents;

    public class DocumentsController : BaseController
    {
        private readonly IDocumentsService documentsService;
        private readonly ICandidatesService candidatesService;
        private readonly IFileExtensionsService fileExtensionsService;
        private readonly IDocumentCategoriesService documentCategoriesService;
        private readonly IJobApplicationService jobApplicationService;
        private readonly IMimeMappingService mimeMappingService;
        private readonly UserManager<ApplicationUser> userManager;

        private IEnumerable<string> allowedExtensions;
        private IEnumerable<CategoriesDropDownViewModel> allowedCategories;

        public DocumentsController(
            IDocumentsService documentsService,
            ICandidatesService candidatesService,
            IFileExtensionsService fileExtensionsService,
            IDocumentCategoriesService documentCategoriesService,
            IJobApplicationService jobApplicationService,
            IMimeMappingService mimeMappingService,
            UserManager<ApplicationUser> userManager)
        {
            this.documentsService = documentsService;
            this.candidatesService = candidatesService;
            this.fileExtensionsService = fileExtensionsService;
            this.documentCategoriesService = documentCategoriesService;
            this.jobApplicationService = jobApplicationService;
            this.mimeMappingService = mimeMappingService;
            this.userManager = userManager;

            this.allowedExtensions = this.fileExtensionsService.GetAll();
            this.allowedCategories = this.documentCategoriesService.GetAll<CategoriesDropDownViewModel>();
        }

        [AuthorizeRoles(GlobalConstants.CandidateRoleName, GlobalConstants.AdministratorRoleName)]
        public IActionResult All(int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            var documents = this.documentsService.GetAllDocumentsForCandidate<DocumentsViewModel>(candidateId);

            var pagesCount = (int)Math.Ceiling(documents.Count() / (decimal)perPage);

            var paginatedDocuments = documents
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            var viewModel = new CandidateDocumentsViewModel
            {
                Documents = paginatedDocuments,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        [AuthorizeRoles(GlobalConstants.CandidateRoleName, GlobalConstants.AdministratorRoleName)]
        public IActionResult Upload()
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);

            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            var viewModel = new UploadInputModel
            {
                FileExtensions = this.allowedExtensions,
                Categories = this.allowedCategories,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [AuthorizeRoles(GlobalConstants.CandidateRoleName, GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Upload(UploadInputModel input)
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            if (!this.ModelState.IsValid)
            {
                input.FileExtensions = this.allowedExtensions;
                input.Categories = this.allowedCategories;
                return this.View(input);
            }

            if (this.documentsService.DocumentNameAlreadyExists(input.SanitizedFileName))
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.DocumentAlreadyExists);
                input.FileExtensions = this.allowedExtensions;
                input.Categories = this.allowedCategories;
                return this.View(input);
            }

            if (!this.allowedExtensions.Any(ae => input.File.FileName.EndsWith(ae)))
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.FileExtensionNotSupported);
                return this.View(input);
            }

            var documentId = await this.documentsService.Upload(input, candidateId);

            if (documentId == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            this.TempData["Success"] = GlobalConstants.DocumentSuccessfullyUploaded;
            return this.RedirectToAction(nameof(this.All));
        }

        [HttpGet]
        [AuthorizeRoles(GlobalConstants.CandidateRoleName, GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(string id)
        {
            var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            var isOwner = this.documentsService.IsCandidateOwnerOfDocument(candidateId, id);

            if (!isOwner)
            {
                return this.Forbid();
            }

            var deletionResult = await this.documentsService.Delete(id);

            if (deletionResult == false)
            {
                return this.NotFound();
            }

            this.TempData["Success"] = GlobalConstants.DocumentSuccessfullyDeleted;
            return this.RedirectToAction(nameof(this.All));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Download(string id, string jobApplicationId = null)
        {
            if (jobApplicationId == null)
            {
                var candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
                if (candidateId == null)
                {
                    return this.RedirectToAction("CreateProfile", "Candidates");
                }

                var isOwner = this.documentsService.IsCandidateOwnerOfDocument(candidateId, id);
                if (!isOwner)
                {
                    return this.Forbid();
                }
            }
            else
            {
                var userId = this.userManager.GetUserId(this.User);

                var isUserRelatedToJobApplication = this.jobApplicationService.IsUserRelatedToJobApplication(jobApplicationId, userId);
                if (!isUserRelatedToJobApplication)
                {
                    return this.Forbid();
                }
            }

            var fileAsByteArray = await this.documentsService.Download(id);
            if (fileAsByteArray == null)
            {
                return this.NotFound();
            }

            var fileName = this.documentsService.GetDocumentNameById(id);
            var contentType = this.mimeMappingService.Map(fileName);

            return this.File(fileAsByteArray, contentType, fileName);
        }
    }
}
