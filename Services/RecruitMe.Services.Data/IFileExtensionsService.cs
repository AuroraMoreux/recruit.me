namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.Administration.FileExtensions;

    public interface IFileExtensionsService
    {
        IEnumerable<string> GetAll();

        IEnumerable<string> GetImageExtensions();

        IEnumerable<T> GetAllWithDeleted<T>();

        Task<int> Create(CreateViewModel input);

        Task<int> Update(EditViewModel input);

        bool Delete(int id);

        T GetDetails<T>(int id);
    }
}
