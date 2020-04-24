namespace RecruitMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
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
            var typesRepository = new EfDeletableEntityRepository<JobOfferJobType>(context);
            var service = this.GetMockedService(repository, languagesRepository, skillsRepository, typesRepository);

            var model = new PostViewModel
            {
                LanguagesIds = new List<int> { 1, 2, 3 },
                SkillsIds = new List<int> { 4, 5, },
                JobTypesIds = new List<int> { 6 },
                JobLevelId = 1,
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddDays(30),
                JobSectorId = 1,
                City = "Sofia",
                Position = "Tech Lead",
            };

            var result = await service.AddAsync(model, "1");

            Assert.NotNull(result);

            var offerLanguagesCount = await context.JobOffers.Include(jo => jo.Languages).Where(jo => jo.Id == result).Select(jo => jo.Languages.Count).FirstOrDefaultAsync();
            var offerSkillsCount = await context.JobOffers.Include(jo => jo.Skills).Where(jo => jo.Id == result).Select(jo => jo.Skills.Count).FirstOrDefaultAsync();
            var offerTypesCount = await context.JobOffers.Include(jo => jo.JobTypes).Where(jo => jo.Id == result).Select(jo => jo.JobTypes.Count).FirstOrDefaultAsync();

            Assert.Equal(1, context.JobOffers.Count());
            Assert.Equal(3, offerLanguagesCount);
            Assert.Equal(2, offerSkillsCount);
            Assert.Equal(1, offerTypesCount);
        }

        [Fact]
        public async Task UpdateAsyncCorrectlyUpdatesRecord()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var languagesRepository = new EfDeletableEntityRepository<JobOfferLanguage>(context);
            var skillsRepository = new EfDeletableEntityRepository<JobOfferSkill>(context);
            var typesRepository = new EfDeletableEntityRepository<JobOfferJobType>(context);
            var service = this.GetMockedService(repository, languagesRepository, skillsRepository, typesRepository);

            var offer = new PostViewModel
            {
                LanguagesIds = new List<int> { 1, 2, 3 },
                SkillsIds = new List<int> { 4, 5, },
                JobTypesIds = new List<int> { 6 },
                JobLevelId = 1,
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddDays(30),
                JobSectorId = 1,
                City = "Sofia",
                Position = "Tech Lead",
            };
            var id = await service.AddAsync(offer, "1");

            var model = new EditViewModel
            {
                JobOfferDetails = new EditJobOfferDetailsModel
                {
                    Id = id,
                    LanguagesIds = new List<int> { 4, 5, },
                    SkillsIds = new List<int> { 6, 7 },
                    JobTypesIds = new List<int> { 8, 9, 1 },
                    JobLevelId = 2,
                    JobSectorId = 3,
                    City = "New York",
                    Position = "CEO",
                },
            };

            var result = await service.UpdateAsync(model, "1");

            Assert.NotNull(result);

            var offerLanguagesCount = await context.JobOffers.Include(jo => jo.Languages).Where(jo => jo.Id == result).Select(jo => jo.Languages.Count).FirstOrDefaultAsync();
            var offerSkillsCount = await context.JobOffers.Include(jo => jo.Skills).Where(jo => jo.Id == result).Select(jo => jo.Skills.Count).FirstOrDefaultAsync();
            var offerTypesCount = await context.JobOffers.Include(jo => jo.JobTypes).Where(jo => jo.Id == result).Select(jo => jo.JobTypes.Count).FirstOrDefaultAsync();

            Assert.Equal(1, context.JobOffers.Count());
            Assert.Equal(2, offerLanguagesCount);
            Assert.Equal(2, offerSkillsCount);
            Assert.Equal(3, offerTypesCount);
        }

        [Fact]
        public async Task UpdateAsyncReturnsNullWhenRecordNotInDatabase()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);

            var model = new EditViewModel
            {
                JobOfferDetails = new EditJobOfferDetailsModel
                {
                    Id = "4444",
                    LanguagesIds = new List<int> { 4, 5, },
                    SkillsIds = new List<int> { 6, 7 },
                    JobTypesIds = new List<int> { 8, 9, 1 },
                    JobLevelId = 2,
                    JobSectorId = 3,
                    City = "New York",
                    Position = "CEO",
                },
            };

            var result = await service.UpdateAsync(model, "1");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllValidFilteredOffersAsyncReturnsAllOffersWhenFilterModelIsEmpty()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);

            var filterModel = new FilterModel();

            var result = await service.GetAllValidFilteredOffersAsync<IndexJobOffersModel>(filterModel);

            Assert.Equal(10, result.Count());
        }

        [Fact]
        public async Task GetAllValidFilteredOffersAsyncReturnsCorrectDetailsWhenSalaryFilterIsActive()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);

            var filterModel = new FilterModel
            {
                SalaryFrom = 990,
                SalaryTo = 2000,
            };

            var result = await service.GetAllValidFilteredOffersAsync<IndexJobOffersModel>(filterModel);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllValidFilteredOffersAsyncReturnsCorrectDetailsWhenJobLevelFilterIsActive()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);

            var filterModel = new FilterModel
            {
                LevelsIds = new List<int> { 2 },
            };

            var result = await service.GetAllValidFilteredOffersAsync<IndexJobOffersModel>(filterModel);

            Assert.Equal(9, result.Count());
        }

        [Fact]
        public async Task GetAllValidFilteredOffersAsyncReturnsCorrectDetailsWhenJobSectorFilterIsActive()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var service = this.GetMockedService(repository);

            var filterModel = new FilterModel
            {
                SectorsIds = new List<int> { 2 },
            };

            var result = await service.GetAllValidFilteredOffersAsync<IndexJobOffersModel>(filterModel);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllValidFilteredOffersAsyncReturnsCorrectDetailsWhenJobTypesFilterIsActive()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var jobTypesRepository = new EfDeletableEntityRepository<JobOfferJobType>(context);
            var service = this.GetMockedService(repository, null, null, jobTypesRepository);

            var filterModel = new FilterModel
            {
                TypesIds = new List<int> { 1 },
            };

            var result = await service.GetAllValidFilteredOffersAsync<IndexJobOffersModel>(filterModel);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllValidFilteredOffersAsyncReturnsCorrectDetailsWhenJobLanguagesFilterIsActive()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var jobLanguagesRepository = new EfDeletableEntityRepository<JobOfferLanguage>(context);
            var service = this.GetMockedService(repository, jobLanguagesRepository, null, null);

            var filterModel = new FilterModel
            {
                LanguagesIds = new List<int> { 1 },
            };

            var result = await service.GetAllValidFilteredOffersAsync<IndexJobOffersModel>(filterModel);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllValidFilteredOffersAsyncReturnsCorrectDetailsWhenJobSkillsFilterIsActive()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobOffers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<JobOffer>(context);
            var jobSkillsRepository = new EfDeletableEntityRepository<JobOfferSkill>(context);
            var service = this.GetMockedService(repository, null, jobSkillsRepository, null);

            var filterModel = new FilterModel
            {
                SkillsIds = new List<int> { 1 },
            };

            var result = await service.GetAllValidFilteredOffersAsync<IndexJobOffersModel>(filterModel);

            Assert.Equal(2, result.Count());
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
                    City = "Chicago",
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
                    City = "Chicago",
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
                    Skills = new List<JobOfferSkill> { new JobOfferSkill { SkillId = 1 } },
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
                    Skills = new List<JobOfferSkill> { new JobOfferSkill { SkillId = 1 } },
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
                    Languages = new List<JobOfferLanguage> { new JobOfferLanguage { LanguageId = 1, } },
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
                    Salary = 1000,
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    ValidFrom = DateTime.UtcNow.AddHours(-4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                    JobTypes = new List<JobOfferJobType> { new JobOfferJobType { JobTypeId = 1 } },
                },
                new JobOffer
                {
                    Id = "12",
                    Position = "HR",
                    EmployerId = "222",
                    City = "New York",
                    JobLevelId = 2,
                    Salary = 1000,
                    CreatedOn = DateTime.UtcNow.AddHours(-18),
                    ValidFrom = DateTime.UtcNow.AddHours(-4),
                    ValidUntil = DateTime.UtcNow.AddDays(3),
                    JobTypes = new List<JobOfferJobType> { new JobOfferJobType { JobTypeId = 1 } },
                },
            };
        }
    }
}
