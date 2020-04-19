namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Administration.JobApplicationStatuses;

    public class JobApplicationStatusesService : IJobApplicationStatusesService
    {
        private readonly IDeletableEntityRepository<JobApplicationStatus> jobApplicationStatusRepository;

        public JobApplicationStatusesService(IDeletableEntityRepository<JobApplicationStatus> jobApplicationStatusRepository)
        {
            this.jobApplicationStatusRepository = jobApplicationStatusRepository;
        }

        public async Task<int> Create(CreateViewModel input)
        {
            JobApplicationStatus status = AutoMapperConfig.MapperInstance.Map<JobApplicationStatus>(input);

            if (status.IsDeleted)
            {
                status.DeletedOn = DateTime.UtcNow;
            }

            status.CreatedOn = DateTime.UtcNow;
            try
            {
                await this.jobApplicationStatusRepository.AddAsync(status);
                await this.jobApplicationStatusRepository.SaveChangesAsync();
                return status.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int id)
        {
            JobApplicationStatus status = this.jobApplicationStatusRepository
                 .All()
                 .Where(s => s.Id == id)
                 .FirstOrDefault();

            if (status == null)
            {
                return false;
            }

            try
            {
                this.jobApplicationStatusRepository.Delete(status);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.jobApplicationStatusRepository
                  .AllAsNoTracking()
                  .To<T>()
                  .ToList();
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            return this.jobApplicationStatusRepository
                .AllAsNoTrackingWithDeleted()
                .OrderBy(s => s.Name)
                .To<T>()
                .ToList();
        }

        public T GetDetails<T>(int id)
        {
            return this.jobApplicationStatusRepository
                 .AllAsNoTrackingWithDeleted()
                 .Where(s => s.Id == id)
                 .To<T>()
                 .FirstOrDefault();
        }

        public async Task<int> Update(EditViewModel input)
        {
            JobApplicationStatus status = this.jobApplicationStatusRepository
                 .AllWithDeleted()
                 .Where(s => s.Id == input.Id)
                 .FirstOrDefault();

            if (status == null)
            {
                return -1;
            }

            status.Name = input.Name;
            status.IsDeleted = input.IsDeleted;
            status.ModifiedOn = DateTime.UtcNow;
            if (status.IsDeleted)
            {
                status.DeletedOn = DateTime.UtcNow;
            }

            try
            {
                this.jobApplicationStatusRepository.Update(status);
                await this.jobApplicationStatusRepository.SaveChangesAsync();
                return status.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
