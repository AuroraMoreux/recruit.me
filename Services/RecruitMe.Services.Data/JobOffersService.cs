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

        public JobOffersService(IDeletableEntityRepository<JobOffer> jobOffersRepository, IDeletableEntityRepository<JobOfferLanguage> jobOfferLanguagesRepository, IDeletableEntityRepository<JobOfferSkill> jobOfferSkillsRepository, IDeletableEntityRepository<JobOfferJobType> jobOfferJobTypeRepository)
        {
            this.jobOffersRepository = jobOffersRepository;
            this.jobOfferLanguagesRepository = jobOfferLanguagesRepository;
            this.jobOfferSkillsRepository = jobOfferSkillsRepository;
            this.jobOfferJobTypeRepository = jobOfferJobTypeRepository;
        }

        public async Task<string> AddOffer(PostViewModel model, string employerId)
        {
            JobOffer offer = AutoMapperConfig.MapperInstance.Map<JobOffer>(model);
            offer.EmployerId = employerId;
            offer.CreatedOn = DateTime.UtcNow;
            await this.jobOffersRepository.AddAsync(offer);

            List<JobOfferLanguage> jobOfferLanguages = new List<JobOfferLanguage>();
            foreach (int languageId in model.LanguagesIds)
            {
                JobOfferLanguage jobOfferLanguage = new JobOfferLanguage
                {
                    JobOffer = offer,
                    LanguageId = languageId,
                };
                jobOfferLanguages.Add(jobOfferLanguage);
            }

            await this.jobOfferLanguagesRepository.AddRangeAsync(jobOfferLanguages);

            List<JobOfferSkill> jobOfferSkills = new List<JobOfferSkill>();
            foreach (int skillId in model.SkillsIds)
            {
                JobOfferSkill jobOfferSkill = new JobOfferSkill
                {
                    JobOffer = offer,
                    SkillId = skillId,
                };
                jobOfferSkills.Add(jobOfferSkill);
            }

            await this.jobOfferSkillsRepository.AddRangeAsync(jobOfferSkills);

            List<JobOfferJobType> jobTypes = new List<JobOfferJobType>();
            foreach (int jobTypeId in model.JobTypesOptions.Where(jt => jt.Selected).Select(jt => jt.Id))
            {
                JobOfferJobType jobOfferJobType = new JobOfferJobType
                {
                    JobOffer = offer,
                    JobTypeId = jobTypeId,
                };
                jobTypes.Add(jobOfferJobType);
            }

            await this.jobOfferJobTypeRepository.AddRangeAsync(jobTypes);
            await this.jobOffersRepository.SaveChangesAsync();
            await this.jobOfferSkillsRepository.SaveChangesAsync();
            await this.jobOfferLanguagesRepository.SaveChangesAsync();
            await this.jobOfferJobTypeRepository.SaveChangesAsync();

            return offer.Id;
        }

        public IEnumerable<T> GetAllValidOffers<T>()
        {
            DateTime requestTime = DateTime.UtcNow;

            List<T> offers = this.jobOffersRepository
                .All()
                .Where(jo => jo.ValidFrom <= requestTime
                && jo.ValidUntil >= requestTime)
                .OrderByDescending(jo => jo.CreatedOn)
                .To<T>()
                .ToList();

            return offers;
        }

        public bool IsJobOfferPostedByEmployer(string jobOfferId, string employerId)
        {
            return this.jobOffersRepository
                .AllAsNoTracking()
                .Any(jo => jo.Id == jobOfferId
                && jo.EmployerId == employerId);
        }

        public T GetOfferDetails<T>(string jobOfferId)
        {
            T offer = this.jobOffersRepository
                 .All()
                 .Where(jo => jo.Id == jobOfferId)
                 .To<T>()
                 .FirstOrDefault();

            return offer;
        }

        public bool IsOfferTitleDuplicate(string employerId, string jobOfferTitle)
        {
            var currentDate = DateTime.UtcNow.Date;

            return this.jobOffersRepository
                .AllAsNoTracking()
                .Any(jo => jo.Title == jobOfferTitle
                && jo.EmployerId == employerId
                && currentDate >= jo.ValidFrom
                && currentDate <= jo.ValidUntil);
        }

        public async Task<string> UpdateOffer(EditViewModel model, string employerId)
        {
            // TODO: Add CreatedOn on all entities in create methods
            var jobOffer = this.jobOffersRepository
                 .All()
                 .Where(jo => jo.Id == model.JobOfferDetails.Id)
                 .FirstOrDefault();

            var input = model.JobOfferDetails;

            jobOffer.Title = input.Title;
            jobOffer.Description = input.SanitizedDescription;
            jobOffer.Salary = input.Salary;
            jobOffer.City = input.City;
            jobOffer.OfficeAddress = input.OfficeAddress;
            jobOffer.IsFullTime = input.IsFullTime;
            jobOffer.IsRemote = input.IsRemote;
            jobOffer.JobSectorId = input.JobSectorId;
            jobOffer.JobLevelId = input.JobLevelId;

            this.jobOffersRepository.Update(jobOffer);

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

            var selectedJobTypes = model.JobTypesOptions.Where(jto => jto.Selected).Select(jto => jto.Id).ToList();

            var jobOfferTypesIds = this.jobOfferJobTypeRepository
                .AllAsNoTracking()
                .Where(jojt => jojt.JobOfferId == jobOffer.Id)
                .Select(jojt => jojt.JobTypeId)
                .ToList();

            // Add new ones
            foreach (var jobTypeId in selectedJobTypes)
            {
                if (!jobOfferTypesIds.Contains(jobTypeId))
                {
                    var jobOfferType = new JobOfferJobType
                    {
                        JobTypeId = jobTypeId,
                        JobOfferId = jobOffer.Id,
                    };
                    await this.jobOfferJobTypeRepository.AddAsync(jobOfferType);
                }
            }

            // Delete old ones
            foreach (var jobOfferTypeId in jobOfferTypesIds)
            {
                if (!selectedJobTypes.Contains(jobOfferTypeId))
                {
                    var jobOfferJobType = this.jobOfferJobTypeRepository
                        .All()
                        .Where(jos => jos.JobTypeId == jobOfferTypeId
                        && jos.JobOfferId == jobOffer.Id)
                        .FirstOrDefault();

                    this.jobOfferJobTypeRepository.Delete(jobOfferJobType);
                }
            }

            // TODO : Check all saveChanges are at the end to avoid data being saved when there is an exception
            await this.jobOffersRepository.SaveChangesAsync();
            await this.jobOfferSkillsRepository.SaveChangesAsync();
            await this.jobOfferLanguagesRepository.SaveChangesAsync();
            await this.jobOfferJobTypeRepository.SaveChangesAsync();

            return jobOffer.Id;
        }

        public async Task DeleteOffer(string jobOfferId)
        {
            var jobOffer = this.jobOffersRepository
                .All()
                .Where(jo => jo.Id == jobOfferId)
                .FirstOrDefault();

            this.jobOffersRepository.Delete(jobOffer);
            await this.jobOffersRepository.SaveChangesAsync();
        }
    }
}
