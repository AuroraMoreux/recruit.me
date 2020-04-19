namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Administration.Languages;

    public class LanguagesService : ILanguagesService
    {
        private readonly IDeletableEntityRepository<Language> languagesRepository;

        public LanguagesService(IDeletableEntityRepository<Language> languagesRepository)
        {
            this.languagesRepository = languagesRepository;
        }

        public async Task<int> Create(CreateViewModel input)
        {
            Language language = AutoMapperConfig.MapperInstance.Map<Language>(input);

            if (language.IsDeleted)
            {
                language.DeletedOn = DateTime.UtcNow;
            }

            language.CreatedOn = DateTime.UtcNow;
            try
            {
                await this.languagesRepository.AddAsync(language);
                await this.languagesRepository.SaveChangesAsync();
                return language.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int id)
        {
            Language language = this.languagesRepository
               .All()
               .Where(l => l.Id == id)
               .FirstOrDefault();

            if (language == null)
            {
                return false;
            }

            try
            {
                this.languagesRepository.Delete(language);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            var languages = this.languagesRepository
                 .AllAsNoTracking()
                 .OrderBy(l => l.Name)
                 .To<T>()
                 .ToList();

            return languages;
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            return this.languagesRepository
                 .AllAsNoTrackingWithDeleted()
                 .OrderBy(l => l.Name)
                 .To<T>()
                 .ToList();
        }

        public T GetDetails<T>(int id)
        {
            return this.languagesRepository
                 .AllAsNoTrackingWithDeleted()
                 .Where(l => l.Id == id)
                 .To<T>()
                 .FirstOrDefault();
        }

        public async Task<int> Update(EditViewModel input)
        {
            Language language = this.languagesRepository
                 .AllWithDeleted()
                 .Where(c => c.Id == input.Id)
                 .FirstOrDefault();

            if (language == null)
            {
                return -1;
            }

            language.Name = input.Name;
            language.IsDeleted = input.IsDeleted;
            language.ModifiedOn = DateTime.UtcNow;
            if (language.IsDeleted)
            {
                language.DeletedOn = DateTime.UtcNow;
            }

            try
            {
                this.languagesRepository.Update(language);
                await this.languagesRepository.SaveChangesAsync();
                return language.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
