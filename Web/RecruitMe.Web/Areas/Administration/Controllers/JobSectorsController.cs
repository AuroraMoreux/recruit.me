namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.JobSectors;

    public class JobSectorsController : AdministrationController
    {
        private readonly IJobSectorsService jobSectorsService;

        public JobSectorsController(IJobSectorsService jobSectorsService)
        {
            this.jobSectorsService = jobSectorsService;
        }

        // GET: Administration/JobSectors
        public IActionResult Index(int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            var sectors = this.jobSectorsService.GetAllWithDeleted<JobSectorsViewModel>();
            var pagesCount = (int)Math.Ceiling(sectors.Count() / (decimal)perPage);

            var paginatedSectors = sectors
               .Skip(perPage * (page - 1))
               .Take(perPage)
               .ToList();

            var viewModel = new AllJobSectorsViewModel
            {
                Sectors = paginatedSectors,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(viewModel);
        }

        // GET: Administration/JobSectors/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/JobSectors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] CreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var result = await this.jobSectorsService.CreateAsync(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobSectors/Edit/5
        public IActionResult Edit(int id)
        {
            var sector = this.jobSectorsService.GetDetails<EditViewModel>(id);
            if (sector == null)
            {
                return this.NotFound();
            }

            return this.View(sector);
        }

        // POST: Administration/JobSectors/Edit/5
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

            var result = await this.jobSectorsService.UpdateAsync(id, input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobSectors/Delete/5
        public IActionResult Delete(int id)
        {
            var sector = this.jobSectorsService.GetDetails<DeleteViewModel>(id);
            if (sector == null)
            {
                return this.NotFound();
            }

            return this.View(sector);
        }

        // POST: Administration/JobSectors/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isDeleted = await this.jobSectorsService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
