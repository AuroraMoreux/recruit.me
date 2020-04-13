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
    using RecruitMe.Web.ViewModels.Documents;

    public class DocumentsController : BaseController
    {
        private const int DocumentsPerPageDefaultCount = 10;

        private readonly IDocumentsService documentsService;
        private readonly ICandidatesService candidatesService;
        private readonly IFileExtensionsService fileExtensionsService;
        private readonly IDocumentCategoriesService documentCategoriesService;
        private readonly IMimeMappingService mimeMappingService;
        private readonly UserManager<ApplicationUser> userManager;

        private IEnumerable<string> allowedExtensions;
        private IEnumerable<CategoriesDropDownViewModel> allowedCategories;

        public DocumentsController(
            IDocumentsService documentsService,
            ICandidatesService candidatesService,
            IFileExtensionsService fileExtensionsService,
            IDocumentCategoriesService documentCategoriesService,
            IMimeMappingService mimeMappingService,
            UserManager<ApplicationUser> userManager)
        {
            this.documentsService = documentsService;
            this.candidatesService = candidatesService;
            this.fileExtensionsService = fileExtensionsService;
            this.documentCategoriesService = documentCategoriesService;
            this.mimeMappingService = mimeMappingService;
            this.userManager = userManager;

            this.allowedExtensions = this.fileExtensionsService.GetAll();
            this.allowedCategories = this.documentCategoriesService.GetAll<CategoriesDropDownViewModel>();
        }

        [Authorize(Roles = GlobalConstants.CandidateRoleName)]
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

        [Authorize(Roles = GlobalConstants.CandidateRoleName)]
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
        [Authorize(Roles = GlobalConstants.CandidateRoleName)]
        public async Task<IActionResult> Upload(UploadInputModel input)
        {
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
                this.ModelState.AddModelError(string.Empty, GlobalConstants.DocumentFileExtensionNotSupported);
                return this.View(input);
            }

            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            string documentId = await this.documentsService.Upload(input, candidateId);

            if (documentId == null)
            {
                return this.View("Error");
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            bool isOwner = this.documentsService.IsCandidateOwnerOfDocument(candidateId, id);

            if (!isOwner)
            {
                return this.Forbid();
            }

            await this.documentsService.Delete(id);

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            string candidateId = this.candidatesService.GetCandidateIdByUsername(this.User.Identity.Name);
            bool isOwner = this.documentsService.IsCandidateOwnerOfDocument(candidateId, id);

            if (!isOwner)
            {
                return this.Forbid();
            }

            byte[] fileAsByteArray = await this.documentsService.Download(id);
            string fileName = this.documentsService.GetDocumentNameById(id);
            string contentType = this.mimeMappingService.Map(fileName);

            return this.File(fileAsByteArray, contentType, fileName);
        }
    }
}
