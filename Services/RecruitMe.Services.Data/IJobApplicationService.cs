namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.JobApplications;

    public interface IJobApplicationService
    {
        Task<string> Apply(ApplyViewModel input, string jobApplicationBaseUrl);

        bool IsUserRelatedToJobApplication(string jobApplicationId, string userId);

        bool HasCandidateAppliedForOffer(string candidateId, string jobOfferId);

        string GetJobOfferIdForApplication(string jobApplicationId);

        T GetJobApplicationDetails<T>(string jobApplicationId);

        int GetJobApplicationStatusId(string jobApplicationId);

        Task<int> ChangeJobApplicationStatus(string jobApplicationId, int statusId, string jobApplicationBaseUrl);

        int GetCount();

        int GetNewApplicationsCount();

        IEnumerable<T> GetCandidateApplications<T>(string candidateId);

        IEnumerable<T> GetApplicationsForOffer<T>(string offerId);
    }
}
