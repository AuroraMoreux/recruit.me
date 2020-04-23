namespace RecruitMe.Web.Tests.AttributesTests
{
    using System.Collections.Generic;

    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using Xunit;

    public class StringArrayLengthAttributeTests
    {
        [Fact]
        public void ValidationPassesWhenMinLengthIsNotSetAndArrayIsEmpty()
        {
            var array = new List<string>();
            var attribute = new StringArrayLengthAttribute("Field", 3);

            var isValid = attribute.IsValid(array);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidationFailsWhenMinLengthIsNotSetAndArrayIsNull()
        {
            List<string> array = null;
            var attribute = new StringArrayLengthAttribute("Field", 3);

            var isValid = attribute.IsValid(array);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new object[] { new string[0] })]
        [InlineData(new object[] { new string[] { "Hello, World", "TestString", "", null } })]
        [InlineData(new object[] { new string[] { "OneWord", "TwoWord", "Hello, World", "TestString", "" } })]
        public void ValidationFailsWhenArrayLengthIsNotBetweenOneAndThree(ICollection<string> array)
        {
            var attribute = new StringArrayLengthAttribute("Field", 3, 1);
            var isValid = attribute.IsValid(array);

            Assert.False(isValid);
        }

        [Fact]
        public void IfMaxLengthIsGreaterThanMinLengthBothFieldsAreSetToTheSameValue()
        {
            var minLength = 3;
            var maxLength = 1;
            var attribute = new StringArrayLengthAttribute("Field", maxLength, minLength);

            Assert.Equal(attribute.MaxLength, minLength);
        }

        [Fact]
        public void IfMinLengthIsNegativeItIsSetToZero()
        {
            var minLength = -3;
            var attribute = new StringArrayLengthAttribute("Field", 4, minLength);

            Assert.Equal(0, attribute.MinLength);
        }

        [Theory]
        [InlineData(new object[] { new string[] { "Hello, World" } })]
        [InlineData(new object[] { new string[] { "Test string", "Other String" } })]
        [InlineData(new object[] { new string[] { "First", "Second", "Third" } })]
        [InlineData(new object[] { new string[] { "", "", "" } })]

        public void ValidationPassesWhenArrayLengthIsWithinBoundaries(ICollection<string> array)
        {
            var attribute = new StringArrayLengthAttribute("Field", 3, 1);
            var isValid = attribute.IsValid(array);

            Assert.True(isValid);
        }
    }
}
