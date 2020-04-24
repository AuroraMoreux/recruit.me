namespace RecruitMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Data.Repositories;
    using RecruitMe.Services.Data.Tests.Common;
    using RecruitMe.Web.ViewModels.JobOffers;
    using RecruitMe.Web.ViewModels.Shared;
    using Xunit;

    public class JobOffersServiceTests
    {
        [Fact]
        public async Task IsOfferPostedByEmployerReturnsCorrectDataWhenTrue()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);
            var result = service.IsOfferPostedByEmployer("1", "111");

            Assert.True(result);
        }

        [Fact]
        public async Task IsOfferPostedByEmployerReturnsCorrectDataWhenFalse()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);
            var result = service.IsOfferPostedByEmployer("1", "222");

            Assert.False(result);
        }

        [Fact]
        public async Task GetDetailsReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);
            var result = service.GetDetails<JobOfferDetailsViewModel>("1");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task IsPositionDuplicateReturnsCorrectInformationWhenTrue()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);
            var result = service.IsPositionDuplicate("222", "CFO");

            Assert.True(result);
        }

        [Fact]
        public async Task IsPositionDuplicateReturnsCorrectInformationWhenFalse()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);
            var result = service.IsPositionDuplicate("222", "Not CFO");

            Assert.False(result);
        }

        [Fact]
        public async Task GetCountReturnsCorrectCount()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);
            var result = service.GetCount();

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task GetNewOffersCountReturnsCorrectCount()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);
            var result = service.GetNewOffersCount();

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task DeleteAsyncCorrectlyMarksRecordAsDeleted()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);

            var result = await service.DeleteAsync("1");

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsyncReturnsFalseWhenRecordNotInDatabase()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);

            var result = await service.DeleteAsync("111");

            Assert.False(result);
        }

        [Fact]
        public async Task GetEmployerJobOffersReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);

            var result = service.GetEmployerJobOffers<EmployerJobOffersViewModel>("222");

            Assert.Equal(9, result.Count());
        }

        [Fact]
        public async Task GetOfferPositionByIdReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);

            var result = service.GetOfferPositionById("1");

            Assert.Equal("CEO", result);
        }

        [Fact]
        public async Task GetLastTenOffersReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);

            var result = service.GetLastTenOffers<IndexJobOffersModel>();

            Assert.Equal(10, result.Count());
        }

        [Fact]
        public async Task AddAsyncSavesRecordCorrectly()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var languagesRepository = new EfDeletableEntityRepository<JobOfferLanguage>(context);
            var skillsRepository = new EfDeletableEntityRepository<JobOfferSkill>(context);
            var service = this.GetMockedService(repository, languagesRepository, skillsRepository);

            var model = new PostViewModel
            {
                LanguagesIds = new List<int> { 1, 2, 3 },
                SkillsIds = new List<int> { 4, 5, 6 },
                JobTypesIds = new List<int> { 7, 8, 9 },

            };
        }
        private JobOffersService GetMockedService(
            IDeletableEntityRepository<JobOffer> offersRepository,
            IDeletableEntityRepository<JobOfferLanguage> languagesRepository = null,
            IDeletableEntityRepository<JobOfferSkill> skillsRepository = null,
            IDeletableEntityRepository<JobOfferJobType> jobTypesRepository = null)
        {
            var mocklanguagesRepository = languagesRepository ?? new Mock<IDeletableEntityRepository<JobOfferLanguage>>().Object;
            var mockSkillsRepository = skillsRepository ?? new Mock<IDeletableEntityRepository<JobOfferSkill>>().Object;
            var mockJobTypesRepository = jobTypesRepository ?? new Mock<IDeletableEntityRepository<JobOfferJobType>>().Object;

            return new JobOffersService(offersRepository, languagesRepository, skillsRepository, jobTypesRepository);
        }

        private IEnumerable<JobOffer> SeedTestData()
        {
            return new List<JobOffer>
            {
                new JobOffer
                {
                    Id = "1",
                    Employer = new Employer
                    {
                        Name = "Recruit Me",
                        ApplicationUser = new ApplicationUser { UserName = "UserName" },
                        Id = "111",
                    },
                    Position = "CEO",
                    CreatedOn = DateTime.UtcNow.AddDays(-3),
                    IsDeleted = false,
                    JobLevel = new JobLevel { Name = "First", Id = 1 },
                    ValidFrom = DateTime.UtcNow.AddHours(-3),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                    JobSector = new JobSector { Name = "IT", Id = 1 },
                },
                new JobOffer
                {
                    Id = "2",
                    Employer = new Employer
                    {
                        Name = "Microsoft",
                        ApplicationUser = new ApplicationUser { UserName = "OtherUserName" },
                        Id = "222",
                    },
                    Position = "CFO",
                    CreatedOn = DateTime.UtcNow.AddHours(-25),
                    IsDeleted = false,
                    JobLevel = new JobLevel { Name = "Second", Id = 2 },
                    ValidFrom = DateTime.UtcNow.AddHours(-3),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                    JobSector = new JobSector { Name = "Networking", Id = 2 },
                },
                new JobOffer
                {
                    Id = "3",
                    EmployerId = "222",
                    Position = "CFO - Sofia",
                    CreatedOn = DateTime.UtcNow.AddHours(-3),
                    IsDeleted = false,
                    JobLevelId = 1,
                    ValidFrom = DateTime.UtcNow.AddHours(13),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                    JobSectorId = 2,
                },
                new JobOffer
                {
                    Id = "4",
                    EmployerId = "222",
                    Position = "CFO - London",
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    IsDeleted = false,
                    JobLevelId = 2,
                    ValidFrom = DateTime.UtcNow.AddHours(4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                    JobSectorId = 2,
                },
                new JobOffer
                {
                    Id = "5",
                    Position = "CTO",
                    EmployerId = "222",
                    City = "New York",
                    JobLevelId = 2,
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    ValidFrom = DateTime.UtcNow.AddHours(-4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                },
                new JobOffer
                {
                    Id = "6",
                    Position = "IT Support",
                    EmployerId = "222",
                    City = "New York",
                    JobLevelId = 2,
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    ValidFrom = DateTime.UtcNow.AddHours(-4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                },
                new JobOffer
                {
                    Id = "7",
                    Position = "QA",
                    EmployerId = "222",
                    City = "New York",
                    JobLevelId = 2,
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    ValidFrom = DateTime.UtcNow.AddHours(-4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                },
                new JobOffer
                {
                    Id = "8",
                    Position = "Senior Developer",
                    EmployerId = "222",
                    City = "New York",
                    JobLevelId = 2,
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    ValidFrom = DateTime.UtcNow.AddHours(-4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                },
                new JobOffer
                {
                    Id = "9",
                    Position = "Junior Developer",
                    EmployerId = "222",
                    City = "New York",
                    JobLevelId = 2,
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    ValidFrom = DateTime.UtcNow.AddHours(-4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                },
                new JobOffer
                {
                    Id = "10",
                    Position = "Intern",
                    EmployerId = "222",
                    City = "New York",
                    JobLevelId = 2,
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    ValidFrom = DateTime.UtcNow.AddHours(-4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                },
                new JobOffer
                {
                    Id = "11",
                    Position = "Cashier",
                    EmployerId = "222",
                    City = "New York",
                    JobLevelId = 2,
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    ValidFrom = DateTime.UtcNow.AddHours(-4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                },
                new JobOffer
                {
                    Id = "12",
                    Position = "HR",
                    EmployerId = "222",
                    City = "New York",
                    JobLevelId = 2,
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    ValidFrom = DateTime.UtcNow.AddHours(-4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                },
            };
        }
    }
}
