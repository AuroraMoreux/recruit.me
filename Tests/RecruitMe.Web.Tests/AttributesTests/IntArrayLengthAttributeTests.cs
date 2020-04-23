namespace RecruitMe.Web.Tests.AttributesTests
{
    using System.Collections.Generic;

    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using Xunit;

    public class IntArrayLengthAttributeTests
    {
        [Fact]
        public void ValidationPassesWhenMinLengthIsNotSetAndArrayIsEmpty()
        {
            var array = new List<int>();
            var attribute = new IntArrayLengthAttribute("Field", 3);

            var isValid = attribute.IsValid(array);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidationFailsWhenMinLengthIsNotSetAndArrayIsNull()
        {
            List<int> array = null;
            var attribute = new IntArrayLengthAttribute("Field", 3);

            var isValid = attribute.IsValid(array);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new object[] { new int[0] })]
        [InlineData(new object[] { new int[] { 1, 20, 5555, 78944, 2 } })]
        public void ValidationFailsWhenArrayLengthIsNotBetweenOneAndThree(ICollection<int> array)
        {
            var attribute = new IntArrayLengthAttribute("Field", 3, 1);

            var isValid = attribute.IsValid(array);

            Assert.False(isValid);
        }

        [Fact]
        public void IfMaxLengthIsGreaterThanMinLengthBothFieldsAreSetToTheSameValue()
        {
            var minLength = 3;
            var maxLength = 1;
            var attribute = new IntArrayLengthAttribute("Field", maxLength, minLength);

            Assert.Equal(attribute.MaxLength, minLength);
        }

        [Fact]
        public void IfMinLengthIsNegativeItIsSetToZero()
        {
            var minLength = -3;
            var attribute = new IntArrayLengthAttribute("Field", 4, minLength);

            Assert.Equal(0, attribute.MinLength);
        }

        [Theory]
        [InlineData(new object[] { new int[] { 1 } })]
        [InlineData(new object[] { new int[] { 222, 22222 } })]
        [InlineData(new object[] { new int[] { 999999, 665544, 3 } })]
        public void ValidationPassesWhenArrayLengthIsWithinBoundaries(ICollection<int> array)
        {
            var attribute = new IntArrayLengthAttribute("Field", 3, 1);

            var isValid = attribute.IsValid(array);

            Assert.True(isValid);
        }
    }
}
