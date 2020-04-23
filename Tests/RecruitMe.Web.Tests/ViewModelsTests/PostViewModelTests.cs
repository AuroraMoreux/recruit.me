namespace RecruitMe.Web.Tests.ViewModelsTests
{
    using System;
    using System.Linq;

    using RecruitMe.Web.ViewModels.JobOffers;
    using Xunit;

    public class PostViewModelTests
    {
        [Fact]
        public void ValidationFailsWhenValidFromDateIsGreaterThanValidUntil()
        {
            var model = new PostViewModel
            {
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddDays(-3),
            };

            var errorsCount = model.Validate(null).Count();

            Assert.True(errorsCount > 0);
        }

        [Fact]
        public void ValidationFailsWhenCurrentDateIsGreaterThanValidFromDate()
        {
            var model = new PostViewModel
            {
                ValidFrom = DateTime.UtcNow.AddDays(-1),
                ValidUntil = DateTime.UtcNow.AddDays(3),
            };

            var errorsCount = model.Validate(null).Count();

            Assert.True(errorsCount > 0);
        }

        [Fact]
        public void ValidationReturnsMultipleErrorCountWhenDatesAreNotCorrect()
        {
            var model = new PostViewModel
            {
                ValidFrom = DateTime.UtcNow.AddDays(-1),
                ValidUntil = DateTime.UtcNow.AddDays(-4),
            };

            var errorsCount = model.Validate(null).Count();

            Assert.True(errorsCount > 1);
        }

        [Fact]
        public void ValidationPassesWhenValidFromDateIsGreaterThanCurrentDateAndValidUntilDateIsGreaterThanValidFromDate()
        {
            var model = new PostViewModel
            {
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddDays(1),
            };

            var errorsCount = model.Validate(null).Count();
            Assert.Equal(0, errorsCount);
        }
    }
}
