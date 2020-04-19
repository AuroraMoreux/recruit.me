namespace RecruitMe.Services.Data
{
    using System;
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
        private readonly Cloudinary cloudinary;

        public CandidatesService(
            IDeletableEntityRepository<Candidate> candidatesRepository,
            Cloudinary cloudinary)
        {
            this.candidatesRepository = candidatesRepository;
            this.cloudinary = cloudinary;
        }

        public async Task<string> CreateProfileAsync(CreateCandidateProfileInputModel model)
        {
            Candidate candidate = AutoMapperConfig.MapperInstance.Map<Candidate>(model);

            if (model.ProfilePicture != null)
            {
                string pictureUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.ProfilePicture, model.ApplicationUserId + PictureNameAddIn);
                if (pictureUrl == null)
                {
                    return null;
                }

                candidate.ProfilePictureUrl = pictureUrl;
            }

            candidate.CreatedOn = DateTime.UtcNow;
            try
            {
                await this.candidatesRepository.AddAsync(candidate);
                await this.candidatesRepository.SaveChangesAsync();
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
            DateTime yesterdaysDate = DateTime.UtcNow.AddDays(-1).Date;
            return this.candidatesRepository
                .AllAsNoTracking()
                .Where(c => c.ApplicationUser.CreatedOn >= yesterdaysDate)
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
            Candidate candidate = this.candidatesRepository
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

            if (model.ProfilePicture != null)
            {
                if (candidate.ProfilePictureUrl != null)
                {
                    CloudinaryService.DeleteFile(this.cloudinary, model.ApplicationUserId + PictureNameAddIn);
                }

                string pictureUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.ProfilePicture, model.ApplicationUserId + PictureNameAddIn);

                if (pictureUrl == null)
                {
                    return null;
                }

                candidate.ProfilePictureUrl = pictureUrl;
            }

            candidate.ModifiedOn = DateTime.UtcNow;
            try
            {
                this.candidatesRepository.Update(candidate);
                await this.candidatesRepository.SaveChangesAsync();
                return candidate.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
