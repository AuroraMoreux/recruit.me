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

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class JobLevelsController : Controller
    {
        private readonly ApplicationDbContext context;

        public JobLevelsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Administration/JobLevels
        public async Task<IActionResult> Index()
        {
            List<JobLevel> levels = await this.context
                 .JobLevels
                 .IgnoreQueryFilters()
                 .ToListAsync();

            return this.View(levels);
        }

        // GET: Administration/JobLevels/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/JobLevels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] JobLevel jobLevel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(jobLevel);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (jobLevel.IsDeleted)
            {
                jobLevel.DeletedOn = currentTime;
            }

            jobLevel.CreatedOn = currentTime;
            this.context.Add(jobLevel);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobLevels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            JobLevel jobLevel = await this.context
                .JobLevels
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(jl => jl.Id == id);

            if (jobLevel == null)
            {
                return this.NotFound();
            }

            return this.View(jobLevel);
        }

        // POST: Administration/JobLevels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,Id")] JobLevel jobLevel)
        {
            if (id != jobLevel.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(jobLevel);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (jobLevel.IsDeleted)
            {
                jobLevel.DeletedOn = currentTime;
            }
            else if (!jobLevel.IsDeleted)
            {
                jobLevel.DeletedOn = null;
            }

            jobLevel.ModifiedOn = currentTime;
            try
            {
                this.context.Update(jobLevel);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.JobLevelExists(jobLevel.Id))
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

        // GET: Administration/JobLevels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            JobLevel jobLevel = await this.context.JobLevels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobLevel == null)
            {
                return this.NotFound();
            }

            return this.View(jobLevel);
        }

        // POST: Administration/JobLevels/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            JobLevel jobLevel = await this.context.JobLevels.FindAsync(id);
            jobLevel.DeletedOn = DateTime.UtcNow;
            this.context.JobLevels.Remove(jobLevel);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool JobLevelExists(int id)
        {
            return this.context
               .DocumentCategories
               .IgnoreQueryFilters()
               .Any(e => e.Id == id);
        }
    }
}
