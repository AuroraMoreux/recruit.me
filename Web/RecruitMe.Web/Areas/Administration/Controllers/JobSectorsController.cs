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

    public class JobSectorsController : AdministrationController
    {
        private readonly ApplicationDbContext context;

        public JobSectorsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Administration/JobSectors
        public async Task<IActionResult> Index()
        {
            List<JobSector> sectors = await this.context
                .JobSectors
                .IgnoreQueryFilters()
                .ToListAsync();

            return this.View(sectors);
        }

        // GET: Administration/JobSectors/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/JobSectors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] JobSector jobSector)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(jobSector);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (jobSector.IsDeleted)
            {
                jobSector.DeletedOn = currentTime;
            }

            jobSector.CreatedOn = currentTime;
            this.context.Add(jobSector);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/JobSectors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            JobSector jobSector = await this.context
                .JobSectors
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(js => js.Id == id);

            if (jobSector == null)
            {
                return this.NotFound();
            }

            return this.View(jobSector);
        }

        // POST: Administration/JobSectors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] JobSector jobSector)
        {
            if (id != jobSector.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(jobSector);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (jobSector.IsDeleted)
            {
                jobSector.DeletedOn = currentTime;
            }
            else if (!jobSector.IsDeleted)
            {
                jobSector.DeletedOn = null;
            }

            jobSector.ModifiedOn = currentTime;
            try
            {
                this.context.Update(jobSector);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.JobSectorExists(jobSector.Id))
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

        // GET: Administration/JobSectors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            JobSector jobSector = await this.context.JobSectors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobSector == null)
            {
                return this.NotFound();
            }

            return this.View(jobSector);
        }

        // POST: Administration/JobSectors/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            JobSector jobSector = await this.context.JobSectors.FindAsync(id);
            jobSector.DeletedOn = DateTime.UtcNow;
            this.context.JobSectors.Remove(jobSector);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool JobSectorExists(int id)
        {
            return this.context
                .JobSectors
                .IgnoreQueryFilters()
                .Any(e => e.Id == id);
        }
    }
}
