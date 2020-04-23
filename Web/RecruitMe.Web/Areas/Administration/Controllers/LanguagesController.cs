namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.Languages;

    public class LanguagesController : AdministrationController
    {
        private readonly ILanguagesService languagesService;

        public LanguagesController(ILanguagesService languagesService)
        {
            this.languagesService = languagesService;
        }

        // GET: Administration/Languages
        public IActionResult Index(int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            var languages = this.languagesService.GetAllWithDeleted<LanguagesViewModel>();
            var pagesCount = (int)Math.Ceiling(languages.Count() / (decimal)perPage);

            var paginatedaLanguages = languages
               .Skip(perPage * (page - 1))
               .Take(perPage)
               .ToList();

            var viewModel = new AllLanguagesViewModel
            {
                Languages = paginatedaLanguages,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        // GET: Administration/Languages/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Languages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] CreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var result = await this.languagesService.CreateAsync(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/Languages/Edit/5
        public IActionResult Edit(int id)
        {
            var language = this.languagesService.GetDetails<EditViewModel>(id);
            if (language == null)
            {
                return this.NotFound();
            }

            return this.View(language);
        }

        // POST: Administration/Languages/Edit/5
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

            var result = await this.languagesService.UpdateAsync(id, input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/Languages/Delete/5
        public IActionResult Delete(int id)
        {
            var language = this.languagesService.GetDetails<DeleteViewModel>(id);
            if (language == null)
            {
                return this.NotFound();
            }

            return this.View(language);
        }

        // POST: Administration/Languages/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isDeleted = await this.languagesService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
