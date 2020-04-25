namespace RecruitMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Moq;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Data.Repositories;
    using RecruitMe.Services.Data.Tests.Common;
    using RecruitMe.Services.Messaging;
    using RecruitMe.Web.ViewModels.JobApplications;
    using Xunit;

    public class JobApplicationsServiceTests
    {
        [Fact]
        public async Task GetApplicationsForOfferReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);

            var service = this.GetMockedService(repository);

            var result = service.GetApplicationsForOffer<JobOfferJobApplicationsViewModel>("111");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCandidateApplicationsReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);

            var service = this.GetMockedService(repository);

            var result = service.GetCandidateApplications<CandidateJobApplicationsViewModel>("Second");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCountReturnsCorrectCount()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);

            var service = this.GetMockedService(repository);

            var result = service.GetCount();

            Assert.Equal(4, result);
        }

        [Fact]
        public async Task GetJobApplicationStatusIdReturnsCorrectId()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var service = this.GetMockedService(repository);

            var result = service.GetJobApplicationStatusId("4");

            Assert.Equal(6, result);
        }

        [Fact]
        public async Task GetJobOfferIdForApplicationReturnsCorrectId()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var service = this.GetMockedService(repository);

            var result = service.GetJobOfferIdForApplication("4");

            Assert.Equal("112", result);
        }

        [Fact]
        public async Task GetNewApplicationsCountReturnsCorrectCount()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var service = this.GetMockedService(repository);
            var result = service.GetNewApplicationsCount();

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task HasCandidateAppliedForOfferReturnsCorrectInformationWhenTrue()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var service = this.GetMockedService(repository);
            var result = service.HasCandidateAppliedForOffer("First", "111");

            Assert.True(result);
        }

        [Fact]
        public async Task HasCandidateAppliedForOfferReturnsCorrectInfoWhenFalse()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var service = this.GetMockedService(repository);
            var result = service.HasCandidateAppliedForOffer("Second", "112");

            Assert.False(result);
        }

        [Fact]
        public async Task HasCandidateAppliedForOfferReturnsCorrectInfoWhenApplicationIsRetracted()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var service = this.GetMockedService(repository);
            var result = service.HasCandidateAppliedForOffer("Second", "3");

            Assert.False(result);
        }

        [Fact]
        public async Task
            IsUserRelatedToJobApplicationReturnsCorrectInformationWhenTrue()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var service = this.GetMockedService(repository);
            var result = service.IsUserRelatedToJobApplication("1", "UserId");

            Assert.True(result);
        }

        [Fact]
        public async Task
            IsUserRelatedToJobApplicationReturnsCorrectInformationWhenFalse()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var service = this.GetMockedService(repository);
            var result = service.IsUserRelatedToJobApplication("2", "UserId");

            Assert.False(result);
        }

        [Fact]
        public async Task
            GetJobApplicationDetailsReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var service = this.GetMockedService(repository);
            var result = service.GetJobApplicationDetails<CandidateJobApplicationsViewModel>("1");

            Assert.NotNull(result);
            Assert.Equal("CEO", result.JobOfferPosition);
            Assert.Equal("First", result.ApplicationStatusName);
        }

        [Fact]
        public async Task
           ApplySavesTheRecordCorrectly()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.ApplicationStatuses.AddAsync(new JobApplicationStatus { Name = "Under Review", Id = 1 });
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var documentsRepository = new EfDeletableEntityRepository<JobApplicationDocument>(context);
            var jobOffersRepository = new Mock<EfDeletableEntityRepository<JobOffer>>(context);
            jobOffersRepository.Setup(r => r.AllAsNoTracking()).Returns(new List<JobOffer>
            {
                new JobOffer
                {
                    Id = "333",
                    Position = "CEO",
                    Employer = new Employer
                    {
                        ContactPersonEmail = "email",
                        ContactPersonNames = "Georgi Georgiev",
                        ApplicationUser = new ApplicationUser
                        { Email = "OtherEmail" },
                    },
                },
            }.AsQueryable());

            var service = this.GetMockedService(repository, documentsRepository, jobOffersRepository.Object);

            var model = new ApplyViewModel
            {
                JobOfferId = "333",
                CandidateId = "1",
                CandidateDetails = new CandidateContactDetailsViewModel
                {
                    ApplicationUserEmail = "userEmail",
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    PhoneNumber = "1234567890",
                },
                JobOfferDetails = new JobApplicationJobOfferDetailsViewModel
                {
                    Position = "CEO",
                },
                DocumentIds = new List<string> { "First" },
            };
            var result = service.Apply(model, "someUrl");

            Assert.NotNull(result);
            Assert.NotEqual(0, context.JobApplicationDocuments.Count());
        }

        [Fact]
        public async Task ChangeJobApplicationStatusCorrectlyUpdatesStatus()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var statusesRepository = new EfDeletableEntityRepository<JobApplicationStatus>(context);
            var service = this.GetMockedService(repository, null, null, statusesRepository, null);

            var result = await service.ChangeJobApplicationStatus("1", 2, "someUrl");

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task ChangeJobApplicationStatusDoesNotUpdateStatusIfApplicationIsNull()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var statusesRepository = new EfDeletableEntityRepository<JobApplicationStatus>(context);
            var service = this.GetMockedService(repository, null, null, statusesRepository, null);

            var result = await service.ChangeJobApplicationStatus("invalidId", 2, "someUrl");

            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task ChangeJobApplicationStatusDoesNotUpdateStatusIfStatusDoesNotExist()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.JobApplications.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<JobApplication>(context);
            var statusesRepository = new EfDeletableEntityRepository<JobApplicationStatus>(context);
            var service = this.GetMockedService(repository, null, null, statusesRepository, null);

            var result = await service.ChangeJobApplicationStatus("1", 2222, "someUrl");

            Assert.Equal(-1, result);
        }

        private JobApplicationsService GetMockedService(
            IDeletableEntityRepository<JobApplication> applicationsRepository,
            IDeletableEntityRepository<JobApplicationDocument> documentsRepository = null,
            IDeletableEntityRepository<JobOffer> offersRepository = null,
            IDeletableEntityRepository<JobApplicationStatus> statusesRepository = null,
            IEmailSender emailSender = null)
        {
            var mockDocumentsRepository = documentsRepository ?? new Mock<IDeletableEntityRepository<JobApplicationDocument>>().Object;
            var mockOffersRepository = offersRepository ?? new Mock<IDeletableEntityRepository<JobOffer>>().Object;
            var mockStatusesRepository = statusesRepository ?? new Mock<IDeletableEntityRepository<JobApplicationStatus>>().Object;
            var mockEmailSender = emailSender ?? new Mock<IEmailSender>().Object;
            var configuration = new Mock<IConfiguration>().Object;

            return new JobApplicationsService(applicationsRepository, mockDocumentsRepository, mockOffersRepository, mockStatusesRepository, mockEmailSender, configuration);
        }

        private IEnumerable<JobApplication> SeedTestData()
        {
            return new List<JobApplication>
            {
                new JobApplication
                {
                    Id = "1",
                    Candidate = new Candidate
                    {
                        FirstName = "Ivan",
                        LastName = "Ivanov",
                        Education = "someEducation",
                        Id = "First",
                        ApplicationUser = new ApplicationUser { Id = "UserId" },
                    },
                    JobOffer = new JobOffer
                    {
                        Position = "CEO",
                        Id = "111",
                        Employer = new Employer
                        {
                            ApplicationUser = new ApplicationUser { Id = "333" },
                        },
                    },
                    CreatedOn = DateTime.UtcNow.AddHours(-1),
                    ApplicationStatus = new JobApplicationStatus { Name = "First", Id = 1 },
                    JobOfferId = "111",
                    Message = "someText",
                },
                new JobApplication
                {
                    Id = "2",
                    Candidate = new Candidate
                    {
                        FirstName = "Petar",
                        LastName = "Petrov",
                        Education = "someEducation",
                        Id = "Second",
                        ApplicationUserId = "OtherUserId",
                    },
                    CreatedOn = DateTime.UtcNow.AddHours(-12),
                    JobOfferId = "111",
                    ApplicationStatus = new JobApplicationStatus { Name = "Second", Id = 2 },
                    Message = "someText",
                },
                new JobApplication
                {
                    Id = "3",
                    CandidateId = "Second",
                    CreatedOn = DateTime.UtcNow.AddHours(-25),
                    JobOfferId = "111",
                    ApplicationStatus = new JobApplicationStatus { Name = "Retracted", Id = 3 },
                    Message = "someText",
                },
                new JobApplication
                {
                    Id = "4",
                    CandidateId = "Third",
                    Candidate = new Candidate { FirstName = "Georgi", LastName = "Georgiev", Education = "someEducation", Id = "Third" },
                    CreatedOn = DateTime.UtcNow.AddHours(-8),
                    JobOfferId = "112",
                    ApplicationStatusId = 6,
                    Message = "someText",
                },
            };
        }
    }
}
