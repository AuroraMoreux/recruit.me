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
    public class LanguagesController : Controller
    {
        private readonly ApplicationDbContext context;

        public LanguagesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Administration/Languages
        public async Task<IActionResult> Index()
        {
            List<Language> languages = await this.context
                .Languages
                .IgnoreQueryFilters()
                .ToListAsync();

            return this.View(languages);
        }

        // GET: Administration/Languages/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Languages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] Language language)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(language);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (language.IsDeleted)
            {
                language.DeletedOn = currentTime;
            }

            language.CreatedOn = currentTime;
            this.context.Add(language);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/Languages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Language language = await this.context
                .Languages
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (language == null)
            {
                return this.NotFound();
            }

            return this.View(language);
        }

        // POST: Administration/Languages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,Id")] Language language)
        {
            if (id != language.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(language);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (language.IsDeleted)
            {
                language.DeletedOn = currentTime;
            }
            else if (!language.IsDeleted)
            {
                language.DeletedOn = null;
            }

            language.ModifiedOn = currentTime;
            try
            {
                this.context.Update(language);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.LanguageExists(language.Id))
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

        // GET: Administration/Languages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Language language = await this.context.Languages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (language == null)
            {
                return this.NotFound();
            }

            return this.View(language);
        }

        // POST: Administration/Languages/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Language language = await this.context.Languages.FindAsync(id);
            language.DeletedOn = DateTime.UtcNow;
            this.context.Languages.Remove(language);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool LanguageExists(int id)
        {
            return this.context
                .Languages
                .IgnoreQueryFilters()
                .Any(e => e.Id == id);
        }
    }
}
