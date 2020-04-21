namespace RecruitMe.Web.Tests.ViewModelsTests
{
    using System;
    using System.Linq;

    using RecruitMe.Web.ViewModels.JobOffers;
    using Xunit;

    public class EditJobOfferDetailsModelTests
    {
        [Fact]
        public void ValidationFailsWhenValidUntilDateIsGreaterThanValidUntil()
        {
            EditJobOfferDetailsModel model = new EditJobOfferDetailsModel
            {
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddDays(-3),
            };

            int errorsCount = model.Validate(null).Count();

            Assert.True(errorsCount > 0);
        }

        [Fact]
        public void ValidationFailsWhenCurrentDateIsGreaterThanValidFromDate()
        {
            EditJobOfferDetailsModel model = new EditJobOfferDetailsModel
            {
                ValidFrom = DateTime.UtcNow.AddDays(-1),
                ValidUntil = DateTime.UtcNow.AddDays(3),
            };

            int errorsCount = model.Validate(null).Count();

            Assert.True(errorsCount > 0);
        }

        [Fact]
        public void ValidationReturnsMultipleErrorCountWhenDatesAreNotCorrect()
        {
            EditJobOfferDetailsModel model = new EditJobOfferDetailsModel
            {
                ValidFrom = DateTime.UtcNow.AddDays(-1),
                ValidUntil = DateTime.UtcNow.AddDays(-4),
            };

            int errorsCount = model.Validate(null).Count();

            Assert.True(errorsCount > 1);
        }

        [Fact]
        public void ValidationPassesWhenValidFromDateIsGreaterThanCurrentDateAndValidUntilDateIsGreaterThanValidFromDate()
        {
            EditJobOfferDetailsModel model = new EditJobOfferDetailsModel
            {
                ValidFrom = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddDays(1),
            };

            int errorsCount = model.Validate(null).Count();
            Assert.Equal(0, errorsCount);
        }
    }
}
