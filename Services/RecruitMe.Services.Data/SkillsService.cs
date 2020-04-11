namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class SkillsService : ISkillsService
    {
        private readonly IDeletableEntityRepository<Skill> skillsRepository;

        public SkillsService(IDeletableEntityRepository<Skill> skillsRepository)
        {
            this.skillsRepository = skillsRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var skills = this.skillsRepository
                 .All()
                 .OrderBy(s => s.Name)
                 .To<T>()
                 .ToList();

            return skills;
        }
    }
}
