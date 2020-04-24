namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.JobOffers;

    public interface IJobOffersService
    {
        Task<string> Add(PostViewModel model, string employerId);

        Task<IEnumerable<T>> GetAllValidFilteredOffers<T>(FilterModel filters);

        T GetDetails<T>(string jobOfferId);

        bool IsOfferPostedByEmployer(string jobOfferId, string employerId);

        bool IsPositionDuplicate(string employerId, string jobOfferPosition);

        Task<string> Update(EditViewModel model, string employerId);

        Task<bool> DeleteAsync(string jobOfferId);

        int GetCount();

        int GetNewOffersCount();

        IEnumerable<T> GetEmployerJobOffers<T>(string employerId);

        string GetOfferPositionById(string jobOfferId);

        IEnumerable<T> GetLastTenOffers<T>();
    }
}
