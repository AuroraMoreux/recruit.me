namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.JobLevels;

    public class JobLevelsController : AdministrationController
    {
        private const int ItemsPerPageCount = 8;
        private readonly IJobLevelsService jobLevelsService;

        public JobLevelsController(IJobLevelsService jobLevelsService)
        {
            this.jobLevelsService = jobLevelsService;
        }

        // GET: Administration/JobLevels
        public IActionResult Index(int page = 1, int perPage = ItemsPerPageCount)
        {
            IEnumerable<JobLevelViewModel> levels = this.jobLevelsService.GetAllWithDeleted<JobLevelViewModel>();
            int pagesCount = (int)Math.Ceiling(levels.Count() / (decimal)perPage);

            List<JobLevelViewModel> paginatedLevels = levels
               .Skip(perPage * (page - 1))
               .Take(perPage)
               .ToList();

            AllJobLevelsViewModel viewModel = new AllJobLevelsViewModel
            {
                Levels = paginatedLevels,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        // GET: Administration/JobLevels/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/JobLevels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] CreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            int result = await this.jobLevelsService.Create(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobLevels/Edit/5
        public IActionResult Edit(int id)
        {
            EditViewModel level = this.jobLevelsService.GetDetails<EditViewModel>(id);
            if (level == null)
            {
                return this.NotFound();
            }

            return this.View(level);
        }

        // POST: Administration/JobLevels/Edit/5
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

            int result = await this.jobLevelsService.Update(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobLevels/Delete/5
        public IActionResult Delete(int id)
        {
            DeleteViewModel level = this.jobLevelsService.GetDetails<DeleteViewModel>(id);
            if (level == null)
            {
                return this.NotFound();
            }

            return this.View(level);
        }

        // POST: Administration/JobLevels/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool isDeleted = this.jobLevelsService.Delete(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
