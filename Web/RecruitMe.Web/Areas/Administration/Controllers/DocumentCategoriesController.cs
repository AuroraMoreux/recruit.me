namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.DocumentCategories;

    public class DocumentCategoriesController : AdministrationController
    {
        private readonly IDocumentCategoriesService documentCategoriesService;

        public DocumentCategoriesController(IDocumentCategoriesService documentCategoriesService)
        {
            this.documentCategoriesService = documentCategoriesService;
        }

        // GET: Administration/DocumentCategories
        public IActionResult Index(int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            IEnumerable<DocumentCategoriesViewModel> categories = this.documentCategoriesService.GetAllWithDeleted<DocumentCategoriesViewModel>();

            int pagesCount = (int)Math.Ceiling(categories.Count() / (decimal)perPage);

            List<DocumentCategoriesViewModel> paginatedCategories = categories
               .Skip(perPage * (page - 1))
               .Take(perPage)
               .ToList();

            AllDocumentCategoriesViewModel viewModel = new AllDocumentCategoriesViewModel
            {
                DocumentCategories = paginatedCategories,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        // GET: Administration/DocumentCategories/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/DocumentCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] CreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            int result = await this.documentCategoriesService.Create(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/DocumentCategories/Edit/5
        public IActionResult Edit(int id)
        {
            EditViewModel category = this.documentCategoriesService.GetDetails<EditViewModel>(id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        // POST: Administration/DocumentCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,Id")] EditViewModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            int result = await this.documentCategoriesService.Update(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/DocumentCategories/Delete/5
        public IActionResult Delete(int id)
        {
            DeleteViewModel category = this.documentCategoriesService.GetDetails<DeleteViewModel>(id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        // POST: Administration/DocumentCategories/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool isDeleted = this.documentCategoriesService.Delete(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
