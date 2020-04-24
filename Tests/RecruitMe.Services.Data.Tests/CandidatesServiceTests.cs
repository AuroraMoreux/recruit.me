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
    using RecruitMe.Data.Repositories;
    using RecruitMe.Services.Data;
    using RecruitMe.Services.Data.Tests.Common;
    using RecruitMe.Web.ViewModels.Candidates;
    using Xunit;

    public class CandidatesServiceTests
    {
        [Fact]
        public async Task GetCandidateIdByUsernameReturnsCorrectResult()
        {
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Candidates.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Candidate>(context);
            var candidatesService = this.GetMockedService(repository);

            var candidateId = candidatesService.GetCandidateIdByUsername("searchableId");

            Assert.Equal("CorrectResult", candidateId);
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectResult()
        {
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Candidates.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Candidate>(context);
            var candidatesService = this.GetMockedService(repository);

            var count = candidatesService.GetCount();

            Assert.Equal(this.SeedTestData().Count(), count);
        }

        [Fact]
        public async Task GetNewCandidatesCountShouldReturnCorrectResult()
        {
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Candidates.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Candidate>(context);
            var candidatesService = this.GetMockedService(repository);

            var count = candidatesService.GetNewCandidatesCount();

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task CreateProfileAsyncSavesInfoCorrectly()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            var candidatesRepository = new EfDeletableEntityRepository<Candidate>(context);
            var languagesRepository = new EfDeletableEntityRepository<CandidateLanguage>(context);
            var skillsRepository = new EfDeletableEntityRepository<CandidateSkill>(context);

            var candidatesService = this.GetMockedService(candidatesRepository, languagesRepository, skillsRepository, null);

            var model = new CreateCandidateProfileInputModel
            {
                AboutMe = "some info",
                ApplicationUserId = "11",
                ContactAddress = "address",
                FirstName = "Georgi",
                LastName = "Georgiev",
                LanguagesIds = new List<int> { 1, 2, 3 },
                SkillsIds = new List<int> { 4, 5, 6 },
            };

            var candidateId = await candidatesService.CreateProfileAsync(model);

            Assert.NotNull(candidateId);

            var record = await context.Candidates.Include(c => c.Languages).Include(c => c.Skills).Where(c => c.Id == candidateId).FirstOrDefaultAsync();

            Assert.Equal(model.AboutMe, record.AboutMe);
            Assert.Equal(model.ApplicationUserId, record.ApplicationUserId);
            Assert.Equal(model.ContactAddress, record.ContactAddress);
            Assert.Null(record.Education);
            Assert.Null(record.PhoneNumber);
            Assert.Null(record.ProfilePictureUrl);
            Assert.Equal(3, record.Languages.Count());
            Assert.Equal(3, record.Skills.Count());
        }

        [Fact]
        public async Task IfProfilePictureIsNotNullCreateProfileSavesCorrectLink()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            var candidateRepository = new EfDeletableEntityRepository<Candidate>(context);
            var cloudinary = new Cloudinary(new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret));
            var candidatesService = this.GetMockedService(candidateRepository, null, null, cloudinary);

            var profilePicture = this.PrepareImage();

            var model = new CreateCandidateProfileInputModel
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                ApplicationUserId = "1",
                ProfilePicture = profilePicture,
            };

            var candidateId = await candidatesService.CreateProfileAsync(model);

            Assert.NotNull(candidateId);

            var pictureUrl = context.Candidates.Find(candidateId).ProfilePictureUrl;

            Assert.NotNull(pictureUrl);
            Assert.Contains(model.ApplicationUserId + "_profilePicture", pictureUrl);

            CloudinaryService.DeleteFile(cloudinary, model.ApplicationUserId + "_profilePicture");
        }

        [Fact]
        public async Task IfPictureIsInvalidCreateProfileReturnsNull()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            var candidatesRepository = new EfDeletableEntityRepository<Candidate>(context);
            var cloudinary = new Cloudinary(new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret));
            var candidatesService = this.GetMockedService(candidatesRepository, null, null, cloudinary);

            var profilePicture = this.PrepareInvalidImage();
            var model = new CreateCandidateProfileInputModel
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                ApplicationUserId = "1",
                ProfilePicture = profilePicture,
            };

            var candidateId = await candidatesService.CreateProfileAsync(model);

            Assert.Null(candidateId);
        }

        [Fact]
        public async Task GetProfileDetailsReturnsCorrectData()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Candidates.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var candidatesRepository = new EfDeletableEntityRepository<Candidate>(context);
            var candidatesService = this.GetMockedService(candidatesRepository, null, null, null);

            var result = candidatesService.GetProfileDetails<ProfileViewModel>("First");

            Assert.NotNull(result);
            Assert.Equal("Ivan Ivanov", result.Name);
        }

        [Fact]
        public async Task IfCandidateDoesNotExistUpdateProfileReturnsNull()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Candidates.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var candidatesRepository = new EfDeletableEntityRepository<Candidate>(context);
            var candidatesService = this.GetMockedService(candidatesRepository, null, null, null);

            var result = candidatesService.GetProfileDetails<ProfileViewModel>("IdNotInDatabase");
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProfileShouldUpdateProfileWhenValidIdIsPassed()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Candidates.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var candidatesRepository = new EfDeletableEntityRepository<Candidate>(context);
            var languagesRepository = new EfDeletableEntityRepository<CandidateLanguage>(context);
            var skillsRepository = new EfDeletableEntityRepository<CandidateSkill>(context);
            var cloudinary = new Cloudinary(new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret));
            var candidatesService = this.GetMockedService(candidatesRepository, languagesRepository, skillsRepository, cloudinary);

            var profilePicture = this.PrepareImage();

            var model = new UpdateCandidateProfileViewModel
            {
                AboutMe = "some info",
                ContactAddress = "address",
                FirstName = "Georgi",
                LastName = "Georgiev",
                LanguagesIds = new List<int> { 1, 2, 3 },
                SkillsIds = new List<int> { 4, 5, 6 },
                ProfilePicture = profilePicture,
            };

            var candidateId = await candidatesService.UpdateProfileAsync("First", model);

            Assert.NotNull(candidateId);

            var record = await context.Candidates.Include(c => c.Languages).Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == "First");

            Assert.Equal(model.FirstName, record.FirstName);
            Assert.Equal(model.LastName, record.LastName);
            Assert.Equal(model.AboutMe, record.AboutMe);
            Assert.Null(record.PhoneNumber);
            Assert.NotNull(record.ProfilePictureUrl);
            Assert.Equal(3, record.Languages.Count());
            Assert.Equal(3, record.Skills.Count());
        }

        [Fact]
        public async Task UpdateProfileCorrectlyUpdatesProfilePictureUrlWhenItIsNotNull()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Candidates.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var candidatesRepository = new EfDeletableEntityRepository<Candidate>(context);
            var languagesRepository = new EfDeletableEntityRepository<CandidateLanguage>(context);
            var skillsRepository = new EfDeletableEntityRepository<CandidateSkill>(context);
            var cloudinary = new Cloudinary(new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret));
            var candidatesService = this.GetMockedService(candidatesRepository, languagesRepository, skillsRepository, cloudinary);

            var profilePicture = this.PrepareImage();

            var candidateId = await candidatesService.UpdateProfileAsync("Second", new UpdateCandidateProfileViewModel { ProfilePicture = profilePicture });

            Assert.NotNull(candidateId);

            var profilePictureUrl = context.Candidates.Find(candidateId).ProfilePictureUrl;

            Assert.NotEqual("someUrl", profilePictureUrl);
        }

        [Fact]
        public async Task UpdateProfileCorrectlyRemovesOldLanguagesAndSkills()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            var candidatesRepository = new EfDeletableEntityRepository<Candidate>(context);
            var languagesRepository = new EfDeletableEntityRepository<CandidateLanguage>(context);
            var skillsRepository = new EfDeletableEntityRepository<CandidateSkill>(context);
            var candidatesService = new CandidatesService(candidatesRepository, languagesRepository, skillsRepository, null);

            var candidateId = await candidatesService.CreateProfileAsync(new CreateCandidateProfileInputModel { FirstName = "Ivan", LastName = "Ivanov", ApplicationUserId = "1", LanguagesIds = new List<int> { 1 }, SkillsIds = new List<int> { 1 } });

            var updateModel = new UpdateCandidateProfileViewModel
            {
                LanguagesIds = new List<int> { 2, 3 },
                SkillsIds = new List<int> { 4, 5 },
            };

            await candidatesService.UpdateProfileAsync(candidateId, updateModel);

            var candidateLanguages = await context.Candidates
                .Where(c => c.Id == candidateId)
                .Select(c => c.Languages.Select(l => l.LanguageId).ToList())
                .FirstOrDefaultAsync();

            var candidateSkills = await context.Candidates
                .Where(c => c.Id == candidateId)
                .Select(c => c.Skills.Select(l => l.SkillId).ToList())
                .FirstOrDefaultAsync();

            Assert.Equal(2, candidateLanguages.Count);
            Assert.Equal(2, candidateSkills.Count);
            Assert.Contains(2, candidateLanguages);
            Assert.Contains(3, candidateLanguages);
            Assert.Contains(4, candidateSkills);
            Assert.Contains(5, candidateSkills);
        }

        private CandidatesService GetMockedService(IDeletableEntityRepository<Candidate> candidatesRepository, IDeletableEntityRepository<CandidateLanguage> languagesRepository = null, IDeletableEntityRepository<CandidateSkill> skillsRepository = null, Cloudinary cloudinary = null)
        {
            var mockLanguagesRepository = languagesRepository ?? new Mock<IDeletableEntityRepository<CandidateLanguage>>().Object;
            var mockSkillsRepository = skillsRepository ?? new Mock<IDeletableEntityRepository<CandidateSkill>>().Object;
            var mockCloudinary = cloudinary ?? new Mock<Cloudinary>(new Account("cloudName", "key", "secret")).Object;
            return new CandidatesService(candidatesRepository, mockLanguagesRepository, mockSkillsRepository, mockCloudinary);
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

        private IEnumerable<Candidate> SeedTestData()
        {
            return new List<Candidate>
            {
                new Candidate
                {
                    Id = "CorrectResult",
                    ApplicationUser = new ApplicationUser { UserName = "searchableId" },
                    CreatedOn = DateTime.UtcNow.AddHours(-15),
                },
                new Candidate
                {
                    Id = "First",
                    ApplicationUser = new ApplicationUser { UserName = "Ivan" },
                    CreatedOn = DateTime.UtcNow.AddHours(-25),
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    PhoneNumber = "1234567890",
                },
                new Candidate
                {
                    Id = "Second",
                    ApplicationUser = new ApplicationUser { UserName = "Pesho" },
                    CreatedOn = DateTime.UtcNow.AddHours(-23),
                    ProfilePictureUrl = "someUrl",
                },
            };
        }
    }
}
