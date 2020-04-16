namespace RecruitMe.Web.Infrastructure.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Common;

    public class ArrayLengthAttribute : ValidationAttribute
    {
        public ArrayLengthAttribute(string fieldName, int maxLength, int minLength = 1)
        {
            if (minLength < 1)
            {
                minLength = 1;
            }

            this.FieldName = fieldName;
            this.MinLength = minLength;
            this.MaxLength = maxLength;
        }

        public string FieldName { get; set; }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(GlobalConstants.SelectionListCannotBeNull);
            }

            List<int> selection = value as List<int>;

            if (selection.Count < this.MinLength)
            {
                return new ValidationResult(string.Format(GlobalConstants.SelectionShouldNotBeLessThanMinLength, this.MinLength, this.FieldName));
            }
            else if (selection.Count > this.MaxLength)
            {
                return new ValidationResult(string.Format(GlobalConstants.SelectionShouldNotExceedMaxLength, this.MaxLength, this.FieldName));
            }

            return ValidationResult.Success;
        }
    }
}
