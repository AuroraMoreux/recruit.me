namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;

    public interface ISkillsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
