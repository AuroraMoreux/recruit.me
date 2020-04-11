namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;

    public interface ILanguagesService
    {
        IEnumerable<T> GetAll<T>();
    }
}
