namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RecruitMe.Common;
    using RecruitMe.Data;
    using RecruitMe.Data.Models.EnumModels;

    public class JobApplicationStatusesController : AdministrationController
    {
        private readonly ApplicationDbContext context;

        public JobApplicationStatusesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Administration/JobApplicationStatuses
        public async Task<IActionResult> Index()
        {
            List<JobApplicationStatus> statuses = await this.context
                 .ApplicationStatuses
                 .IgnoreQueryFilters()
                 .ToListAsync();

            return this.View(statuses);
        }

        // GET: Administration/JobApplicationStatuses/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/JobApplicationStatuses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] JobApplicationStatus jobApplicationStatus)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(jobApplicationStatus);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (jobApplicationStatus.IsDeleted)
            {
                jobApplicationStatus.DeletedOn = currentTime;
            }

            jobApplicationStatus.CreatedOn = currentTime;
            this.context.Add(jobApplicationStatus);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobApplicationStatuses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            JobApplicationStatus jobApplicationStatus = await this.context
                .ApplicationStatuses
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(jas => jas.Id == id);

            if (jobApplicationStatus == null)
            {
                return this.NotFound();
            }

            return this.View(jobApplicationStatus);
        }

        // POST: Administration/JobApplicationStatuses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,Id")] JobApplicationStatus jobApplicationStatus)
        {
            if (id != jobApplicationStatus.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(jobApplicationStatus);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (jobApplicationStatus.IsDeleted)
            {
                jobApplicationStatus.DeletedOn = currentTime;
            }
            else if (!jobApplicationStatus.IsDeleted)
            {
                jobApplicationStatus.DeletedOn = null;
            }

            jobApplicationStatus.ModifiedOn = currentTime;
            try
            {
                this.context.Update(jobApplicationStatus);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.JobApplicationStatusExists(jobApplicationStatus.Id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobApplicationStatuses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            JobApplicationStatus jobApplicationStatus = await this.context.ApplicationStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplicationStatus == null)
            {
                return this.NotFound();
            }

            return this.View(jobApplicationStatus);
        }

        // POST: Administration/JobApplicationStatuses/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            JobApplicationStatus jobApplicationStatus = await this.context.ApplicationStatuses.FindAsync(id);
            jobApplicationStatus.DeletedOn = DateTime.UtcNow;
            this.context.ApplicationStatuses.Remove(jobApplicationStatus);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool JobApplicationStatusExists(int id)
        {
            return this.context
                .ApplicationStatuses
                .IgnoreQueryFilters()
                .Any(e => e.Id == id);
        }
    }
}
