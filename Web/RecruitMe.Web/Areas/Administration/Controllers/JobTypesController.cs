namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.JobTypes;

    public class JobTypesController : AdministrationController
    {
        private const int ItemsPerPageCount = 8;
        private readonly IJobTypesService jobTypesService;

        public JobTypesController(IJobTypesService jobTypesService)
        {
            this.jobTypesService = jobTypesService;
        }

        // GET: Administration/JobTypes
        public IActionResult Index(int page = 1, int perPage = ItemsPerPageCount)
        {
            IEnumerable<JobTypesViewModel> types = this.jobTypesService.GetAllWithDeleted<JobTypesViewModel>();
            int pagesCount = (int)Math.Ceiling(types.Count() / (decimal)perPage);

            var paginatedTypes = types
               .Skip(perPage * (page - 1))
               .Take(perPage)
               .ToList();

            AllJobTypesViewModel viewModel = new AllJobTypesViewModel
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

            int result = await this.jobTypesService.Create(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobTypes/Edit/5
        public IActionResult Edit(int id)
        {
            EditViewModel jobType = this.jobTypesService.GetDetails<EditViewModel>(id);
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

            int result = await this.jobTypesService.Update(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobTypes/Delete/5
        public IActionResult Delete(int id)
        {
            DeleteViewModel jobType = this.jobTypesService.GetDetails<DeleteViewModel>(id);
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
        public IActionResult DeleteConfirmed(int id)
        {
            bool isDeleted = this.jobTypesService.Delete(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
