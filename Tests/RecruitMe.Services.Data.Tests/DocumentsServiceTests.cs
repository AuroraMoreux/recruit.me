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
    using Moq;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Data.Repositories;
    using RecruitMe.Services.Data.Tests.Common;
    using RecruitMe.Web.ViewModels.Documents;
    using Xunit;

    public class DocumentsServiceTests
    {
        [Fact]
        public async Task GetAllDocumentsForCandidateReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Documents.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Document>(context);
            var documentsService = this.GetMockedService(repository, null, null);

            var result = documentsService.GetAllDocumentsForCandidate<DocumentsViewModel>("CandidateId");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task DocumentNameAlreadyExistsReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Documents.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Document>(context);
            var documentsService = this.GetMockedService(repository, null, null);

            var result = documentsService.DocumentNameAlreadyExists("FiLE1.DOC", "CandidateId");

            Assert.True(result);
        }

        [Fact]
        public async Task IsCandidateOwnerOfDocumentReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Documents.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Document>(context);
            var documentsService = this.GetMockedService(repository, null, null);

            var result = documentsService.IsCandidateOwnerOfDocument("CandidateId", "11");

            Assert.True(result);
        }

        [Fact]
        public async Task GetDocumentNameByIdReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();
            await context.Documents.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Document>(context);
            var documentsService = this.GetMockedService(repository, null, null);

            var result = documentsService.GetDocumentNameById("11");

            Assert.Equal("File1.doc", result);
        }

        [Fact]
        public async Task DeleteMarksRecordAsDeletedButPreservesTheDbEntry()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            await context.Documents.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Document>(context);
            var documentsService = this.GetMockedService(repository, null, null);
            var result = await documentsService.DeleteAsync("11");

            Assert.True(result);
            var dbRecord = await context.Documents.FindAsync("11");

            Assert.True(dbRecord.IsDeleted);
            Assert.NotNull(dbRecord.DeletedOn);
            Assert.Equal(1, context.Documents.Count());
        }

        [Fact]
        public async Task DownloadAsyncReturnsCorrectFile()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            await context.Documents.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Document>(context);
            var mockFileDownload = new Mock<IFileDownloadService>();
            mockFileDownload.Setup(m => m.DownloadFileAsync(It.IsAny<string>())).Returns(Task.FromResult(new byte[7]));
            var documentsService = this.GetMockedService(repository, mockFileDownload.Object, null);

            var result = await documentsService.DownloadAsync("11");

            Assert.Equal(7, result.Length);
        }

        [Fact]
        public async Task GetDocumentCountForCandidateReturnsCorrectCount()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            await context.Documents.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Document>(context);
            var mockFileDownload = new Mock<IFileDownloadService>();
            mockFileDownload.Setup(m => m.DownloadFileAsync(It.IsAny<string>())).Returns(Task.FromResult(new byte[7]));
            var documentsService = this.GetMockedService(repository, mockFileDownload.Object, null);

            var result = documentsService.GetDocumentCountForCandidate("CandidateId");

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task UploadAsyncSavesFileSuccessfully()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            var repository = new EfDeletableEntityRepository<Document>(context);
            var cloudinary = new Cloudinary(new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret));
            var documentsService = this.GetMockedService(repository, null, cloudinary);

            var model = new UploadInputModel
            {
                DocumentCategoryId = 123,
                File = this.PrepareFile(),
            };

            var result = await documentsService.UploadAsync(model, "1");

            Assert.NotNull(result);
            Assert.Equal(1, context.Documents.Count());
        }

        [Fact]
        public async Task GetDocumentDetailsReturnsCorrectInformation()
        {
            AutoMapperInitializer.InitializeMapper();
            var context = InMemoryDbContextInitializer.InitializeContext();

            await context.Documents.AddRangeAsync(this.SeedTestData());
            await context.SaveChangesAsync();

            var repository = new EfDeletableEntityRepository<Document>(context);

            var documentsService = this.GetMockedService(repository);

            var result = documentsService.GetDocumentDetails<DeleteViewModel>("11");

            Assert.NotNull(result);
            Assert.Equal("File1.doc", result.Name);
        }

        private DocumentsService GetMockedService(IDeletableEntityRepository<Document> documentsRepository, IFileDownloadService fileDownloadService = null, Cloudinary cloudinary = null)
        {
            var mockFileDownload = fileDownloadService ?? new Mock<IFileDownloadService>().Object;
            var mockCloudinary = cloudinary ?? new Mock<Cloudinary>(new Account("cloudName", "key", "secret")).Object;
            return new DocumentsService(documentsRepository, mockFileDownload, mockCloudinary);
        }

        private FormFile PrepareFile()
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

        private IEnumerable<Document> SeedTestData()
        {
            return new List<Document>
            {
               new Document
               {
                   Id = "11",
                   Name = "File1.doc",
                   CandidateId = "CandidateId",
                   CreatedOn = DateTime.UtcNow.AddHours(-1),
                   IsDeleted = false,
                   DocumentCategory = new DocumentCategory { Name = "Resume", Id = 1 },
                   Size = 100,
                   Url = "SomeUrl",
               },
               new Document
               {
                   Id = "12",
                   Name = "File2.doc",
                   CandidateId = "CandidateId",
                   CreatedOn = DateTime.UtcNow.AddHours(-1),
                   IsDeleted = false,
                   DocumentCategory = new DocumentCategory { Name = "Portfolio", Id = 2 },
                   Size = 1000,
                   Url = "SomeOtherUrl",
               },
            };
        }
    }
}
