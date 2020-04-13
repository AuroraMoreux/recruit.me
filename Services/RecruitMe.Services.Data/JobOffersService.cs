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
            await this.jobOffersRepository.AddAsync(offer);
            await this.jobOffersRepository.SaveChangesAsync();

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
            await this.jobOfferLanguagesRepository.SaveChangesAsync();

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
            await this.jobOfferSkillsRepository.SaveChangesAsync();

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
    }
}
