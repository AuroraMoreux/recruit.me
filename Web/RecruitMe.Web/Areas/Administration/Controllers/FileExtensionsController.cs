namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.FileExtensions;

    public class FileExtensionsController : AdministrationController
    {
        private readonly IFileExtensionsService fileExtensionsService;

        public FileExtensionsController(IFileExtensionsService fileExtensionsService)
        {
            this.fileExtensionsService = fileExtensionsService;
        }

        // GET: Administration/FileExtensions
        public IActionResult Index(int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            IEnumerable<ExtensionsViewModel> extensions = this.fileExtensionsService.GetAllWithDeleted<ExtensionsViewModel>();
            int pagesCount = (int)Math.Ceiling(extensions.Count() / (decimal)perPage);

            List<ExtensionsViewModel> paginatedExtensions = extensions
               .Skip(perPage * (page - 1))
               .Take(perPage)
               .ToList();

            AllFileExtensionsViewModel viewModel = new AllFileExtensionsViewModel
            {
                Extensions = paginatedExtensions,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        // GET: Administration/FileExtensions/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/FileExtensions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted,FileType")] CreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            int result = await this.fileExtensionsService.Create(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/FileExtensions/Edit/5
        public IActionResult Edit(int id)
        {
            EditViewModel extension = this.fileExtensionsService.GetDetails<EditViewModel>(id);
            if (extension == null)
            {
                return this.NotFound();
            }

            return this.View(extension);
        }

        // POST: Administration/FileExtensions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,Id,FileType")] EditViewModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            int result = await this.fileExtensionsService.Update(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/FileExtensions/Delete/5
        public IActionResult Delete(int id)
        {
            DeleteViewModel extension = this.fileExtensionsService.GetDetails<DeleteViewModel>(id);
            if (extension == null)
            {
                return this.NotFound();
            }

            return this.View(extension);
        }

        // POST: Administration/FileExtensions/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool isDeleted = this.fileExtensionsService.Delete(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
