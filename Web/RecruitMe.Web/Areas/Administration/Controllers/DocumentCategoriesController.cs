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

    public class DocumentCategoriesController : AdministrationController
    {
        private readonly ApplicationDbContext context;

        public DocumentCategoriesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Administration/DocumentCategories
        public async Task<IActionResult> Index()
        {
            List<DocumentCategory> categories = await this.context
                .DocumentCategories
                .IgnoreQueryFilters()
                .ToListAsync();

            return this.View(categories);
        }

        // GET: Administration/DocumentCategories/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/DocumentCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] DocumentCategory documentCategory)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(documentCategory);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (documentCategory.IsDeleted)
            {
                documentCategory.DeletedOn = currentTime;
            }

            documentCategory.CreatedOn = currentTime;
            this.context.Add(documentCategory);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/DocumentCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            DocumentCategory documentCategory = await this.context
                .DocumentCategories
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(dc => dc.Id == id);

            if (documentCategory == null)
            {
                return this.NotFound();
            }

            return this.View(documentCategory);
        }

        // POST: Administration/DocumentCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,Id")] DocumentCategory documentCategory)
        {
            if (id != documentCategory.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(documentCategory);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (documentCategory.IsDeleted)
            {
                documentCategory.DeletedOn = currentTime;
            }
            else if (!documentCategory.IsDeleted)
            {
                documentCategory.DeletedOn = null;
            }

            documentCategory.ModifiedOn = currentTime;
            try
            {
                this.context.Update(documentCategory);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.DocumentCategoryExists(documentCategory.Id))
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

        // GET: Administration/DocumentCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            DocumentCategory documentCategory = await this.context.DocumentCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documentCategory == null)
            {
                return this.NotFound();
            }

            return this.View(documentCategory);
        }

        // POST: Administration/DocumentCategories/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            DocumentCategory documentCategory = await this.context.DocumentCategories.FindAsync(id);
            documentCategory.DeletedOn = DateTime.UtcNow;
            this.context.DocumentCategories.Remove(documentCategory);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool DocumentCategoryExists(int id)
        {
            return this.context
                .DocumentCategories
                .IgnoreQueryFilters()
                .Any(e => e.Id == id);
        }
    }
}
