namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class JobTypesService : IJobTypesService
    {
        private readonly IDeletableEntityRepository<JobType> jobTypesRepository;

        public JobTypesService(IDeletableEntityRepository<JobType> jobTypesRepository)
        {
            this.jobTypesRepository = jobTypesRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            List<T> jobTypes = this.jobTypesRepository
                .All()
                .OrderBy(jt => jt.Name)
                .To<T>()
                .ToList();

            return jobTypes;
        }
    }
}
