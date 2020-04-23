namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Administration.Skills;

    public class SkillsService : ISkillsService
    {
        private readonly IDeletableEntityRepository<Skill> skillsRepository;

        public SkillsService(IDeletableEntityRepository<Skill> skillsRepository)
        {
            this.skillsRepository = skillsRepository;
        }

        public async Task<int> CreateAsync(CreateViewModel input)
        {
            var skill = AutoMapperConfig.MapperInstance.Map<Skill>(input);

            if (skill.IsDeleted)
            {
                skill.DeletedOn = DateTime.UtcNow;
            }

            skill.CreatedOn = DateTime.UtcNow;
            try
            {
                await this.skillsRepository.AddAsync(skill);
                await this.skillsRepository.SaveChangesAsync();
                return skill.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var skill = this.skillsRepository
                 .All()
                 .Where(s => s.Id == id)
                 .FirstOrDefault();

            if (skill == null)
            {
                return false;
            }

            try
            {
                this.skillsRepository.Delete(skill);
                await this.skillsRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            var skills = this.skillsRepository
                 .AllAsNoTracking()
                 .OrderBy(s => s.Name)
                 .To<T>()
                 .ToList();

            return skills;
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            return this.skillsRepository
                 .AllAsNoTrackingWithDeleted()
                 .OrderBy(s => s.Name)
                 .To<T>()
                 .ToList();
        }

        public T GetDetails<T>(int id)
        {
            return this.skillsRepository
                 .AllAsNoTrackingWithDeleted()
                 .Where(s => s.Id == id)
                 .To<T>()
                 .FirstOrDefault();
        }

        public async Task<int> UpdateAsync(int id, EditViewModel input)
        {
            var skill = this.skillsRepository
                 .AllWithDeleted()
                 .Where(c => c.Id == id)
                 .FirstOrDefault();

            if (skill == null)
            {
                return -1;
            }

            skill.Name = input.Name;
            skill.IsDeleted = input.IsDeleted;
            skill.ModifiedOn = DateTime.UtcNow;
            if (skill.IsDeleted)
            {
                skill.DeletedOn = DateTime.UtcNow;
            }

            try
            {
                this.skillsRepository.Update(skill);
                await this.skillsRepository.SaveChangesAsync();
                return skill.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
