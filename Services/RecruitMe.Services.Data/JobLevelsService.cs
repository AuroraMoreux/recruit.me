namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Administration.JobLevels;

    public class JobLevelsService : IJobLevelsService
    {
        private readonly IDeletableEntityRepository<JobLevel> jobLevelsRepository;

        public JobLevelsService(IDeletableEntityRepository<JobLevel> jobLevelsRepository)
        {
            this.jobLevelsRepository = jobLevelsRepository;
        }

        public async Task<int> CreateAsync(CreateViewModel input)
        {
            var level = AutoMapperConfig.MapperInstance.Map<JobLevel>(input);

            if (level.IsDeleted)
            {
                level.DeletedOn = DateTime.UtcNow;
            }

            level.CreatedOn = DateTime.UtcNow;
            try
            {
                await this.jobLevelsRepository.AddAsync(level);
                await this.jobLevelsRepository.SaveChangesAsync();
                return level.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var level = this.jobLevelsRepository
                  .All()
                  .Where(l => l.Id == id)
                  .FirstOrDefault();

            if (level == null)
            {
                return false;
            }

            try
            {
                this.jobLevelsRepository.Delete(level);
                await this.jobLevelsRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            var jobLevels = this.jobLevelsRepository
                .AllAsNoTracking()
                .To<T>()
                .ToList();

            return jobLevels;
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            return this.jobLevelsRepository
                .AllAsNoTrackingWithDeleted()
                .OrderBy(l => l.Name)
                .To<T>()
                .ToList();
        }

        public T GetDetails<T>(int id)
        {
            return this.jobLevelsRepository
                .AllAsNoTrackingWithDeleted()
                .Where(l => l.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task<int> UpdateAsync(int id,EditViewModel input)
        {
            var level = this.jobLevelsRepository
                 .AllWithDeleted()
                 .Where(l => l.Id == id)
                 .FirstOrDefault();

            if (level == null)
            {
                return -1;
            }

            level.Name = input.Name;
            level.IsDeleted = input.IsDeleted;
            level.ModifiedOn = DateTime.UtcNow;
            if (level.IsDeleted)
            {
                level.DeletedOn = DateTime.UtcNow;
            }

            try
            {
                this.jobLevelsRepository.Update(level);
                await this.jobLevelsRepository.SaveChangesAsync();
                return level.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
