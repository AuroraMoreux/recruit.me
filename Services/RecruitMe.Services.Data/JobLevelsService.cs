namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class JobLevelsService : IJobLevelsService
    {
        private readonly IDeletableEntityRepository<JobLevel> jobLevelsRepository;

        public JobLevelsService(IDeletableEntityRepository<JobLevel> jobLevelsRepository)
        {
            this.jobLevelsRepository = jobLevelsRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var jobLevels = this.jobLevelsRepository
                .All()
                .To<T>()
                .ToList();

            return jobLevels;
        }
    }
}
