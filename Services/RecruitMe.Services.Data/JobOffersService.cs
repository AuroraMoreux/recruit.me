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
            var offer = AutoMapperConfig.MapperInstance.Map<JobOffer>(model);
            offer.EmployerId = employerId;
            await this.jobOffersRepository.AddAsync(offer);
            await this.jobOffersRepository.SaveChangesAsync();

            var jobOfferLanguages = new List<JobOfferLanguage>();
            foreach (var languageId in model.LanguagesIds)
            {
                var jobOfferLanguage = new JobOfferLanguage
                {
                    JobOffer = offer,
                    LanguageId = languageId,
                };
                jobOfferLanguages.Add(jobOfferLanguage);
            }

            await this.jobOfferLanguagesRepository.AddRangeAsync(jobOfferLanguages);
            await this.jobOfferLanguagesRepository.SaveChangesAsync();

            var jobOfferSkills = new List<JobOfferSkill>();
            foreach (var skillId in model.SkillsIds)
            {
                var jobOfferSkill = new JobOfferSkill
                {
                    JobOffer = offer,
                    SkillId = skillId,
                };
                jobOfferSkills.Add(jobOfferSkill);
            }

            await this.jobOfferSkillsRepository.AddRangeAsync(jobOfferSkills);
            await this.jobOfferSkillsRepository.SaveChangesAsync();

            var jobTypes = new List<JobOfferJobType>();
            foreach (var jobTypeId in model.JobTypesOptions.Where(jt => jt.Selected).Select(jt => jt.Id))
            {
                var jobOfferJobType = new JobOfferJobType
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
            var requestTime = DateTime.UtcNow;

            var offers = this.jobOffersRepository
                .All()
                .Where(jo => jo.ValidFrom <= requestTime
                && jo.ValidUntil >= requestTime)
                .OrderByDescending(jo => jo.CreatedOn)
                .To<T>()
                .ToList();

            return offers;
        }

        public T GetOfferDetails<T>(string jobOfferId)
        {
            var offer = this.jobOffersRepository
                 .All()
                 .Where(jo => jo.Id == jobOfferId)
                 .To<T>()
                 .FirstOrDefault();

            return offer;
        }
    }
}
