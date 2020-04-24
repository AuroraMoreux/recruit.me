namespace RecruitMe.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Data.Repositories;
    using RecruitMe.Services.Data.Tests.Common;
    using RecruitMe.Web.ViewModels.Administration.FileExtensions;
    using Xunit;

    public class FileExtensionsServiceTests
    {
        [Fact]
        public async Task CreateSuccessfullyAddsNewExtension()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            var repository = new EfDeletableEntityRepository<FileExtension>(context);

            var service = new FileExtensionsService(repository);

            var model = new CreateViewModel
            {
                Name = "New Extension",
                IsDeleted = true,
            };

            var id = await service.CreateAsync(model);

            Assert.NotEqual(-1, id);
            Assert.Equal(1, context.FileExtensions.IgnoreQueryFilters().Count());
        }

        [Fact]
        public async Task DeleteMarksRecordAsDeleted()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.FileExtensions.AddAsync(new FileExtension { Id = 1, Name = "Extension", FileType = "File", IsDeleted = false });
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<FileExtension>(context);

            var service = new FileExtensionsService(repository);

            var result = await service.DeleteAsync(1);

            var dbRecord = await context.FileExtensions.FindAsync(1);
            Assert.True(result);
            Assert.True(dbRecord.IsDeleted);
            Assert.NotNull(dbRecord.DeletedOn);
            Assert.Equal(1, context.FileExtensions.IgnoreQueryFilters().Count());
        }

        [Fact]
        public async Task DeleteFailsWhenIdNotInDatabase()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            var repository = new EfDeletableEntityRepository<FileExtension>(context);

            var service = new FileExtensionsService(repository);
            var result = await service.DeleteAsync(100);

            Assert.False(result);
        }

        [Fact]
        public async Task GetDetailsReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.FileExtensions.AddRangeAsync(this.SeedData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<FileExtension>(context);

            var service = new FileExtensionsService(repository);

            var result = service.GetDetails<EditViewModel>(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("First", result.Name);
            Assert.False(result.IsDeleted);
        }

        [Fact]
        public async Task GetAllReturnsCorrectNumberOfRecords()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.FileExtensions.AddRangeAsync(this.SeedData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<FileExtension>(context);

            var service = new FileExtensionsService(repository);
            var result = service.GetAll();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllWithDeletedReturnsCorrectNumberOfRecords()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.FileExtensions.AddRangeAsync(this.SeedData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<FileExtension>(context);

            var service = new FileExtensionsService(repository);
            var result = service.GetAllWithDeleted<EditViewModel>();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task UpdateAsyncCorrectlyUpdatesInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.FileExtensions.AddRangeAsync(this.SeedData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<FileExtension>(context);

            var service = new FileExtensionsService(repository);

            var model = new EditViewModel
            {
                Name = "NewName",
                FileType = "File",
                IsDeleted = true,
            };

            var result = await service.UpdateAsync(1, model);
            Assert.NotEqual(-1, result);

            var dbRecord = await context.FileExtensions.FindAsync(1);

            Assert.NotEqual("First", dbRecord.Name);
            Assert.NotNull(dbRecord.DeletedOn);
            Assert.True(dbRecord.IsDeleted);
        }

        [Fact]
        public async Task UpdateAsyncFailsWhenIdNotInDb()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            var repository = new EfDeletableEntityRepository<FileExtension>(context);

            var service = new FileExtensionsService(repository);

            var model = new EditViewModel
            {
                Name = "NewName",
                IsDeleted = true,
            };

            var result = await service.UpdateAsync(100, model);
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task GetImageExtensionsReturnsCorrectNumberOfRecords()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.FileExtensions.AddRangeAsync(this.SeedData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<FileExtension>(context);

            var service = new FileExtensionsService(repository);

            var result = service.GetImageExtensions();

            Assert.Equal(2, result.Count());
        }

        private IEnumerable<FileExtension> SeedData()
        {
            return new List<FileExtension>
            {
                new FileExtension
                {
                    Id = 1,
                    Name = "First",
                    FileType = "Image",
                    IsDeleted = false,
                },
                new FileExtension
                {
                    Id = 2,
                    Name = "Second",
                    FileType = "Image",
                    IsDeleted = false,
                },
                new FileExtension
                {
                    Id = 3,
                    Name = "Third",
                    FileType = "File",
                    IsDeleted = true,
                },
            };
        }
    }
}
