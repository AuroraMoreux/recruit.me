namespace RecruitMe.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    using RecruitMe.Common;

    public class UicValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(GlobalConstants.UicCannotBeNull);
            }

            string valueAsString = value.ToString();

            if (!Regex.IsMatch(valueAsString, "[0-9]{9}")
                && !Regex.IsMatch(valueAsString, "[0-9]{13}"))
            {
                return new ValidationResult(GlobalConstants.UicMustBeNineOrThirteenDigits);
            }

            var ninthDigit = CalculateNinthDigit(valueAsString);

            if (valueAsString[8] != ninthDigit)
            {
                return new ValidationResult(GlobalConstants.InvalidUic);
            }

            if (valueAsString.Length > 9)
            {
                var thirteenthDigit = CalculateThirteenthDigit(valueAsString);

                if (valueAsString[12] != thirteenthDigit)
                {
                    return new ValidationResult(GlobalConstants.InvalidUic);
                }
            }

            return ValidationResult.Success;
        }

        private static char CalculateThirteenthDigit(string valueAsString)
        {
            int currentSum = 0;
            var initialCoefficients = new int[] { 2, 7, 3, 5 };

            for (int i = 0; i < 4; i++)
            {
                currentSum += (valueAsString[i + 8] - '0') * initialCoefficients[i];
            }

            var remainder = currentSum % 11;

            if (remainder != 10)
            {
                return Convert.ToChar(remainder + '0');
            }
            else
            {
                currentSum = 0;
                var secondaryCoefficients = new int[] { 4, 9, 5, 7 };

                for (int i = 0; i < 4; i++)
                {
                    currentSum += (valueAsString[i + 8] - '0') * secondaryCoefficients[i];
                }

                remainder = currentSum % 11;

                if (remainder != 10)
                {
                    return Convert.ToChar(remainder + '0');
                }
                else
                {
                    return '0';
                }
            }
        }

        private static char CalculateNinthDigit(string valueAsString)
        {
            int currentSum = 0;
            var initialCoefficients = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            for (int i = 0; i < 8; i++)
            {
                currentSum += (valueAsString[i] - '0') * initialCoefficients[i];
            }

            var remainder = currentSum % 11;

            if (remainder != 10)
            {
                return Convert.ToChar(remainder + '0');
            }
            else
            {
                currentSum = 0;
                var secondaryCoefficients = new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };

                for (int i = 0; i < 8; i++)
                {
                    currentSum += (valueAsString[i] - '0') * secondaryCoefficients[i];
                }

                remainder = currentSum % 11;
                if (remainder != 10)
                {
                    return Convert.ToChar(remainder + '0');
                }
                else
                {
                    return '0';
                }
            }
        }
    }
}
