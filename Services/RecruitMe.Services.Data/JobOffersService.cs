namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.JobOffers;

    public class JobOffersService : IJobOffersService
    {
        private readonly IDeletableEntityRepository<JobOffer> jobOffersRepository;
        private readonly IDeletableEntityRepository<JobOfferLanguage> jobOfferLanguagesRepository;
        private readonly IDeletableEntityRepository<JobOfferSkill> jobOfferSkillsRepository;
        private readonly IDeletableEntityRepository<JobOfferJobType> jobOfferJobTypesRepository;

        public JobOffersService(
            IDeletableEntityRepository<JobOffer> jobOffersRepository,
            IDeletableEntityRepository<JobOfferLanguage> jobOfferLanguagesRepository,
            IDeletableEntityRepository<JobOfferSkill> jobOfferSkillsRepository,
            IDeletableEntityRepository<JobOfferJobType> jobOfferJobTypeRepository)
        {
            this.jobOffersRepository = jobOffersRepository;
            this.jobOfferLanguagesRepository = jobOfferLanguagesRepository;
            this.jobOfferSkillsRepository = jobOfferSkillsRepository;
            this.jobOfferJobTypesRepository = jobOfferJobTypeRepository;
        }

        public async Task<string> AddAsync(PostViewModel model, string employerId)
        {
            var offer = AutoMapperConfig.MapperInstance.Map<JobOffer>(model);
            offer.EmployerId = employerId;
            offer.CreatedOn = DateTime.UtcNow;

            var jobOfferLanguages = new List<JobOfferLanguage>();
            foreach (var languageId in model.LanguagesIds)
            {
                var jobOfferLanguage = new JobOfferLanguage
                {
                    JobOffer = offer,
                    LanguageId = languageId,
                    CreatedOn = DateTime.UtcNow,
                };
                jobOfferLanguages.Add(jobOfferLanguage);
            }

            var jobOfferSkills = new List<JobOfferSkill>();
            foreach (var skillId in model.SkillsIds)
            {
                var jobOfferSkill = new JobOfferSkill
                {
                    JobOffer = offer,
                    SkillId = skillId,
                    CreatedOn = DateTime.UtcNow,
                };
                jobOfferSkills.Add(jobOfferSkill);
            }

            var jobTypes = new List<JobOfferJobType>();
            foreach (var jobTypeId in model.JobTypesIds)
            {
                var jobOfferJobType = new JobOfferJobType
                {
                    JobOffer = offer,
                    JobTypeId = jobTypeId,
                    CreatedOn = DateTime.UtcNow,
                };
                jobTypes.Add(jobOfferJobType);
            }

            try
            {
                await this.jobOffersRepository.AddAsync(offer);
                await this.jobOfferSkillsRepository.AddRangeAsync(jobOfferSkills);
                await this.jobOfferLanguagesRepository.AddRangeAsync(jobOfferLanguages);
                await this.jobOfferJobTypesRepository.AddRangeAsync(jobTypes);
                await this.jobOffersRepository.SaveChangesAsync();
                await this.jobOfferSkillsRepository.SaveChangesAsync();
                await this.jobOfferLanguagesRepository.SaveChangesAsync();
                await this.jobOfferJobTypesRepository.SaveChangesAsync();
                return offer.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAllValidFilteredOffers<T>(FilterModel filters)
        {
            var offers = await Task.Run(() =>
           {
               var requestTime = DateTime.UtcNow;

               // build the query
               var baseQuery = this.jobOffersRepository
                     .AllAsNoTracking()
                     .Where(jo => jo.ValidFrom <= requestTime
                     && jo.ValidUntil >= requestTime);

               // nullable value filters
               var keywords = filters.Keywords != null ? filters.Keywords.ToLower() : string.Empty;
               var employerName = filters.Employer != null ? filters.Employer.ToLower() : string.Empty;
               var city = filters.City != null ? filters.City.ToLower() : string.Empty;

               baseQuery = baseQuery
               .Where(jo => (jo.Position.ToLower().Contains(keywords)
                   || jo.Description.ToLower().Contains(keywords))
                   && jo.Employer.Name.ToLower().Contains(employerName)
                   && jo.City.ToLower().Contains(city));

               // range filters
               var publishedFromDate = filters.ValidFrom != null ? filters.ValidFrom.Value.Date : DateTime.MinValue.Date;
               var publishedToDate = filters.ValidUntil != null ? filters.ValidUntil.Value.Date : DateTime.MaxValue.Date;

               baseQuery = baseQuery
                       .Where(jo => jo.ValidFrom >= publishedFromDate
                           && jo.ValidUntil <= publishedToDate);

               if (filters.SalaryFrom != null || filters.SalaryTo != null)
               {
                   var salaryFrom = filters.SalaryFrom != null ? filters.SalaryFrom.Value : 0m;
                   var salaryTo = filters.SalaryTo != null ? filters.SalaryTo.Value : decimal.MaxValue;

                   baseQuery = baseQuery
                       .Where(jo => jo.Salary >= salaryFrom
                           && jo.Salary <= salaryTo);
               }

               // select options filters
               if (filters.LevelsIds.Count() > 0)
               {
                   var jobOffersIdsWithSelectedLevels = this.jobOffersRepository
                       .AllAsNoTracking()
                       .Where(jo => filters.LevelsIds.Contains(jo.JobLevelId))
                       .Select(jo => jo.Id)
                       .ToList();

                   baseQuery = baseQuery
                       .Where(jo => jobOffersIdsWithSelectedLevels.Contains(jo.Id));
               }

               if (filters.SectorsIds.Count() > 0)
               {
                   var jobOffersIdsWithSelectedSectors = this.jobOffersRepository
                       .AllAsNoTracking()
                       .Where(jo => filters.SectorsIds.Contains(jo.JobLevelId))
                       .Select(jo => jo.Id)
                       .ToList();

                   baseQuery = baseQuery
                       .Where(jo => jobOffersIdsWithSelectedSectors.Contains(jo.Id));
               }

               if (filters.TypesIds.Count() > 0)
               {
                   var jobOffersIdsWithSelectedTypes = this.jobOfferJobTypesRepository
                       .AllAsNoTracking()
                       .Where(jojt => filters.TypesIds.Contains(jojt.JobTypeId))
                       .Select(jo => jo.JobOfferId)
                       .ToList();

                   baseQuery = baseQuery
                       .Where(jo => jobOffersIdsWithSelectedTypes.Contains(jo.Id));
               }

               if (filters.LanguagesIds.Count() > 0)
               {
                   var jobOffersIdsWithSelectedLanguages = this.jobOfferLanguagesRepository
                       .AllAsNoTracking()
                       .Where(jl => filters.LanguagesIds.Contains(jl.LanguageId))
                       .Select(jo => jo.JobOfferId)
                       .ToList();

                   baseQuery = baseQuery
                       .Where(jo => jobOffersIdsWithSelectedLanguages.Contains(jo.Id));
               }

               if (filters.SkillsIds.Count() > 0)
               {
                   var jobOffersIdsWithSelectedskills = this.jobOfferSkillsRepository
                       .AllAsNoTracking()
                       .Where(js => filters.SkillsIds.Contains(js.SkillId))
                       .Select(jo => jo.JobOfferId)
                       .ToList();

                   baseQuery = baseQuery
                       .Where(jo => jobOffersIdsWithSelectedskills.Contains(jo.Id));
               }

               var filteredOffers = baseQuery.To<T>().ToList();
               return filteredOffers;
           });

            return offers;
        }

        public bool IsOfferPostedByEmployer(string jobOfferId, string employerId)
        {
            return this.jobOffersRepository
                .AllAsNoTracking()
                .Any(jo => jo.Id == jobOfferId
                && jo.EmployerId == employerId);
        }

        public T GetDetails<T>(string jobOfferId)
        {
            return this.jobOffersRepository
                 .AllAsNoTracking()
                 .Where(jo => jo.Id == jobOfferId)
                 .To<T>()
                 .FirstOrDefault();
        }

        public bool IsPositionDuplicate(string employerId, string jobOfferPosition)
        {
            var currentDate = DateTime.UtcNow;

            return this.jobOffersRepository
                .AllAsNoTracking()
                .Any(jo => jo.Position.ToLower() == jobOfferPosition.ToLower()
                && jo.EmployerId == employerId
                && currentDate >= jo.ValidFrom
                && currentDate <= jo.ValidUntil);
        }

        public async Task<string> Update(EditViewModel model, string employerId)
        {
            var jobOffer = this.jobOffersRepository
                 .All()
                 .Where(jo => jo.Id == model.JobOfferDetails.Id)
                 .FirstOrDefault();

            var input = model.JobOfferDetails;

            jobOffer.Position = input.Position;
            jobOffer.Description = input.SanitizedDescription;
            jobOffer.Salary = input.Salary;
            jobOffer.City = input.City;
            jobOffer.OfficeAddress = input.OfficeAddress;
            jobOffer.JobSectorId = input.JobSectorId;
            jobOffer.JobLevelId = input.JobLevelId;

            var jobOfferSkillsIds = this.jobOfferSkillsRepository
                .AllAsNoTracking()
                .Where(jos => jos.JobOfferId == jobOffer.Id)
                .Select(jos => jos.SkillId)
                .ToList();

            // Add new ones
            foreach (var skillId in input.SkillsIds)
            {
                if (!jobOfferSkillsIds.Contains(skillId))
                {
                    var jobOfferSkill = new JobOfferSkill
                    {
                        SkillId = skillId,
                        JobOfferId = jobOffer.Id,
                        CreatedOn = DateTime.UtcNow,
                    };
                    await this.jobOfferSkillsRepository.AddAsync(jobOfferSkill);
                }
            }

            // Delete old ones
            foreach (var jobOfferSkillId in jobOfferSkillsIds)
            {
                if (!input.SkillsIds.Contains(jobOfferSkillId))
                {
                    var jobOfferSkill = this.jobOfferSkillsRepository
                        .All()
                        .Where(jos => jos.SkillId == jobOfferSkillId
                        && jos.JobOfferId == jobOffer.Id)
                        .FirstOrDefault();

                    this.jobOfferSkillsRepository.Delete(jobOfferSkill);
                }
            }

            var jobOfferLanguagesIds = this.jobOfferLanguagesRepository
                .AllAsNoTracking()
                .Where(jol => jol.JobOfferId == jobOffer.Id)
                .Select(jol => jol.LanguageId)
                .ToList();

            // Add new ones
            foreach (var languageId in input.LanguagesIds)
            {
                if (!jobOfferLanguagesIds.Contains(languageId))
                {
                    var jobOfferLanguage = new JobOfferLanguage
                    {
                        LanguageId = languageId,
                        JobOfferId = jobOffer.Id,
                        CreatedOn = DateTime.UtcNow,
                    };
                    await this.jobOfferLanguagesRepository.AddAsync(jobOfferLanguage);
                }
            }

            // Delete old ones
            foreach (var jobOfferLanguageId in jobOfferLanguagesIds)
            {
                if (!input.LanguagesIds.Contains(jobOfferLanguageId))
                {
                    var jobOfferLanguage = this.jobOfferLanguagesRepository
                        .All()
                        .Where(jos => jos.LanguageId == jobOfferLanguageId
                        && jos.JobOfferId == jobOffer.Id)
                        .FirstOrDefault();

                    this.jobOfferLanguagesRepository.Delete(jobOfferLanguage);
                }
            }

            var jobOfferTypesIds = this.jobOfferJobTypesRepository
                .AllAsNoTracking()
                .Where(jojt => jojt.JobOfferId == jobOffer.Id)
                .Select(jojt => jojt.JobTypeId)
                .ToList();

            // Add new ones
            foreach (var jobTypeId in input.JobTypesIds)
            {
                if (!jobOfferTypesIds.Contains(jobTypeId))
                {
                    var jobOfferType = new JobOfferJobType
                    {
                        JobTypeId = jobTypeId,
                        JobOfferId = jobOffer.Id,
                        CreatedOn = DateTime.UtcNow,
                    };
                    await this.jobOfferJobTypesRepository.AddAsync(jobOfferType);
                }
            }

            // Delete old ones
            foreach (var jobOfferTypeId in jobOfferTypesIds)
            {
                if (!input.JobTypesIds.Contains(jobOfferTypeId))
                {
                    var jobOfferJobType = this.jobOfferJobTypesRepository
                        .All()
                        .Where(jos => jos.JobTypeId == jobOfferTypeId
                        && jos.JobOfferId == jobOffer.Id)
                        .FirstOrDefault();

                    this.jobOfferJobTypesRepository.Delete(jobOfferJobType);
                }
            }

            try
            {
                this.jobOffersRepository.Update(jobOffer);
                await this.jobOffersRepository.SaveChangesAsync();
                await this.jobOfferSkillsRepository.SaveChangesAsync();
                await this.jobOfferLanguagesRepository.SaveChangesAsync();
                await this.jobOfferJobTypesRepository.SaveChangesAsync();
                return jobOffer.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string jobOfferId)
        {
            var jobOffer = this.jobOffersRepository
                .All()
                .Where(jo => jo.Id == jobOfferId)
                .FirstOrDefault();

            if (jobOffer == null)
            {
                return false;
            }

            try
            {
                this.jobOffersRepository.Delete(jobOffer);
                await this.jobOffersRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetCount()
        {
            var requestTime = DateTime.UtcNow;
            return this.jobOffersRepository
                 .AllAsNoTracking()
                 .Where(jo => jo.ValidFrom <= requestTime
                     && jo.ValidUntil >= requestTime)
                 .Count();
        }

        public int GetNewOffersCount()
        {
            var yesterdaysDate = DateTime.UtcNow.AddDays(-1);
            return this.jobOffersRepository
                .AllAsNoTracking()
                .Where(jo => jo.CreatedOn >= yesterdaysDate)
                .Count();
        }

        public IEnumerable<T> GetEmployerJobOffers<T>(string employerId)
        {
            var requestTime = DateTime.UtcNow;

            return this.jobOffersRepository
                 .AllAsNoTracking()
                 .Where(jo => jo.EmployerId == employerId
                     && jo.ValidFrom <= requestTime
                     && jo.ValidUntil >= requestTime)
                 .OrderByDescending(jo => jo.CreatedOn)
                 .To<T>()
                 .ToList();
        }

        public string GetOfferPositionById(string jobOfferId)
        {
            return this.jobOffersRepository
                .AllAsNoTracking()
                .Where(jo => jo.Id == jobOfferId)
                .Select(jo => jo.Position)
                .FirstOrDefault();
        }

        public IEnumerable<T> GetLastTenOffers<T>()
        {
            var requestTime = DateTime.UtcNow;
            return this.jobOffersRepository
                 .AllAsNoTracking()
                 .Where(jo => jo.ValidFrom <= requestTime
                     && jo.ValidUntil >= requestTime)
                 .OrderByDescending(jo => jo.CreatedOn)
                 .Take(10)
                 .To<T>()
                 .ToList();
        }
    }
}
