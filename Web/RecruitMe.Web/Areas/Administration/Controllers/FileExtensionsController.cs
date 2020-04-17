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
    public class FileExtensionsController : Controller
    {
        private readonly ApplicationDbContext context;

        public FileExtensionsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Administration/FileExtensions
        public async Task<IActionResult> Index()
        {
            List<FileExtension> extensions = await this.context
                 .FileExtensions
                 .IgnoreQueryFilters()
                 .ToListAsync();

            return this.View(extensions);
        }

        // GET: Administration/FileExtensions/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/FileExtensions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] FileExtension fileExtension)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(fileExtension);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (fileExtension.IsDeleted)
            {
                fileExtension.DeletedOn = currentTime;
            }

            fileExtension.CreatedOn = currentTime;
            this.context.Add(fileExtension);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/FileExtensions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            FileExtension fileExtension = await this.context
                .FileExtensions
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(fe => fe.Id == id);

            if (fileExtension == null)
            {
                return this.NotFound();
            }

            return this.View(fileExtension);
        }

        // POST: Administration/FileExtensions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,Id")] FileExtension fileExtension)
        {
            if (id != fileExtension.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(fileExtension);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (fileExtension.IsDeleted)
            {
                fileExtension.DeletedOn = currentTime;
            }
            else if (!fileExtension.IsDeleted)
            {
                fileExtension.DeletedOn = null;
            }

            fileExtension.ModifiedOn = currentTime;
            try
            {
                this.context.Update(fileExtension);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.FileExtensionExists(fileExtension.Id))
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

        // GET: Administration/FileExtensions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            FileExtension fileExtension = await this.context.FileExtensions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileExtension == null)
            {
                return this.NotFound();
            }

            return this.View(fileExtension);
        }

        // POST: Administration/FileExtensions/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            FileExtension fileExtension = await this.context.FileExtensions.FindAsync(id);
            fileExtension.DeletedOn = DateTime.UtcNow;
            this.context.FileExtensions.Remove(fileExtension);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool FileExtensionExists(int id)
        {
            return this.context
                .FileExtensions
                .IgnoreQueryFilters()
                .Any(e => e.Id == id);
        }
    }
}
