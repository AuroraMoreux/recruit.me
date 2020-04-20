namespace RecruitMe.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using RecruitMe.Common;

    public class FileValidatiorAttribute : ValidationAttribute
    {
        public FileValidatiorAttribute(bool fileCanBeNull)
        {
            this.FileCanBeNull = fileCanBeNull;
        }

        public bool FileCanBeNull { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && this.FileCanBeNull)
            {
                return ValidationResult.Success;
            }
            else
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
}
