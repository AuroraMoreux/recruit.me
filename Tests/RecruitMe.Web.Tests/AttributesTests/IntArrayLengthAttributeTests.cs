namespace RecruitMe.Web.Tests.AttributesTests
{
    using System.Collections.Generic;

    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using Xunit;

    public class IntArrayLengthAttributeTests
    {
        [Fact]
        public void ValidationPassesWhenMinLengthIsZeroAndArrayIsEmpty()
        {
            // Arrange
            List<int> list = new List<int>();
            IntArrayLengthAttribute attribute = new IntArrayLengthAttribute(string.Empty, 3);

            // Act
            bool isValid = attribute.IsValid(list);

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new object[] { new int[0] })]
        [InlineData(new object[] { new int[] { 1, 20, 5555, 78944, 2 } })]
        public void ValidationFailsWhenArrayLengthIsNotBetweenOneAndThree(IEnumerable<int> list)
        {
            // Arrange
            IntArrayLengthAttribute attribute = new IntArrayLengthAttribute(string.Empty, 3, 1);

            // Act
            bool isValid = attribute.IsValid(list);

            // Assert
            Assert.False(isValid);
        }
    }
}
