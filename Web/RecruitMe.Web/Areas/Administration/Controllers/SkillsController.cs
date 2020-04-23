namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Common;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.Skills;

    public class SkillsController : AdministrationController
    {
        private readonly ISkillsService skillsService;

        public SkillsController(ISkillsService skillsService)
        {
            this.skillsService = skillsService;
        }

        // GET: Administration/Skills
        public IActionResult Index(int page = 1, int perPage = GlobalConstants.ItemsPerPage)
        {
            var skills = this.skillsService.GetAllWithDeleted<SkillsViewModel>();

            var pagesCount = (int)Math.Ceiling(skills.Count() / (decimal)perPage);

            var paginatedSkills = skills
               .Skip(perPage * (page - 1))
               .Take(perPage)
               .ToList();

            var viewModel = new AllSkillsViewModel
            {
                Skills = paginatedSkills,
                CurrentPage = page,
                PagesCount = pagesCount,
            };
            return this.View(viewModel);
        }

        // GET: Administration/Skills/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Skills/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted")] CreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var result = await this.skillsService.CreateAsync(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/Skills/Edit/5
        public IActionResult Edit(int id)
        {
            var skill = this.skillsService.GetDetails<EditViewModel>(id);
            if (skill == null)
            {
                return this.NotFound();
            }

            return this.View(skill);
        }

        // POST: Administration/Skills/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,Id")] EditViewModel input)
        {
            if (id != input.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var result = await this.skillsService.UpdateAsync(id, input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/Skills/Delete/5
        public IActionResult Delete(int id)
        {
            var skill = this.skillsService.GetDetails<DeleteViewModel>(id);
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
            var isDeleted = await this.skillsService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
