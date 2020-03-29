namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IJobSectorsService
    {
        IEnumerable<T> GetAllAsync<T>();
    }
}
