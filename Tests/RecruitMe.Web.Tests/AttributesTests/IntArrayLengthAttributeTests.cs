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
            List<int> array = new List<int>();
            IntArrayLengthAttribute attribute = new IntArrayLengthAttribute("Field", 3);

            bool isValid = attribute.IsValid(array);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidationFailsWhenMinLengthIsNotSetAndArrayIsNull()
        {
            List<int> array = null;
            IntArrayLengthAttribute attribute = new IntArrayLengthAttribute("Field", 3);

            bool isValid = attribute.IsValid(array);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new object[] { new int[0] })]
        [InlineData(new object[] { new int[] { 1, 20, 5555, 78944, 2 } })]
        public void ValidationFailsWhenArrayLengthIsNotBetweenOneAndThree(ICollection<int> array)
        {
            IntArrayLengthAttribute attribute = new IntArrayLengthAttribute("Field", 3, 1);

            bool isValid = attribute.IsValid(array);

            Assert.False(isValid);
        }

        [Fact]
        public void IfMaxLengthIsGreaterThanMinLengthBothFieldsAreSetToTheSameValue()
        {
            int minLength = 3;
            int maxLength = 1;
            IntArrayLengthAttribute attribute = new IntArrayLengthAttribute("Field", maxLength, minLength);

            Assert.Equal(attribute.MaxLength, minLength);
        }

        [Fact]
        public void IfMinLengthIsNegativeItIsSetToZero()
        {
            int minLength = -3;
            IntArrayLengthAttribute attribute = new IntArrayLengthAttribute("Field", 4, minLength);

            Assert.Equal(0, attribute.MinLength);
        }

        [Theory]
        [InlineData(new object[] { new int[] { 1 } })]
        [InlineData(new object[] { new int[] { 222, 22222 } })]
        [InlineData(new object[] { new int[] { 999999, 665544, 3 } })]
        public void ValidationPassesWhenArrayLengthIsWithinBoundaries(ICollection<int> array)
        {
            IntArrayLengthAttribute attribute = new IntArrayLengthAttribute("Field", 3, 1);

            bool isValid = attribute.IsValid(array);

            Assert.True(isValid);
        }
    }
}
