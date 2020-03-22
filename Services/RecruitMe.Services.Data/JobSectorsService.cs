namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class JobSectorsService : IJobSectorsService
    {
        public JobSectorsService(IRepository<JobSector> jobSectorsRepository)
        {
            this.JobSectorsRepository = jobSectorsRepository;
        }

        public IRepository<JobSector> JobSectorsRepository { get; }

        public Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var jobSectors = this.JobSectorsRepository
                .All()
                .OrderBy(js => js.Name)
                .To<T>()
                .ToList();

            return jobSectors;
        }
    }
}
