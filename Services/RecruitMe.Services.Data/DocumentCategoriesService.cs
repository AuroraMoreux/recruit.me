namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class DocumentCategoriesService : IDocumentCategoriesService
    {
        private readonly IDeletableEntityRepository<DocumentCategory> categoriesRepository;

        public DocumentCategoriesService(IDeletableEntityRepository<DocumentCategory> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var categories = this.categoriesRepository
                 .All()
                 .OrderBy(c => c.Name)
                 .To<T>()
                 .ToList();

            return categories;
        }
    }
}
