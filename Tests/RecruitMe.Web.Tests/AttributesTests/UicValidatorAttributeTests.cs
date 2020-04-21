namespace RecruitMe.Web.Tests.AttributesTests
{
    using RecruitMe.Web.ValidationAttributes;
    using Xunit;

    public class UicValidatorAttributeTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("12345678")]
        [InlineData("1234567890")]
        [InlineData("123456789012")]
        [InlineData("12345678901234")]
        [InlineData("")]
        [InlineData("abcdefghi")]
        [InlineData("abcdefghijklm")]
        [InlineData("\0")]
        [InlineData("           ")]
        [InlineData("123456799")]
        [InlineData("1122334455667")]
        [InlineData("0009039460578")]
        public void UicShouldBeInvalid(string uic)
        {
            UicValidatorAttribute attribute = new UicValidatorAttribute();
            bool result = attribute.IsValid(uic);

            Assert.False(result);
        }

        [Theory]
        [InlineData("204558718")]
        [InlineData("204952162")]
        [InlineData("000903946")]
        [InlineData("160026043")]
        [InlineData("0009039460577")]
        [InlineData("1314720070163")]
        [InlineData("1314720070041")]
        [InlineData("1314720070060")]
        [InlineData("1314720070178")]
        [InlineData("1314720070037")]
        [InlineData("1314720070110")]
        [InlineData("1314720070125")]
        [InlineData("1314720070075")]
        [InlineData("1314720070087")]
        [InlineData("1314720070130")]
        [InlineData("1314720070022")]
        [InlineData("1314720070106")]
        [InlineData("1314720070056")]
        [InlineData("1314720070018")]
        [InlineData("1314720070144")]
        [InlineData("1314720070094")]
        [InlineData("1314720070182")]
        [InlineData("1314720070159")]
        public void UicShouldBeValid(string uic)
        {
            UicValidatorAttribute attribute = new UicValidatorAttribute();
            bool result = attribute.IsValid(uic);

            Assert.True(result);
        }
    }
}
