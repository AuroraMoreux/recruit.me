namespace RecruitMe.Web.Tests.ViewModelsTests
{
    using System;
    using System.Linq;

    using RecruitMe.Web.ViewModels.JobOffers;
    using Xunit;

    public class FilterModelTests
    {
        [Fact]
        public void ValidationFailsWhenValidFromDateIsGreaterThanValidUntil()
        {
            var model = new FilterModel
            {
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddDays(-3),
            };

            var errorsCount = model.Validate(null).Count();

            Assert.True(errorsCount > 0);
        }

        [Fact]
        public void ValidationFailsWhenSalaryFromIsGreaterThanSalaryTo()
        {
            var model = new FilterModel
            {
                SalaryFrom = 1000,
                SalaryTo = 10,
            };

            var errorsCount = model.Validate(null).Count();

            Assert.True(errorsCount > 0);
        }

        [Fact]
        public void ValidationReturnsMultipleErrorCountWhenDatesAndSalariesAreNotCorrect()
        {
            var model = new FilterModel
            {
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddDays(-4),
                SalaryFrom = 1000,
                SalaryTo = 10,
            };

            var errorsCount = model.Validate(null).Count();

            Assert.True(errorsCount > 1);
        }

        [Fact]
        public void ValidationPassesWhenDatesAndSalariesAreCorrect()
        {
            var model = new FilterModel
            {
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddDays(1),
                SalaryFrom = 10,
                SalaryTo = 1000,
            };

            var errorsCount = model.Validate(null).Count();
            Assert.Equal(0, errorsCount);
        }
    }
}
