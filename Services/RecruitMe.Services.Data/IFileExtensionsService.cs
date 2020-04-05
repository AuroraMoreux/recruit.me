namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IFileExtensionsService
    {
        IEnumerable<string> GetAll();
    }
}
