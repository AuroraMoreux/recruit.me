namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Administration.FileExtensions;

    public class FileExtensionsService : IFileExtensionsService
    {
        private readonly IDeletableEntityRepository<FileExtension> extensionsRepository;

        public FileExtensionsService(IDeletableEntityRepository<FileExtension> extensionsRepository)
        {
            this.extensionsRepository = extensionsRepository;
        }

        public async Task<int> CreateAsync(CreateViewModel input)
        {
            var extension = AutoMapperConfig.MapperInstance.Map<FileExtension>(input);

            if (extension.IsDeleted)
            {
                extension.DeletedOn = DateTime.UtcNow;
            }

            extension.CreatedOn = DateTime.UtcNow;
            try
            {
                await this.extensionsRepository.AddAsync(extension);
                await this.extensionsRepository.SaveChangesAsync();
                return extension.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var extension = this.extensionsRepository
                .All()
                .Where(e => e.Id == id)
                .FirstOrDefault();

            if (extension == null)
            {
                return false;
            }

            try
            {
                this.extensionsRepository.Delete(extension);
                await this.extensionsRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<string> GetAll()
        {
            return this.extensionsRepository
                .AllAsNoTracking()
                .OrderBy(e => e.Name)
                .Select(e => e.Name)
                .ToList();
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            return this.extensionsRepository
                .AllAsNoTrackingWithDeleted()
                .OrderBy(e => e.Name)
                .To<T>()
                .ToList();
        }

        public T GetDetails<T>(int id)
        {
            return this.extensionsRepository
                  .AllAsNoTrackingWithDeleted()
                  .Where(e => e.Id == id)
                  .To<T>()
                  .FirstOrDefault();
        }

        public IEnumerable<string> GetImageExtensions()
        {
            return this.extensionsRepository
                .AllAsNoTracking()
                .Where(e => e.FileType.ToLower() == "image")
                .OrderBy(e => e.Name)
                .Select(e => e.Name)
                .ToList();
        }

        public async Task<int> UpdateAsync(int id, EditViewModel input)
        {
            var extension = this.extensionsRepository
                 .AllWithDeleted()
                 .Where(e => e.Id == id)
                 .FirstOrDefault();

            if (extension == null)
            {
                return -1;
            }

            extension.Name = input.Name;
            extension.IsDeleted = input.IsDeleted;
            extension.FileType = input.FileType;
            extension.ModifiedOn = DateTime.UtcNow;
            if (extension.IsDeleted)
            {
                extension.DeletedOn = DateTime.UtcNow;
            }

            try
            {
                this.extensionsRepository.Update(extension);
                await this.extensionsRepository.SaveChangesAsync();
                return extension.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
