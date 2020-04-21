namespace RecruitMe.Web.Tests.AttributesTests
{
    using System.IO;
    using System.Text;

    using Microsoft.AspNetCore.Http;
    using RecruitMe.Web.Infrastructure.ValidationAttributes;
    using Xunit;

    public class FileValidatorAttributeTests
    {
        [Fact]
        public void NullFileValidationPassesWhenNullFlagIsRaised()
        {
            // Arrange
            IFormFile file = null;
            FileValidatiorAttribute attribute = new FileValidatiorAttribute(true);

            // Act
            bool isValid = attribute.IsValid(file);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void NullFileValidationFailsWhenNullFlagIsNotRaised()
        {
            // Arrange
            IFormFile file = null;
            FileValidatiorAttribute attribute = new FileValidatiorAttribute(false);

            // Act
            bool isValid = attribute.IsValid(file);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidationFailsWhenFileNameLongerThanFiftyCharacters()
        {
            // Arrange
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 50, "fileWithNameLongerThanFiftyCharactersShouldNotPassValidation", "fileWithNameLongerThanFiftyCharactersShouldNotPassValidation.doc");

            FileValidatiorAttribute attribute = new FileValidatiorAttribute(false);

            // Act
            bool isValid = attribute.IsValid(file);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("FileName")]
        [InlineData("FileNameIsExactlyFiftyCharactersLongAndIsValid")]
        public void ValidationPassesWhenFileNameNotLongerThanFiftyCharacters(string fileName)
        {
            // Arrange
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 50, fileName, fileName + ".doc");

            FileValidatiorAttribute attribute = new FileValidatiorAttribute(false);

            // Act
            bool isValid = attribute.IsValid(file);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidationFailsWhenFileSizeExceeds10MB()
        {
            // Arrange
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, (10 * 1024 * 1024) + 1, "FileName", "FileName.doc");

            FileValidatiorAttribute attribute = new FileValidatiorAttribute(false);

            // Act
            bool isValid = attribute.IsValid(file);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(10 * 1024 * 1024)]
        [InlineData(2 * 1024 * 1024)]
        public void ValidationPassesWhenFileSizeIsNotLargerThan10MB(long fileSize)
        {
            // Arrange
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, fileSize, "FileName", "FileName.doc");

            FileValidatiorAttribute attribute = new FileValidatiorAttribute(false);

            // Act
            bool isValid = attribute.IsValid(file);

            // Assert
            Assert.True(isValid);
        }
    }
}
