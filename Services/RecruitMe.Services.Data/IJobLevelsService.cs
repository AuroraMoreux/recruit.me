namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJobLevelsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
