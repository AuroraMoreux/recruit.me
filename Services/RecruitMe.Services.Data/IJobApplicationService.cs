namespace RecruitMe.Services.Data
{
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.JobApplications;

    public interface IJobApplicationService
    {
        Task<string> Apply(ApplyViewModel input, string jobOfferBaseUrl);

        bool HasCandidateAppliedForOffer(string candidateId, string jobOfferId);

        string GetJobOfferIdForApplication(string jobApplicationId);

        T GetJobApplicationDetails<T>(string jobApplicationId);
    }
}
