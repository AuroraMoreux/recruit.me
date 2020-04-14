namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class JobApplicationStatusesService : IJobApplicationStatusesService
    {
        private readonly IDeletableEntityRepository<JobApplicationStatus> jobApplicationStatusRepository;

        public JobApplicationStatusesService(IDeletableEntityRepository<JobApplicationStatus> jobApplicationStatusRepository)
        {
            this.jobApplicationStatusRepository = jobApplicationStatusRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.jobApplicationStatusRepository
                  .AllAsNoTracking()
                  .To<T>()
                  .ToList();
        }
    }
}
