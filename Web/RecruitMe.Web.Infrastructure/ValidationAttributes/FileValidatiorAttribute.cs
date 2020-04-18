﻿namespace RecruitMe.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using RecruitMe.Common;

    public class FileValidatiorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(GlobalConstants.FileCannotBeNull);
            }

            IFormFile file = value as IFormFile;

            if (file.FileName.Length > 50)
            {
                return new ValidationResult(GlobalConstants.FileNameTooLong);
            }

            if (file.Length > 10 * 1024 * 1024)
            {
                return new ValidationResult(GlobalConstants.FileSizeTooLarge);
            }

            return ValidationResult.Success;
         }
    }
}
