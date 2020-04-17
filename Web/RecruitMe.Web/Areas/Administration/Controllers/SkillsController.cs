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
    public class SkillsController : Controller
    {
        private readonly ApplicationDbContext context;

        public SkillsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Administration/Skills
        public async Task<IActionResult> Index()
        {
            List<Skill> skills = await this.context
                .Skills
                .IgnoreQueryFilters()
                .ToListAsync();

            return this.View(skills);
        }

        // GET: Administration/Skills/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Skills/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] Skill skill)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(skill);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (skill.IsDeleted)
            {
                skill.DeletedOn = currentTime;
            }

            skill.CreatedOn = currentTime;
            this.context.Add(skill);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/Skills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Skill skill = await this.context
                .Skills
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (skill == null)
            {
                return this.NotFound();
            }

            return this.View(skill);
        }

        // POST: Administration/Skills/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,Id")] Skill skill)
        {
            if (id != skill.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(skill);
            }

            DateTime currentTime = DateTime.UtcNow;
            if (skill.IsDeleted)
            {
                skill.DeletedOn = currentTime;
            }
            else if (!skill.IsDeleted)
            {
                skill.DeletedOn = null;
            }

            skill.ModifiedOn = currentTime;
            try
            {
                this.context.Update(skill);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.SkillExists(skill.Id))
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

        // GET: Administration/Skills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Skill skill = await this.context.Skills
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skill == null)
            {
                return this.NotFound();
            }

            return this.View(skill);
        }

        // POST: Administration/Skills/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Skill skill = await this.context.Skills.FindAsync(id);
            skill.DeletedOn = DateTime.UtcNow;
            this.context.Skills.Remove(skill);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool SkillExists(int id)
        {
            return this.context
                .Skills
                .IgnoreQueryFilters()
                .Any(e => e.Id == id);
        }
    }
}
