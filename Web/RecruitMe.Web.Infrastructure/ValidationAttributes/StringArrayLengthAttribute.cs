namespace RecruitMe.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RecruitMe.Common;

    public class StringArrayLengthAttribute : ValidationAttribute
    {
        public StringArrayLengthAttribute(string fieldName, int maxLength, int minLength = 0)
        {
            if (minLength < 0)
            {
                minLength = 0;
            }

            if (maxLength < minLength)
            {
                maxLength = minLength;
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
            var selection = value as ICollection<string>;

            if (selection == null)
            {
                return new ValidationResult(GlobalConstants.SelectionListCannotBeNull);
            }

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
