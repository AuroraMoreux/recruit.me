namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Administration.JobTypes;

    public class JobTypesService : IJobTypesService
    {
        private readonly IDeletableEntityRepository<JobType> jobTypesRepository;

        public JobTypesService(IDeletableEntityRepository<JobType> jobTypesRepository)
        {
            this.jobTypesRepository = jobTypesRepository;
        }

        public async Task<int> CreateAsync(CreateViewModel input)
        {
            var jobType = AutoMapperConfig.MapperInstance.Map<JobType>(input);

            if (jobType.IsDeleted)
            {
                jobType.DeletedOn = DateTime.UtcNow;
            }

            jobType.CreatedOn = DateTime.UtcNow;
            try
            {
                await this.jobTypesRepository.AddAsync(jobType);
                await this.jobTypesRepository.SaveChangesAsync();
                return jobType.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var jobType = this.jobTypesRepository
                  .All()
                  .Where(t => t.Id == id)
                  .FirstOrDefault();

            if (jobType == null)
            {
                return false;
            }

            try
            {
                this.jobTypesRepository.Delete(jobType);
                await jobTypesRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            var jobTypes = this.jobTypesRepository
                .AllAsNoTracking()
                .OrderBy(jt => jt.Name)
                .To<T>()
                .ToList();

            return jobTypes;
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            return this.jobTypesRepository
                 .AllAsNoTrackingWithDeleted()
                 .OrderBy(t => t.Name)
                 .To<T>()
                 .ToList();
        }

        public T GetDetails<T>(int id)
        {
            return this.jobTypesRepository
                  .AllAsNoTrackingWithDeleted()
                  .Where(t => t.Id == id)
                  .To<T>()
                  .FirstOrDefault();
        }

        public async Task<int> UpdateAsync(int id, EditViewModel input)
        {
            var jobType = this.jobTypesRepository
                  .AllWithDeleted()
                  .Where(t => t.Id == id)
                  .FirstOrDefault();

            if (jobType == null)
            {
                return -1;
            }

            jobType.Name = input.Name;
            jobType.IsDeleted = input.IsDeleted;
            jobType.ModifiedOn = DateTime.UtcNow;
            if (jobType.IsDeleted)
            {
                jobType.DeletedOn = DateTime.UtcNow;
            }

            try
            {
                this.jobTypesRepository.Update(jobType);
                await this.jobTypesRepository.SaveChangesAsync();
                return jobType.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
