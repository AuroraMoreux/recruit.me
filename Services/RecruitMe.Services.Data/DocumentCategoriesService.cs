namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Administration.DocumentCategories;

    public class DocumentCategoriesService : IDocumentCategoriesService
    {
        private readonly IDeletableEntityRepository<DocumentCategory> categoriesRepository;

        public DocumentCategoriesService(IDeletableEntityRepository<DocumentCategory> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<int> Create(CreateViewModel input)
        {
            DocumentCategory category = AutoMapperConfig.MapperInstance.Map<DocumentCategory>(input);

            if (category.IsDeleted)
            {
                category.DeletedOn = DateTime.UtcNow;
            }

            category.CreatedOn = DateTime.UtcNow;
            try
            {
                await this.categoriesRepository.AddAsync(category);
                await this.categoriesRepository.SaveChangesAsync();
                return category.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int id)
        {
            DocumentCategory category = this.categoriesRepository
                 .All()
                 .Where(c => c.Id == id)
                 .FirstOrDefault();

            if (category == null)
            {
                return false;
            }

            try
            {
                this.categoriesRepository.Delete(category);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public T GetDetails<T>(int id)
        {
            return this.categoriesRepository
                 .AllAsNoTrackingWithDeleted()
                 .Where(c => c.Id == id)
                 .To<T>()
                 .FirstOrDefault();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.categoriesRepository
                 .AllAsNoTracking()
                 .OrderBy(c => c.Name)
                 .To<T>()
                 .ToList();
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            return this.categoriesRepository
                 .AllAsNoTrackingWithDeleted()
                 .OrderBy(c => c.Name)
                 .To<T>()
                 .ToList();
        }

        public async Task<int> Update(EditViewModel input)
        {
            DocumentCategory category = this.categoriesRepository
                 .AllWithDeleted()
                 .Where(c => c.Id == input.Id)
                 .FirstOrDefault();

            if (category == null)
            {
                return -1;
            }

            category.Name = input.Name;
            category.IsDeleted = input.IsDeleted;
            category.ModifiedOn = DateTime.UtcNow;
            if (category.IsDeleted)
            {
                category.DeletedOn = DateTime.UtcNow;
            }

            try
            {
                this.categoriesRepository.Update(category);
                await this.categoriesRepository.SaveChangesAsync();
                return category.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
