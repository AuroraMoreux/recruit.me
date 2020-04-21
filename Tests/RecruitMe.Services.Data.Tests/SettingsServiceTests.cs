namespace RecruitMe.Services.Data.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class SettingsServiceTests
    {
        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            //Mock<IDeletableEntityRepository<Setting>> repository = new Mock<IDeletableEntityRepository<Setting>>();
            //repository.Setup(r => r.All()).Returns(new List<Setting>
            //                                            {
            //                                                new Setting(),
            //                                                new Setting(),
            //                                                new Setting(),
            //                                            }.AsQueryable());
            //SettingsService service = new SettingsService(repository.Object);
            //Assert.Equal(3, service.GetCount());
            //repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public void GetCountShouldReturnCorrectNumberUsingDbContext()
        {
            //DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //    .UseInMemoryDatabase(databaseName: "SettingsTestDb").Options;
            //ApplicationDbContext dbContext = new ApplicationDbContext(options);
            //dbContext.Settings.Add(new Setting());
            //dbContext.Settings.Add(new Setting());
            //dbContext.Settings.Add(new Setting());
            //await dbContext.SaveChangesAsync();

            //EfDeletableEntityRepository<Setting> repository = new EfDeletableEntityRepository<Setting>(dbContext);
            //SettingsService service = new SettingsService(repository);
            //Assert.Equal(3, service.GetCount());
        }
    }
}
