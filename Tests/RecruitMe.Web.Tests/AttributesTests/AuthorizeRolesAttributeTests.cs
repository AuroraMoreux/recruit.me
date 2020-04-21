namespace RecruitMe.Web.Tests.AttributesTests
{
    using RecruitMe.Web.Infrastructure.Attributes;
    using Xunit;

    public class AuthorizeRolesAttributeTests
    {
        [Theory]
        [InlineData("FirstRole", "SecondRole")]
        [InlineData("FirstRole", "")]
        [InlineData("FirstRole", null)]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void MultipleRolesAreCorrectlyConcatenated(params string[] roles)
        {
            // Arrange
            string expectedRoles = string.Join(",", roles);

            // Act
            AuthorizeRolesAttribute attribute = new AuthorizeRolesAttribute(roles);

            // Assert
            Assert.Equal(expectedRoles, attribute.Roles);
        }
    }
}
