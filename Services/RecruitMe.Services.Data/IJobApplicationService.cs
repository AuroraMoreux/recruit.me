namespace RecruitMe.Services.Data
{
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.JobApplications;

    public interface IJobApplicationService
    {
        Task<string> Apply(ApplyViewModel input, string jobOfferBaseUrl);

        bool IsUserRelatedToJobApplication(string jobApplicationId, string userId);

        bool HasCandidateAppliedForOffer(string candidateId, string jobOfferId);

        string GetJobOfferIdForApplication(string jobApplicationId);

        T GetJobApplicationDetails<T>(string jobApplicationId);

        int GetJobApplicationStatusId(string jobApplicationId);

        Task<int> ChangeJobApplicationStatus(string jobApplicationId, int statusId);

        int GetCount();

        int GetNewApplicationsCount();
    }
}
