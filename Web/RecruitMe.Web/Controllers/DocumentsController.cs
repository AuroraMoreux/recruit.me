namespace RecruitMe.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Documents;

    public class DocumentsController : BaseController
    {
        private const int DocumentsPerPageDefaultCount = 15;

        private readonly IDocumentsService documentsService;
        private readonly ICandidatesService candidatesService;
        private readonly UserManager<ApplicationUser> userManager;

        public DocumentsController(IDocumentsService documentsService, ICandidatesService candidatesService, UserManager<ApplicationUser> userManager)
        {
            this.documentsService = documentsService;
            this.candidatesService = candidatesService;
            this.userManager = userManager;
        }

        [Authorize(Roles = GlobalConstants.CandidateRoleName)]
        public async Task<IActionResult> All(int page = 1, int perPage = DocumentsPerPageDefaultCount)
        {
            var pagesCount = (int)Math.Ceiling(this.documentsService.GetCount() / (decimal)perPage);

            ApplicationUser user = await this.userManager.GetUserAsync(this.User);

            var documents = this.documentsService.GetAllCandidateDocuments<DocumentsViewModel>(user.Id, page, perPage);

            var viewModel = new UserDocumentsViewModel
            {
                Documents = documents,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }
    }
}
