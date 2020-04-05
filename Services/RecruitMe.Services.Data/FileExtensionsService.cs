namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;

    public class FileExtensionsService : IFileExtensionsService
    {
        private readonly IDeletableEntityRepository<FileExtension> extensionsRepository;

        public FileExtensionsService(IDeletableEntityRepository<FileExtension> extensionsRepository)
        {
            this.extensionsRepository = extensionsRepository;
        }

        public IEnumerable<string> GetAll()
        {
            var names = this.extensionsRepository
                .All()
                .OrderBy(e => e.Name)
                .Select(e => e.Name)
                .ToList();

            return names;
        }
    }
}
