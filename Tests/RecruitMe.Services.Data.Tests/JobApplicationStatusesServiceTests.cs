namespace RecruitMe.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Data.Repositories;
    using RecruitMe.Services.Data.Tests.Common;
    using RecruitMe.Web.ViewModels.Administration.JobApplicationStatuses;
    using Xunit;

    public class JobApplicationStatusesServiceTests
    {
        [Fact]
        public async Task CreateSuccessfullyAddsNewStatus()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            var repository = new EfDeletableEntityRepository<JobApplicationStatus>(context);

            var service = new JobApplicationStatusesService(repository);

            var model = new CreateViewModel
            {
                Name = "New Status",
                IsDeleted = true,
            };

            int id = await service.CreateAsync(model);

            Assert.NotEqual(-1, id);
            Assert.Equal(1, context.ApplicationStatuses.IgnoreQueryFilters().Count());
        }

        [Fact]
        public async Task DeleteMarksRecordAsDeleted()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.ApplicationStatuses.AddAsync(new JobApplicationStatus { Id = 1, Name = "Status", IsDeleted = false });
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobApplicationStatus>(context);

            var service = new JobApplicationStatusesService(repository);

            var result = await service.DeleteAsync(1);

            var dbRecord = await context.ApplicationStatuses.FindAsync(1);
            Assert.True(result);
            Assert.True(dbRecord.IsDeleted);
            Assert.NotNull(dbRecord.DeletedOn);
            Assert.Equal(1, context.ApplicationStatuses.IgnoreQueryFilters().Count());
        }

        [Fact]
        public async Task DeleteFailsWhenIdNotInDatabase()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            var repository = new EfDeletableEntityRepository<JobApplicationStatus>(context);

            var service = new JobApplicationStatusesService(repository);
            var result = await service.DeleteAsync(100);

            Assert.False(result);
        }

        [Fact]
        public async Task GetDetailsReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.ApplicationStatuses.AddRangeAsync(this.SeedData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobApplicationStatus>(context);

            var service = new JobApplicationStatusesService(repository);

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
            await context.ApplicationStatuses.AddRangeAsync(this.SeedData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobApplicationStatus>(context);

            var service = new JobApplicationStatusesService(repository);
            var result = service.GetAll<EditViewModel>();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllWithDeletedReturnsCorrectNumberOfRecords()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.ApplicationStatuses.AddRangeAsync(this.SeedData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobApplicationStatus>(context);

            var service = new JobApplicationStatusesService(repository);
            var result = service.GetAllWithDeleted<EditViewModel>();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task UpdateAsyncCorrectlyUpdatesInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.ApplicationStatuses.AddRangeAsync(this.SeedData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobApplicationStatus>(context);

            var service = new JobApplicationStatusesService(repository);

            var model = new EditViewModel
            {
                Name = "NewName",
                IsDeleted = true,
            };

            var result = await service.UpdateAsync(1, model);
            Assert.NotEqual(-1, result);

            var dbRecord = await context.ApplicationStatuses.FindAsync(1);

            Assert.NotEqual("First", dbRecord.Name);
            Assert.NotNull(dbRecord.DeletedOn);
            Assert.True(dbRecord.IsDeleted);
        }

        [Fact]
        public async Task UpdateAsyncFailsWhenIdNotInDb()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            var repository = new EfDeletableEntityRepository<JobApplicationStatus>(context);

            var service = new JobApplicationStatusesService(repository);

            var model = new EditViewModel
            {
                Name = "NewName",
                IsDeleted = true,
            };

            var result = await service.UpdateAsync(100, model);
            Assert.Equal(-1, result);
        }

        private IEnumerable<JobApplicationStatus> SeedData()
        {
            return new List<JobApplicationStatus>
            {
                new JobApplicationStatus
                {
                    Id = 1,
                    Name = "First",
                    IsDeleted = false,
                },
                new JobApplicationStatus
                {
                    Id = 2,
                    Name = "Second",
                    IsDeleted = false,
                },
                new JobApplicationStatus
                {
                    Id = 3,
                    Name = "Third",
                    IsDeleted = true,
                },
            };
        }
    }
}
