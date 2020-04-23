namespace RecruitMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using Xunit;

    public class ApplicationUsersServiceTests
    {
        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            var repository = GetMockedRepository();

            var service = new ApplicationUsersService(repository.Object);

            Assert.Equal(5, service.GetCount());
            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetNewUsersCountShouldReturnCorrectNumber()
        {
            var repository = GetMockedRepository();

            var service = new ApplicationUsersService(repository.Object);

            Assert.Equal(2, service.GetNewUsersCount());
            repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        private static Mock<IDeletableEntityRepository<ApplicationUser>> GetMockedRepository()
        {
            var repository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            repository.Setup(r => r.AllAsNoTracking()).Returns(new List<ApplicationUser>
            {
                new ApplicationUser { CreatedOn = DateTime.UtcNow.AddDays(-3) },
                new ApplicationUser { CreatedOn = DateTime.UtcNow.AddDays(-4) },
                new ApplicationUser { CreatedOn = DateTime.UtcNow.AddHours(-23) },
                new ApplicationUser { CreatedOn = DateTime.UtcNow.AddHours(-25) },
                new ApplicationUser { CreatedOn = DateTime.UtcNow.AddHours(-15) },
            }.AsQueryable());
            return repository;
        }
    }
}
