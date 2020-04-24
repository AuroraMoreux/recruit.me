namespace RecruitMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Data.Repositories;
    using RecruitMe.Services.Data.Tests.Common;
    using RecruitMe.Web.ViewModels.Employers;
    using RecruitMe.Web.ViewModels.Shared;
    using Xunit;

    public class EmployersServiceTests
    {
        [Fact]
        public async Task GetEmployerIdByUsernameReturnsCorrectResult()
        {
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Employers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var employersService = this.GetMockedService(repository);

            var employerId = employersService.GetEmployerIdByUsername("searchableId");

            Assert.Equal("CorrectResult", employerId);
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectResult()
        {
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Employers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var employersService = this.GetMockedService(repository);

            var count = employersService.GetCount();

            Assert.Equal(this.SeedTestData().Count(), count);
        }

        [Fact]
        public async Task GetNewEmployersCountShouldReturnCorrectResult()
        {
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Employers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var employersService = this.GetMockedService(repository);

            var count = employersService.GetNewEmployersCount();

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task CreateProfileAsyncSavesInfoCorrectly()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            var repository = new EfDeletableEntityRepository<Employer>(context);

            var employersService = this.GetMockedService(repository, null);

            var model = new CreateEmployerProfileInputModel
            {
                ApplicationUserId = "11",
                Address = "address",
                Name = "Recruit Me",
                ContactPersonNames = "Ivan Ivanov",
                ContactPersonEmail = "ivan@ivanov.com",
                JobSectorId = 4,
                UniqueIdentificationCode = "204558718",
            };

            var employerId = await employersService.CreateProfileAsync(model);

            Assert.NotNull(employerId);

            var record = await context.Employers.Where(c => c.Id == employerId).FirstOrDefaultAsync();

            Assert.Equal(model.Name, record.Name);
            Assert.Equal(model.ApplicationUserId, record.ApplicationUserId);
            Assert.Equal(model.Address, record.Address);
            Assert.Null(record.WebsiteAddress);
            Assert.Null(record.PhoneNumber);
            Assert.Null(record.LogoUrl);
            Assert.Equal(4, record.JobSectorId);
            Assert.NotNull(record.ContactPersonNames);
            Assert.NotNull(record.ContactPersonEmail);
            Assert.NotNull(record.UniqueIdentificationCode);
        }

        [Fact]
        public async Task IfLogoIsNotNullCreateProfileSavesCorrectLink()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var cloudinary = new Cloudinary(new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret));
            var employersService = this.GetMockedService(repository, cloudinary);

            var logo = this.PrepareImage();

            var model = new CreateEmployerProfileInputModel
            {
                ApplicationUserId = "1",
                Name = "Recruit Me",
                ContactPersonNames = "Ivan Ivanov",
                ContactPersonEmail = "ivan@ivanov.com",
                JobSectorId = 4,
                UniqueIdentificationCode = "204558718",
                Logo = logo,
            };

            var employerId = await employersService.CreateProfileAsync(model);

            Assert.NotNull(employerId);

            var logoUrl = context.Employers.Find(employerId).LogoUrl;

            Assert.NotNull(logoUrl);
            Assert.Contains(model.ApplicationUserId + "_logo", logoUrl);

            CloudinaryService.DeleteFile(cloudinary, model.ApplicationUserId + "_logo");
        }

        [Fact]
        public async Task IfLogoIsInvalidCreateProfileReturnsNull()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var cloudinary = new Cloudinary(new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret));
            var employersService = this.GetMockedService(repository, cloudinary);

            var logo = this.PrepareInvalidImage();
            var model = new CreateEmployerProfileInputModel
            {
                ApplicationUserId = "1",
                Name = "Recruit Me",
                ContactPersonNames = "Ivan Ivanov",
                ContactPersonEmail = "ivan@ivanov.com",
                JobSectorId = 4,
                UniqueIdentificationCode = "204558718",
                Logo = logo,
            };

            var employerId = await employersService.CreateProfileAsync(model);

            Assert.Null(employerId);
        }

        [Fact]
        public async Task GetProfileDetailsReturnsCorrectData()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Employers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var employersService = this.GetMockedService(repository, null);

            var result = employersService.GetProfileDetails<ProfileViewModel>("First");

            Assert.NotNull(result);
            Assert.Equal("Recruit Me", result.Name);
            Assert.Equal("company@company.com", result.ApplicationUserEmail);
            Assert.Equal("Sector", result.JobSectorName);
        }

        [Fact]
        public async Task IfEmployerDoesNotExistUpdateProfileReturnsNull()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Employers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var employersService = this.GetMockedService(repository, null);

            var result = employersService.GetProfileDetails<ProfileViewModel>("IdNotInDatabase");
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProfileShouldUpdateProfileWhenValidIdIsPassed()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Employers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var cloudinary = new Cloudinary(new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret));
            var employersService = this.GetMockedService(repository, cloudinary);

            var logo = this.PrepareImage();

            var model = new UpdateEmployerProfileViewModel
            {
                Name = "New Name",
                ContactPersonNames = "Georgi Georgiev",
                ContactPersonEmail = "georgi@georgiev.com",
                UniqueIdentificationCode = "0009039460577",
                JobSectorId = 3,
                Logo = logo,
            };
            var employerId = await employersService.UpdateProfileAsync("First", model);

            Assert.NotNull(employerId);

            var record = await context.Employers.FirstOrDefaultAsync(c => c.Id == "First");

            Assert.Equal(model.Name, record.Name);
            Assert.Equal(model.ContactPersonNames, record.ContactPersonNames);
            Assert.Equal(model.ContactPersonEmail, record.ContactPersonEmail);
            Assert.NotNull(record.LogoUrl);
            Assert.Equal(3, record.JobSectorId);
            Assert.Equal(model.UniqueIdentificationCode, record.UniqueIdentificationCode);
        }

        [Fact]
        public async Task UpdateProfileCorrectlyUpdatesProfilePictureUrlWhenItIsNotNull()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Employers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var cloudinary = new Cloudinary(new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret));
            var employersService = this.GetMockedService(repository, cloudinary);

            var logo = this.PrepareImage();

            var employerId = await employersService.UpdateProfileAsync("Second", new UpdateEmployerProfileViewModel { Logo = logo });

            Assert.NotNull(employerId);

            var logoUrl = context.Employers.Find(employerId).LogoUrl;

            Assert.NotEqual("someUrl", logoUrl);
        }

        [Fact]
        public async Task GetEmployerNameByIdReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Employers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var employersService = this.GetMockedService(repository, null);

            var result = employersService.GetEmployerNameById("First");
            Assert.Equal("Recruit Me", result);
        }

        [Fact]
        public async Task GetTopFiveEmployersReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Employers.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Employer>(context);
            var employersService = this.GetMockedService(repository, null);

            var result = employersService.GetTopFiveEmployers<IndexTopEmployersModel>();

            Assert.DoesNotContain("First", result.Select(r => r.Id).ToList());
        }

        private EmployersService GetMockedService(IDeletableEntityRepository<Employer> employersRepository, Cloudinary cloudinary = null)
        {
            var mockCloudinary = cloudinary ?? new Mock<Cloudinary>(new Account("cloudName", "key", "secret")).Object;
            return new EmployersService(employersRepository, mockCloudinary);
        }

        private FormFile PrepareImage()
        {
            var stream = new MemoryStream(File.ReadAllBytes(@"UploadFiles/testImage.png"));
            var picture = new FormFile(stream, 0, stream.Length, "image", "image.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png",
                ContentDisposition = "form-data",
            };

            return picture;
        }

        private FormFile PrepareInvalidImage()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file"));
            var file = new FormFile(stream, 0, stream.Length, "file", "file.doc")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/msword",
                ContentDisposition = "form-data",
            };

            return file;
        }

        private IEnumerable<Employer> SeedTestData()
        {
            return new List<Employer>
            {
                new Employer
                {
                    Id = "CorrectResult",
                    ApplicationUser = new ApplicationUser { UserName = "searchableId" },
                    CreatedOn = DateTime.UtcNow.AddHours(-15),
                    JobOffers = new HashSet<JobOffer> { new JobOffer { Id = "1" } },
                },
                new Employer
                {
                    Id = "First",
                    ApplicationUser = new ApplicationUser { UserName = "Company", Email = "company@company.com" },
                    CreatedOn = DateTime.UtcNow.AddHours(-25),
                    Name = "Recruit Me",
                    ContactPersonNames = "Ivan Ivanov",
                    ContactPersonEmail = "ivan@ivanov.com",
                    UniqueIdentificationCode = "204558718",
                    JobSector = new JobSector { Name = "Sector" },
                },
                new Employer
                {
                    Id = "Second",
                    ApplicationUser = new ApplicationUser { UserName = "Other Company" },
                    CreatedOn = DateTime.UtcNow.AddHours(-23),
                    LogoUrl = "someUrl",
                    JobOffers = new HashSet<JobOffer> { new JobOffer { Id = "2" } },
                },
                new Employer
                {
                    Id = "Third",
                    CreatedOn = DateTime.UtcNow.AddDays(-2),

                    JobOffers = new HashSet<JobOffer> { new JobOffer { Id = "3" } },
                },
                new Employer
                {
                   Id = "Fourth",
                   CreatedOn = DateTime.UtcNow.AddDays(-2),

                   JobOffers = new HashSet<JobOffer> { new JobOffer { Id = "4" } },
                },
                new Employer
                {
                     Id = "Fifth",
                     CreatedOn = DateTime.UtcNow.AddDays(-2),
                     JobOffers = new HashSet<JobOffer> { new JobOffer { Id = "5" } },
                },
            };
        }
    }
}
