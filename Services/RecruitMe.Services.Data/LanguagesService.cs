namespace RecruitMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class LanguagesService : ILanguagesService
    {
        private readonly IDeletableEntityRepository<Language> languagesRepository;

        public LanguagesService(IDeletableEntityRepository<Language> languagesRepository)
        {
            this.languagesRepository = languagesRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var languages = this.languagesRepository
                 .All()
                 .OrderBy(l => l.Name)
                 .To<T>()
                 .ToList();

            return languages;
        }
    }
}
