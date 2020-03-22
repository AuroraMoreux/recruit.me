namespace RecruitMe.Services.Data
{
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.Employers;

    public interface IEmployersService
    {
        Task<string> CreateProfileAsync(CreateEmployerProfileInputModel model);

        Task<T> GetProfileDetailsAsync<T>(string employerId);
    }
}
