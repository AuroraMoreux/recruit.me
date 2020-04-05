namespace RecruitMe.Services.Data
{
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

        public CandidatesService(IDeletableEntityRepository<Candidate> candidatesRepository, Cloudinary cloudinary)
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
                candidate.ProfilePictureUrl = pictureUrl;
            }

            await this.candidatesRepository.AddAsync(candidate);
            await this.candidatesRepository.SaveChangesAsync();

            return candidate.Id;
        }

        public string GetCandidateIdByUsername(string username)
        {
            var candidateId = this.candidatesRepository
                  .All()
                  .Where(c => c.ApplicationUser.UserName == username)
                  .Select(c => c.Id)
                  .FirstOrDefault();

            return candidateId;
        }

        public T GetProfileDetails<T>(string candidateId)
        {
            T candidate = this.candidatesRepository
                 .All()
                 .Where(e => e.Id == candidateId)
                 .To<T>()
                 .FirstOrDefault();

            return candidate;
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

            if (candidate.ProfilePictureUrl != null)
            {
                CloudinaryService.DeleteFile(this.cloudinary, model.ApplicationUserId + PictureNameAddIn);
                candidate.ProfilePictureUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.ProfilePicture, model.ApplicationUserId + PictureNameAddIn);
            }

            this.candidatesRepository.Update(candidate);
            await this.candidatesRepository.SaveChangesAsync();

            return candidate.Id;
        }
    }
}
