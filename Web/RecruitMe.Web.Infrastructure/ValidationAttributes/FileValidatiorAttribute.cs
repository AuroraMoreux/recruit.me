namespace RecruitMe.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Microsoft.AspNetCore.Http;
    using RecruitMe.Common;

    public class FileValidatiorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(GlobalConstants.DocumentFileCannotBeNull);
            }

            IFormFile file = value as IFormFile;

            if (file.FileName.Length > 50)
            {
                return new ValidationResult(GlobalConstants.DocumentFileNameTooLong);
            }

            if (file.Length > 10 * 1024 * 1024)
            {
                return new ValidationResult(GlobalConstants.DocumentFileSizeTooLarge);
            }

            return ValidationResult.Success;
         }
    }
}
