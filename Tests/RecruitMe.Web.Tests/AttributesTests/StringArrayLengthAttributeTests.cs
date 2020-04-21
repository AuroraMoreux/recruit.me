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
            List<string> array = new List<string>();
            StringArrayLengthAttribute attribute = new StringArrayLengthAttribute("Field", 3);

            bool isValid = attribute.IsValid(array);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidationFailsWhenMinLengthIsNotSetAndArrayIsNull()
        {
            List<string> array = null;
            StringArrayLengthAttribute attribute = new StringArrayLengthAttribute("Field", 3);

            bool isValid = attribute.IsValid(array);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new object[] { new string[0] })]
        [InlineData(new object[] { new string[] { "Hello, World", "TestString", "", null } })]
        [InlineData(new object[] { new string[] { "OneWord", "TwoWord", "Hello, World", "TestString", "" } })]
        public void ValidationFailsWhenArrayLengthIsNotBetweenOneAndThree(ICollection<string> array)
        {
            StringArrayLengthAttribute attribute = new StringArrayLengthAttribute("Field", 3, 1);
            bool isValid = attribute.IsValid(array);

            Assert.False(isValid);
        }

        [Fact]
        public void IfMaxLengthIsGreaterThanMinLengthBothFieldsAreSetToTheSameValue()
        {
            int minLength = 3;
            int maxLength = 1;
            StringArrayLengthAttribute attribute = new StringArrayLengthAttribute("Field", maxLength, minLength);

            Assert.Equal(attribute.MaxLength, minLength);
        }

        [Fact]
        public void IfMinLengthIsNegativeItIsSetToZero()
        {
            int minLength = -3;
            StringArrayLengthAttribute attribute = new StringArrayLengthAttribute("Field", 4, minLength);

            Assert.Equal(0, attribute.MinLength);
        }

        [Theory]
        [InlineData(new object[] { new string[] { "Hello, World" } })]
        [InlineData(new object[] { new string[] { "Test string", "Other String" } })]
        [InlineData(new object[] { new string[] { "First", "Second", "Third" } })]
        [InlineData(new object[] { new string[] { "", "", "" } })]

        public void ValidationPassesWhenArrayLengthIsWithinBoundaries(ICollection<string> array)
        {
            StringArrayLengthAttribute attribute = new StringArrayLengthAttribute("Field", 3, 1);
            bool isValid = attribute.IsValid(array);

            Assert.True(isValid);
        }
    }
}
