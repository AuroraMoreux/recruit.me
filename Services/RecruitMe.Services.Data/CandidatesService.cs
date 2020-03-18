namespace RecruitMe.Services.Data
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Services;
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

        public async Task<string> CreateProfile(CreateCandidateProfileInputModel model)
        {
            // TODO: Can this be done with Automapper?
            Candidate candidate = new Candidate
            {
                ApplicationUserId = model.ApplicationUserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ContactAddress = model.ContactAddress,
                PhoneNumber = model.PhoneNumber,
                Education = model.Education,
            };

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
