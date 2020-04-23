namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.JobApplicationStatuses;

    public class JobApplicationStatusesController : AdministrationController
    {
        private readonly IJobApplicationStatusesService jobApplicationStatusesService;

        public JobApplicationStatusesController(IJobApplicationStatusesService jobApplicationStatusesService)
        {
            this.jobApplicationStatusesService = jobApplicationStatusesService;
        }

        // GET: Administration/JobApplicationStatuses
        public IActionResult Index(int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            var statuses = this.jobApplicationStatusesService.GetAllWithDeleted<JobApplicationStatusViewModel>();
            var pagesCount = (int)Math.Ceiling(statuses.Count() / (decimal)perPage);

            var paginatedStatuses = statuses
               .Skip(perPage * (page - 1))
               .Take(perPage)
               .ToList();

            var viewModel = new AllStatusesViewModel
            {
                Statuses = paginatedStatuses,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        // GET: Administration/JobApplicationStatuses/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/JobApplicationStatuses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] CreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var result = await this.jobApplicationStatusesService.CreateAsync(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobApplicationStatuses/Edit/5
        public IActionResult Edit(int id)
        {
            var status = this.jobApplicationStatusesService.GetDetails<EditViewModel>(id);
            if (status == null)
            {
                return this.NotFound();
            }

            return this.View(status);
        }

        // POST: Administration/JobApplicationStatuses/Edit/5
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

            var result = await this.jobApplicationStatusesService.UpdateAsync(id, input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobApplicationStatuses/Delete/5
        public IActionResult Delete(int id)
        {
            var status = this.jobApplicationStatusesService.GetDetails<DeleteViewModel>(id);
            if (status == null)
            {
                return this.NotFound();
            }

            return this.View(status);
        }

        // POST: Administration/JobApplicationStatuses/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isDeleted = await this.jobApplicationStatusesService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
