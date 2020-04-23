namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.JobTypes;

    public class JobTypesController : AdministrationController
    {
        private readonly IJobTypesService jobTypesService;

        public JobTypesController(IJobTypesService jobTypesService)
        {
            this.jobTypesService = jobTypesService;
        }

        // GET: Administration/JobTypes
        public IActionResult Index(int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            var types = this.jobTypesService.GetAllWithDeleted<JobTypesViewModel>();
            var pagesCount = (int)Math.Ceiling(types.Count() / (decimal)perPage);

            var paginatedTypes = types
               .Skip(perPage * (page - 1))
               .Take(perPage)
               .ToList();

            var viewModel = new AllJobTypesViewModel
            {
                Types = paginatedTypes,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        // GET: Administration/JobTypes/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/JobTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] CreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var result = await this.jobTypesService.CreateAsync(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobTypes/Edit/5
        public IActionResult Edit(int id)
        {
            var jobType = this.jobTypesService.GetDetails<EditViewModel>(id);
            if (jobType == null)
            {
                return this.NotFound();
            }

            return this.View(jobType);
        }

        // POST: Administration/JobTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] EditViewModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var result = await this.jobTypesService.UpdateAsync(id, input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobTypes/Delete/5
        public IActionResult Delete(int id)
        {
            var jobType = this.jobTypesService.GetDetails<DeleteViewModel>(id);
            if (jobType == null)
            {
                return this.NotFound();
            }

            return this.View(jobType);
        }

        // POST: Administration/JobTypes/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isDeleted = await this.jobTypesService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
