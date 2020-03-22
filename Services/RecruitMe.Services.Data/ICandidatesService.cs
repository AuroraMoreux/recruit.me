namespace RecruitMe.Services.Data
{
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.Candidates;

    public interface ICandidatesService
    {
        Task<string> CreateProfileAsync(CreateCandidateProfileInputModel model);
    }
}
