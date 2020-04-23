namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Services;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Candidates;

    public class CandidatesService : ICandidatesService
    {
        private const string PictureNameAddIn = "_profilePicture";
        private readonly IDeletableEntityRepository<Candidate> candidatesRepository;
        private readonly IDeletableEntityRepository<CandidateLanguage> candidateLanguagesRepository;
        private readonly IDeletableEntityRepository<CandidateSkill> candidateSkillsRepository;
        private readonly Cloudinary cloudinary;

        public CandidatesService(
            IDeletableEntityRepository<Candidate> candidatesRepository,
            IDeletableEntityRepository<CandidateLanguage> candidateLanguagesRepository,
            IDeletableEntityRepository<CandidateSkill> candidateSkillsRepository,
            Cloudinary cloudinary)
        {
            this.candidatesRepository = candidatesRepository;
            this.candidateLanguagesRepository = candidateLanguagesRepository;
            this.candidateSkillsRepository = candidateSkillsRepository;
            this.cloudinary = cloudinary;
        }

        public async Task<string> CreateProfileAsync(CreateCandidateProfileInputModel model)
        {
            var candidate = AutoMapperConfig.MapperInstance.Map<Candidate>(model);

            if (model.ProfilePicture != null)
            {
                var pictureUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.ProfilePicture, model.ApplicationUserId + PictureNameAddIn);
                if (pictureUrl == null)
                {
                    return null;
                }

                candidate.ProfilePictureUrl = pictureUrl;
            }

            candidate.CreatedOn = DateTime.UtcNow;

            var candidateLanguages = new List<CandidateLanguage>();
            foreach (var languageId in model.LanguagesIds)
            {
                var language = new CandidateLanguage
                {
                    Candidate = candidate,
                    LanguageId = languageId,
                    CreatedOn = DateTime.UtcNow,
                };
                candidateLanguages.Add(language);
            }

            var candidateSkills = new List<CandidateSkill>();
            foreach (var skillId in model.SkillsIds)
            {
                var skill = new CandidateSkill
                {
                    Candidate = candidate,
                    SkillId = skillId,
                    CreatedOn = DateTime.UtcNow,
                };
                candidateSkills.Add(skill);
            }

            try
            {
                await this.candidatesRepository.AddAsync(candidate);
                await this.candidateLanguagesRepository.AddRangeAsync(candidateLanguages);
                await this.candidateSkillsRepository.AddRangeAsync(candidateSkills);
                await this.candidatesRepository.SaveChangesAsync();
                await this.candidateLanguagesRepository.SaveChangesAsync();
                await this.candidateSkillsRepository.SaveChangesAsync();
                return candidate.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetCandidateIdByUsername(string username)
        {
            return this.candidatesRepository
                  .AllAsNoTracking()
                  .Where(c => c.ApplicationUser.UserName == username)
                  .Select(c => c.Id)
                  .FirstOrDefault();
        }

        public int GetCount()
        {
            return this.candidatesRepository
                .AllAsNoTracking()
                .Count();
        }

        public int GetNewCandidatesCount()
        {
            var yesterdaysDate = DateTime.UtcNow.AddDays(-1);
            return this.candidatesRepository
                .AllAsNoTracking()
                .Where(c => c.CreatedOn >= yesterdaysDate)
                .Count();
        }

        public T GetProfileDetails<T>(string candidateId)
        {
            return this.candidatesRepository
                 .AllAsNoTracking()
                 .Where(e => e.Id == candidateId)
                 .To<T>()
                 .FirstOrDefault();
        }

        public async Task<string> UpdateProfileAsync(string candidateId, UpdateCandidateProfileViewModel model)
        {
            var candidate = this.candidatesRepository
                .All()
                .FirstOrDefault(c => c.Id == candidateId);

            if (candidate == null)
            {
                return null;
            }

            candidate.FirstName = model.FirstName;
            candidate.LastName = model.LastName;
            candidate.PhoneNumber = model.PhoneNumber;
            candidate.ContactAddress = model.ContactAddress;
            candidate.Education = model.Education;
            candidate.AboutMe = model.SanitizedAboutMe;

            if (model.ProfilePicture != null)
            {
                if (candidate.ProfilePictureUrl != null)
                {
                    CloudinaryService.DeleteFile(this.cloudinary, model.ApplicationUserId + PictureNameAddIn);
                }

                var pictureUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.ProfilePicture, model.ApplicationUserId + PictureNameAddIn);

                if (pictureUrl == null)
                {
                    return null;
                }

                candidate.ProfilePictureUrl = pictureUrl;
            }

            candidate.ModifiedOn = DateTime.UtcNow;

            var candidateLanguagesIds = this.candidateLanguagesRepository
               .AllAsNoTracking()
               .Where(cl => cl.CandidateId == candidateId)
               .Select(cl => cl.LanguageId)
               .ToList();

            // Add new ones
            foreach (var languageId in model.LanguagesIds)
            {
                if (!candidateLanguagesIds.Contains(languageId))
                {
                    var language = new CandidateLanguage
                    {
                        LanguageId = languageId,
                        CandidateId = candidate.Id,
                        CreatedOn = DateTime.UtcNow,
                    };
                    await this.candidateLanguagesRepository.AddAsync(language);
                }
            }

            // Delete old ones
            foreach (var languageId in candidateLanguagesIds)
            {
                if (!model.LanguagesIds.Contains(languageId))
                {
                    var languages = this.candidateLanguagesRepository
                        .All()
                        .Where(cl => cl.LanguageId == languageId
                        && cl.CandidateId == candidate.Id)
                        .FirstOrDefault();

                    this.candidateLanguagesRepository.Delete(languages);
                }
            }

            var candidateSkillsIds = this.candidateSkillsRepository
              .AllAsNoTracking()
              .Where(cs => cs.CandidateId == candidateId)
              .Select(cs => cs.SkillId)
              .ToList();

            // Add new ones
            foreach (var skillId in model.SkillsIds)
            {
                if (!candidateSkillsIds.Contains(skillId))
                {
                    var skill = new CandidateSkill
                    {
                        SkillId = skillId,
                        CandidateId = candidate.Id,
                        CreatedOn = DateTime.UtcNow,
                    };
                    await this.candidateSkillsRepository.AddAsync(skill);
                }
            }

            // Delete old ones
            foreach (var skillId in candidateSkillsIds)
            {
                if (!model.SkillsIds.Contains(skillId))
                {
                    var skills = this.candidateSkillsRepository
                        .All()
                        .Where(cs => cs.SkillId == skillId
                        && cs.CandidateId == candidate.Id)
                        .FirstOrDefault();

                    this.candidateSkillsRepository.Delete(skills);
                }
            }

            try
            {
                this.candidatesRepository.Update(candidate);
                await this.candidatesRepository.SaveChangesAsync();
                await this.candidateLanguagesRepository.SaveChangesAsync();
                await this.candidateSkillsRepository.SaveChangesAsync();

                return candidate.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
