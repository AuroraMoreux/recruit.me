namespace RecruitMe.Services.Data
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Services;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Candidates;

    public class CandidatesService : ICandidatesService
    {
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
                string pictureUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.ProfilePicture, model.ApplicationUserId + "_profilePicture");

                candidate.ProfilePictureUrl = pictureUrl;
            }

            await this.candidatesRepository.AddAsync(candidate);
            await this.candidatesRepository.SaveChangesAsync();

            return candidate.Id;
        }
    }
}
