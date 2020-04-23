namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Administration.JobSectors;

    public class JobSectorsService : IJobSectorsService
    {
        private readonly IDeletableEntityRepository<JobSector> jobSectorsRepository;

        public JobSectorsService(IDeletableEntityRepository<JobSector> jobSectorsRepository)
        {
            this.jobSectorsRepository = jobSectorsRepository;
        }

        public async Task<int> CreateAsync(CreateViewModel input)
        {
            var sector = AutoMapperConfig.MapperInstance.Map<JobSector>(input);

            if (sector.IsDeleted)
            {
                sector.DeletedOn = DateTime.UtcNow;
            }

            sector.CreatedOn = DateTime.UtcNow;
            try
            {
                await this.jobSectorsRepository.AddAsync(sector);
                await this.jobSectorsRepository.SaveChangesAsync();
                return sector.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sector = this.jobSectorsRepository
                  .All()
                  .Where(s => s.Id == id)
                  .FirstOrDefault();

            if (sector == null)
            {
                return false;
            }

            try
            {
                this.jobSectorsRepository.Delete(sector);
                await this.jobSectorsRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.jobSectorsRepository
                .AllAsNoTracking()
                .OrderBy(js => js.Name)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            return this.jobSectorsRepository
                .AllAsNoTrackingWithDeleted()
                .OrderBy(s => s.Name)
                .To<T>()
                .ToList();
        }

        public T GetDetails<T>(int id)
        {
            return this.jobSectorsRepository
                 .AllAsNoTrackingWithDeleted()
                 .Where(s => s.Id == id)
                 .To<T>()
                 .FirstOrDefault();
        }

        public async Task<int> UpdateAsync(int id,EditViewModel input)
        {
            var sector = this.jobSectorsRepository
                  .AllWithDeleted()
                  .Where(s => s.Id == id)
                  .FirstOrDefault();

            if (sector == null)
            {
                return -1;
            }

            sector.Name = input.Name;
            sector.IsDeleted = input.IsDeleted;
            sector.ModifiedOn = DateTime.UtcNow;
            if (sector.IsDeleted)
            {
                sector.DeletedOn = DateTime.UtcNow;
            }

            try
            {
                this.jobSectorsRepository.Update(sector);
                await this.jobSectorsRepository.SaveChangesAsync();
                return sector.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
