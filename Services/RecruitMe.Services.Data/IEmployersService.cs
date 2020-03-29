namespace RecruitMe.Services.Data
{
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.Employers;

    public interface IEmployersService
    {
        Task<string> CreateProfileAsync(CreateEmployerProfileInputModel model);

        T GetProfileDetails<T>(string employerId);

        Task<string> UpdateProfileAsync(string employerId, UpdateEmployerProfileViewModel model);
    }
}
