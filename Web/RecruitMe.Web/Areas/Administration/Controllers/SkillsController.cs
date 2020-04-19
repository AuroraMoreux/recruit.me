namespace RecruitMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using RecruitMe.Services.Data;
    using RecruitMe.Web.ViewModels.Administration.Skills;

    public class SkillsController : AdministrationController
    {
        private const int ItemsPerPageCount = 8;
        private readonly ISkillsService skillsService;

        public SkillsController(ISkillsService skillsService)
        {
            this.skillsService = skillsService;
        }

        // GET: Administration/Skills
        public IActionResult Index(int page = 1, int perPage = ItemsPerPageCount)
        {
            IEnumerable<SkillsViewModel> skills = this.skillsService.GetAllWithDeleted<SkillsViewModel>();

            int pagesCount = (int)Math.Ceiling(skills.Count() / (decimal)perPage);

            List<SkillsViewModel> paginatedSkills = skills
               .Skip(perPage * (page - 1))
               .Take(perPage)
               .ToList();

            AllSkillsViewModel viewModel = new AllSkillsViewModel
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

            int result = await this.skillsService.Create(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/Skills/Edit/5
        public IActionResult Edit(int id)
        {
            EditViewModel skill = this.skillsService.GetDetails<EditViewModel>(id);
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

            int result = await this.skillsService.Update(input);

            if (result < 0)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/Skills/Delete/5
        public IActionResult Delete(int id)
        {
            DeleteViewModel skill = this.skillsService.GetDetails<DeleteViewModel>(id);
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
        public IActionResult DeleteConfirmed(int id)
        {
            bool isDeleted = this.skillsService.Delete(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Error", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
