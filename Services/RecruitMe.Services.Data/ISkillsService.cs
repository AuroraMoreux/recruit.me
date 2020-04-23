namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RecruitMe.Web.ViewModels.Administration.Skills;

    public interface ISkillsService
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllWithDeleted<T>();

        Task<int> CreateAsync(CreateViewModel input);

        Task<int> UpdateAsync(int id,EditViewModel input);

        Task<bool> DeleteAsync(int id);

        T GetDetails<T>(int id);
    }
}
