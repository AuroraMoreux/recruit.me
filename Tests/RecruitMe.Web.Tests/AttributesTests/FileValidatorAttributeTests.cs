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
            IFormFile file = null;
            var attribute = new FileValidatiorAttribute(true);

            var isValid = attribute.IsValid(file);

            Assert.True(isValid);
        }

        [Fact]
        public void NullFileValidationFailsWhenNullFlagIsNotRaised()
        {
            IFormFile file = null;
            var attribute = new FileValidatiorAttribute(false);

            var isValid = attribute.IsValid(file);

            Assert.False(isValid);
        }

        [Fact]
        public void ValidationFailsWhenFileNameLongerThanFiftyCharacters()
        {
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 50, "fileWithNameLongerThanFiftyCharactersShouldNotPassValidation", "fileWithNameLongerThanFiftyCharactersShouldNotPassValidation.doc");
            var attribute = new FileValidatiorAttribute(false);

            var isValid = attribute.IsValid(file);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("FileName")]
        [InlineData("FileNameIsExactlyFiftyCharactersLongAndIsValid")]
        public void ValidationPassesWhenFileNameNotLongerThanFiftyCharacters(string fileName)
        {
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 50, fileName, fileName + ".doc");
            var attribute = new FileValidatiorAttribute(false);

            var isValid = attribute.IsValid(file);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidationFailsWhenFileSizeExceeds10MB()
        {
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, (10 * 1024 * 1024) + 1, "FileName", "FileName.doc");
            var attribute = new FileValidatiorAttribute(false);

            var isValid = attribute.IsValid(file);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData(10 * 1024 * 1024)]
        [InlineData(2 * 1024 * 1024)]
        public void ValidationPassesWhenFileSizeIsNotLargerThan10MB(long fileSize)
        {
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, fileSize, "FileName", "FileName.doc");
            var attribute = new FileValidatiorAttribute(false);

            var isValid = attribute.IsValid(file);

            Assert.True(isValid);
        }
    }
}
