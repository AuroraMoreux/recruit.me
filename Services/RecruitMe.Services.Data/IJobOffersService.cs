namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.JobOffers;

    public interface IJobOffersService
    {
        Task<string> Add(PostViewModel model, string employerId);

        IEnumerable<T> GetAllValidFilteredOffers<T>(FilterModel filters);

        T GetDetails<T>(string jobOfferId);

        bool IsOfferPostedByEmployer(string jobOfferId, string employerId);

        bool IsTitleDuplicate(string employerId, string jobOfferTitle);

        Task<string> Update(EditViewModel model, string employerId);

        Task Delete(string jobOfferId);

        int GetCount();
    }
}
