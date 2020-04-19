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
        private readonly IDeletableEntityRepository<JobOfferJobType> jobOfferJobTypeRepository;

        public JobOffersService(
            IDeletableEntityRepository<JobOffer> jobOffersRepository,
            IDeletableEntityRepository<JobOfferLanguage> jobOfferLanguagesRepository,
            IDeletableEntityRepository<JobOfferSkill> jobOfferSkillsRepository,
            IDeletableEntityRepository<JobOfferJobType> jobOfferJobTypeRepository)
        {
            this.jobOffersRepository = jobOffersRepository;
            this.jobOfferLanguagesRepository = jobOfferLanguagesRepository;
            this.jobOfferSkillsRepository = jobOfferSkillsRepository;
            this.jobOfferJobTypeRepository = jobOfferJobTypeRepository;
        }

        public async Task<string> Add(PostViewModel model, string employerId)
        {
            JobOffer offer = AutoMapperConfig.MapperInstance.Map<JobOffer>(model);
            offer.EmployerId = employerId;
            offer.CreatedOn = DateTime.UtcNow;

            List<JobOfferLanguage> jobOfferLanguages = new List<JobOfferLanguage>();
            foreach (int languageId in model.LanguagesIds)
            {
                JobOfferLanguage jobOfferLanguage = new JobOfferLanguage
                {
                    JobOffer = offer,
                    LanguageId = languageId,
                    CreatedOn = DateTime.UtcNow,
                };
                jobOfferLanguages.Add(jobOfferLanguage);
            }

            List<JobOfferSkill> jobOfferSkills = new List<JobOfferSkill>();
            foreach (int skillId in model.SkillsIds)
            {
                JobOfferSkill jobOfferSkill = new JobOfferSkill
                {
                    JobOffer = offer,
                    SkillId = skillId,
                    CreatedOn = DateTime.UtcNow,
                };
                jobOfferSkills.Add(jobOfferSkill);
            }

            List<JobOfferJobType> jobTypes = new List<JobOfferJobType>();
            foreach (int jobTypeId in model.JobTypesIds)
            {
                JobOfferJobType jobOfferJobType = new JobOfferJobType
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
                await this.jobOfferJobTypeRepository.AddRangeAsync(jobTypes);
                await this.jobOffersRepository.SaveChangesAsync();
                await this.jobOfferSkillsRepository.SaveChangesAsync();
                await this.jobOfferLanguagesRepository.SaveChangesAsync();
                await this.jobOfferJobTypeRepository.SaveChangesAsync();
                return offer.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAllValidFilteredOffers<T>(FilterModel filters)
        {
            List<T> offers = await Task.Run(() =>
           {
               DateTime requestTime = DateTime.UtcNow;

               // build the query
               IQueryable<JobOffer> baseQuery = this.jobOffersRepository
                     .AllAsNoTracking()
                     .Where(jo => jo.ValidFrom <= requestTime
                     && jo.ValidUntil >= requestTime);

               // nullable value filters
               string keywords = filters.Keywords != null ? filters.Keywords.ToLower() : string.Empty;
               string employerName = filters.Employer != null ? filters.Employer.ToLower() : string.Empty;
               string city = filters.City != null ? filters.City.ToLower() : string.Empty;
               baseQuery = baseQuery

                   .Where(jo => (jo.Title.ToLower().Contains(keywords)
                       || jo.Description.ToLower().Contains(keywords))
                       && jo.Employer.Name.ToLower().Contains(employerName)
                       && jo.City.ToLower().Contains(city));

               // range filters
               DateTime publishedFromDate = filters.PublishedFromDate != null ? filters.PublishedFromDate.Value.Date : DateTime.MinValue.Date;
               DateTime publishedToDate = filters.PublishedToDate != null ? filters.PublishedToDate.Value.Date : DateTime.MaxValue.Date;
               decimal salaryFrom = filters.SalaryFrom != null ? filters.SalaryFrom.Value : 0m;
               decimal salaryTo = filters.SalaryTo != null ? filters.SalaryTo.Value : decimal.MaxValue;

               baseQuery = baseQuery
                   .Where(jo => jo.ValidFrom >= publishedFromDate
                       && jo.ValidUntil <= publishedToDate
                       && jo.Salary >= salaryFrom
                       && jo.Salary <= salaryTo);

               // select options filters
               if (filters.LevelsIds.Count > 0)
               {
                   List<string> jobOffersIdsWithSelectedLevels = this.jobOffersRepository
                       .AllAsNoTracking()
                       .Where(jo => filters.LevelsIds.Contains(jo.JobLevelId))
                       .Select(jo => jo.Id)
                       .ToList();

                   baseQuery = baseQuery
                       .Where(jo => jobOffersIdsWithSelectedLevels.Contains(jo.Id));
               }

               if (filters.SectorsIds.Count > 0)
               {
                   List<string> jobOffersIdsWithSelectedSectors = this.jobOffersRepository
                       .AllAsNoTracking()
                       .Where(jo => filters.SectorsIds.Contains(jo.JobLevelId))
                       .Select(jo => jo.Id)
                       .ToList();

                   baseQuery = baseQuery
                       .Where(jo => jobOffersIdsWithSelectedSectors.Contains(jo.Id));
               }

               if (filters.TypesIds.Count > 0)
               {
                   List<string> jobOffersIdsWithSelectedTypes = this.jobOfferJobTypeRepository
                       .AllAsNoTracking()
                       .Where(jojt => filters.TypesIds.Contains(jojt.JobTypeId))
                       .Select(jo => jo.JobOfferId)
                       .ToList();

                   baseQuery = baseQuery
                       .Where(jo => jobOffersIdsWithSelectedTypes.Contains(jo.Id));
               }

               if (filters.LanguagesIds.Count > 0)
               {
                   List<string> jobOffersIdsWithSelectedLanguages = this.jobOfferLanguagesRepository
                       .AllAsNoTracking()
                       .Where(jl => filters.LanguagesIds.Contains(jl.LanguageId))
                       .Select(jo => jo.JobOfferId)
                       .ToList();

                   baseQuery = baseQuery
                       .Where(jo => jobOffersIdsWithSelectedLanguages.Contains(jo.Id));
               }

               if (filters.SkillsIds.Count > 0)
               {
                   List<string> jobOffersIdsWithSelectedskills = this.jobOfferSkillsRepository
                       .AllAsNoTracking()
                       .Where(js => filters.SkillsIds.Contains(js.SkillId))
                       .Select(jo => jo.JobOfferId)
                       .ToList();

                   baseQuery = baseQuery
                       .Where(jo => jobOffersIdsWithSelectedskills.Contains(jo.Id));
               }

               List<T> filteredOffers = baseQuery.To<T>().ToList();
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

        public bool IsTitleDuplicate(string employerId, string jobOfferTitle)
        {
            DateTime currentDate = DateTime.UtcNow.Date;

            return this.jobOffersRepository
                .AllAsNoTracking()
                .Any(jo => jo.Title == jobOfferTitle
                && jo.EmployerId == employerId
                && currentDate >= jo.ValidFrom
                && currentDate <= jo.ValidUntil);
        }

        public async Task<string> Update(EditViewModel model, string employerId)
        {
            JobOffer jobOffer = this.jobOffersRepository
                 .All()
                 .Where(jo => jo.Id == model.JobOfferDetails.Id)
                 .FirstOrDefault();

            EditJobOfferDetailsModel input = model.JobOfferDetails;

            jobOffer.Title = input.Title;
            jobOffer.Description = input.SanitizedDescription;
            jobOffer.Salary = input.Salary;
            jobOffer.City = input.City;
            jobOffer.OfficeAddress = input.OfficeAddress;
            jobOffer.JobSectorId = input.JobSectorId;
            jobOffer.JobLevelId = input.JobLevelId;

            this.jobOffersRepository.Update(jobOffer);

            List<int> jobOfferSkillsIds = this.jobOfferSkillsRepository
                .AllAsNoTracking()
                .Where(jos => jos.JobOfferId == jobOffer.Id)
                .Select(jos => jos.SkillId)
                .ToList();

            // Add new ones
            foreach (int skillId in input.SkillsIds)
            {
                if (!jobOfferSkillsIds.Contains(skillId))
                {
                    JobOfferSkill jobOfferSkill = new JobOfferSkill
                    {
                        SkillId = skillId,
                        JobOfferId = jobOffer.Id,
                        CreatedOn = DateTime.UtcNow,
                    };
                    await this.jobOfferSkillsRepository.AddAsync(jobOfferSkill);
                }
            }

            // Delete old ones
            foreach (int jobOfferSkillId in jobOfferSkillsIds)
            {
                if (!input.SkillsIds.Contains(jobOfferSkillId))
                {
                    JobOfferSkill jobOfferSkill = this.jobOfferSkillsRepository
                        .All()
                        .Where(jos => jos.SkillId == jobOfferSkillId
                        && jos.JobOfferId == jobOffer.Id)
                        .FirstOrDefault();

                    this.jobOfferSkillsRepository.Delete(jobOfferSkill);
                }
            }

            List<int> jobOfferLanguagesIds = this.jobOfferLanguagesRepository
                .AllAsNoTracking()
                .Where(jol => jol.JobOfferId == jobOffer.Id)
                .Select(jol => jol.LanguageId)
                .ToList();

            // Add new ones
            foreach (int languageId in input.LanguagesIds)
            {
                if (!jobOfferLanguagesIds.Contains(languageId))
                {
                    JobOfferLanguage jobOfferLanguage = new JobOfferLanguage
                    {
                        LanguageId = languageId,
                        JobOfferId = jobOffer.Id,
                        CreatedOn = DateTime.UtcNow,
                    };
                    await this.jobOfferLanguagesRepository.AddAsync(jobOfferLanguage);
                }
            }

            // Delete old ones
            foreach (int jobOfferLanguageId in jobOfferLanguagesIds)
            {
                if (!input.LanguagesIds.Contains(jobOfferLanguageId))
                {
                    JobOfferLanguage jobOfferLanguage = this.jobOfferLanguagesRepository
                        .All()
                        .Where(jos => jos.LanguageId == jobOfferLanguageId
                        && jos.JobOfferId == jobOffer.Id)
                        .FirstOrDefault();

                    this.jobOfferLanguagesRepository.Delete(jobOfferLanguage);
                }
            }

            List<int> jobOfferTypesIds = this.jobOfferJobTypeRepository
                .AllAsNoTracking()
                .Where(jojt => jojt.JobOfferId == jobOffer.Id)
                .Select(jojt => jojt.JobTypeId)
                .ToList();

            // Add new ones
            foreach (int jobTypeId in input.JobTypesIds)
            {
                if (!jobOfferTypesIds.Contains(jobTypeId))
                {
                    JobOfferJobType jobOfferType = new JobOfferJobType
                    {
                        JobTypeId = jobTypeId,
                        JobOfferId = jobOffer.Id,
                        CreatedOn = DateTime.UtcNow,
                    };
                    await this.jobOfferJobTypeRepository.AddAsync(jobOfferType);
                }
            }

            // Delete old ones
            foreach (int jobOfferTypeId in jobOfferTypesIds)
            {
                if (!input.JobTypesIds.Contains(jobOfferTypeId))
                {
                    JobOfferJobType jobOfferJobType = this.jobOfferJobTypeRepository
                        .All()
                        .Where(jos => jos.JobTypeId == jobOfferTypeId
                        && jos.JobOfferId == jobOffer.Id)
                        .FirstOrDefault();

                    this.jobOfferJobTypeRepository.Delete(jobOfferJobType);
                }
            }

            try
            {
                await this.jobOffersRepository.SaveChangesAsync();
                await this.jobOfferSkillsRepository.SaveChangesAsync();
                await this.jobOfferLanguagesRepository.SaveChangesAsync();
                await this.jobOfferJobTypeRepository.SaveChangesAsync();
                return jobOffer.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> Delete(string jobOfferId)
        {
            JobOffer jobOffer = this.jobOffersRepository
                .All()
                .Where(jo => jo.Id == jobOfferId)
                .FirstOrDefault();

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
            return this.jobOffersRepository
                 .AllAsNoTracking()
                 .Count();
        }

        public int GetNewOffersCount()
        {
            DateTime yesterdaysDate = DateTime.UtcNow.AddDays(-1).Date;
            return this.jobOffersRepository
                .AllAsNoTracking()
                .Where(jo => jo.CreatedOn >= yesterdaysDate)
                .Count();
        }
    }
}
