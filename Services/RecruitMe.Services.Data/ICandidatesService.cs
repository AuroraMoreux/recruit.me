namespace RecruitMe.Services.Data
{
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.Candidates;

    public interface ICandidatesService
    {
        Task<string> CreateProfileAsync(CreateCandidateProfileInputModel model);

        T GetProfileDetails<T>(string candidateId);

        Task<string> UpdateProfileAsync(string candidateId, UpdateCandidateProfileViewModel model);
    }
}
