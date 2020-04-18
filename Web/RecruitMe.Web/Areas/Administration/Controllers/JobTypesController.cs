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

    public class JobTypesController : AdministrationController
    {
        private readonly ApplicationDbContext context;

        public JobTypesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Administration/JobTypes
        public async Task<IActionResult> Index()
        {
            List<JobType> types = await this.context
                 .JobTypes
                 .IgnoreQueryFilters()
                 .ToListAsync();

            return this.View(types);
        }

        // GET: Administration/JobTypes/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/JobTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] JobType jobType)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(jobType);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (jobType.IsDeleted)
            {
                jobType.DeletedOn = currentTime;
            }

            jobType.CreatedOn = currentTime;
            this.context.Add(jobType);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            JobType jobType = await this.context
                .JobTypes
                .IgnoreQueryFilters()

                .FirstOrDefaultAsync(jt => jt.Id == id);
            if (jobType == null)
            {
                return this.NotFound();
            }

            return this.View(jobType);
        }

        // POST: Administration/JobTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] JobType jobType)
        {
            if (id != jobType.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(jobType);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (jobType.IsDeleted)
            {
                jobType.DeletedOn = currentTime;
            }
            else if (!jobType.IsDeleted)
            {
                jobType.DeletedOn = null;
            }

            jobType.ModifiedOn = currentTime;
            try
            {
                this.context.Update(jobType);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.JobTypeExists(jobType.Id))
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

        // GET: Administration/JobTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            JobType jobType = await this.context.JobTypes
                .FirstOrDefaultAsync(m => m.Id == id);
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
            JobType jobType = await this.context.JobTypes.FindAsync(id);
            jobType.DeletedOn = DateTime.UtcNow;
            this.context.JobTypes.Remove(jobType);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool JobTypeExists(int id)
        {
            return this.context
               .JobTypes
               .IgnoreQueryFilters()
               .Any(e => e.Id == id);
        }
    }
}
