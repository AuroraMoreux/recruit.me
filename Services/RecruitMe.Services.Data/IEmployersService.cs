﻿namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.Employers;

    public interface IEmployersService
    {
        Task<string> CreateProfileAsync(CreateEmployerProfileInputModel model);

        T GetProfileDetails<T>(string employerId);

        Task<string> UpdateProfileAsync(string employerId, UpdateEmployerProfileViewModel model);

        string GetEmployerIdByUsername(string username);

        int GetCount();

        int GetNewEmployersCount();

        IEnumerable<T> GetTopFiveEmployers<T>();

        string GetEmployerNameById(string id);
    }
}
