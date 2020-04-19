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
        private const int DocumentsPerPageDefaultCount = 10;

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
        public IActionResult All(int page = 1, int perPage = DocumentsPerPageDefaultCount)
        {
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            IEnumerable<DocumentsViewModel> documents = this.documentsService.GetAllDocumentsForCandidate<DocumentsViewModel>(candidateId);

            int pagesCount = (int)Math.Ceiling(documents.Count() / (decimal)perPage);

            List<DocumentsViewModel> paginatedDocuments = documents
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            UserDocumentsViewModel viewModel = new UserDocumentsViewModel
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
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);

            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            UploadInputModel viewModel = new UploadInputModel
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
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
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

            string documentId = await this.documentsService.Upload(input, candidateId);

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
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            bool isOwner = this.documentsService.IsCandidateOwnerOfDocument(candidateId, id);

            if (!isOwner)
            {
                return this.Forbid();
            }

            bool deletionResult = await this.documentsService.Delete(id);

            if (deletionResult == false)
            {
                return this.RedirectToAction("Error", "Home");
            }

            this.TempData["Success"] = GlobalConstants.DocumentSuccessfullyDeleted;
            return this.RedirectToAction(nameof(this.All));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Download(string id, string jobApplicationId = null)
        {
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            if (candidateId == null)
            {
                return this.RedirectToAction("CreateProfile", "Candidates");
            }

            if (jobApplicationId == null)
            {
                bool isOwner = this.documentsService.IsCandidateOwnerOfDocument(candidateId, id);
                if (!isOwner)
                {
                    return this.Forbid();
                }
            }
            else
            {
                string userId = this.userManager.GetUserId(this.User);

                bool isUserRelatedToJobApplication = this.jobApplicationService.IsUserRelatedToJobApplication(jobApplicationId, userId);
                if (!isUserRelatedToJobApplication)
                {
                    return this.Forbid();
                }
            }

            byte[] fileAsByteArray = await this.documentsService.Download(id);
            if (fileAsByteArray == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            string fileName = this.documentsService.GetDocumentNameById(id);
            string contentType = this.mimeMappingService.Map(fileName);

            return this.File(fileAsByteArray, contentType, fileName);
        }
    }
}
