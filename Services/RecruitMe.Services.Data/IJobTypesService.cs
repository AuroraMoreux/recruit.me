namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;

    public interface IJobTypesService
    {
        IEnumerable<T> GetAll<T>();
    }
}
