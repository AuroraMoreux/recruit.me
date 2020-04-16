﻿namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.JobOffers;

    public interface IJobOffersService
    {
        Task<string> AddOffer(PostViewModel model, string employerId);

        IEnumerable<T> GetAllValidOffers<T>();

        T GetOfferDetails<T>(string jobOfferId);

        bool IsJobOfferPostedByEmployer(string jobOfferId, string employerId);

        bool IsOfferTitleDuplicate(string employerId, string jobOfferTitle);

        Task<string> UpdateOffer(EditViewModel model, string employerId);

        Task DeleteOffer(string jobOfferId);
    }
}
