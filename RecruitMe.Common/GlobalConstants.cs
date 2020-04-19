namespace RecruitMe.Common
{
    public static class GlobalConstants
    {
        // Variables
        public const string SystemName = "RecruitMe";

        public const string AdministratorRoleName = "Administrator";

        public const string CandidateRoleName = "Candidate";

        public const string EmployerRoleName = "Employer";

        public const string IdentityRedirectPath = "/Identity/Account/Manage";

        public const int ItemsPerPage = 8;

        // Validation Errors
        public const string UserAlreadyExists = "The provided email address is already registered";

        public const string UicCannotBeNull = "The UIC value cannot be null";

        public const string UicMustBeNineOrThirteenDigits = "The UIC must contain 9 or 13 digits";

        public const string InvalidUic = "The provided UIC is invalid";

        public const string FileCannotBeNull = "Please provide a file to upload";

        public const string FileExtensionNotSupported = "The file extension is not supported";

        public const string FileSizeTooLarge = "File size exceeds the maximum allowed limit of 10MB";

        public const string FileNameTooLong = "File size name exceeds the maximum length of 50 characters";

        public const string DocumentAlreadyExists = "A file with the same name already exists for your profile";

        public const string AtLeastOneDocumentRequired = "At least one document must be attached to the application";

        public const string CandidateAlreadyApplied = "You have already submitted an application for this offer";

        public const string SelectionListCannotBeNull = "Please select at least one option from the list";

        public const string SelectionShouldNotBeLessThanMinLength = "You must select at least {0} options for {1}";

        public const string SelectionShouldNotExceedMaxLength = "You can only select up to {0} options for {1}";

        public const string ValidFromDateMustBeAfterCurrentDate = @"The ""Valid from"" date must be greater than the current date";

        public const string ValidUntilDateMustBeCreaterThanValidFromDate = @"The ""Valid until"" date must be greater than the ""Valid from"" date";

        public const string JobOfferWithSameNameAlreadyExists = "You already have a Job Offer with the same title that is currently active";

        public const string InvalidInputType = "Invalid input type";

        public const string SalaryToValueMustBeGreater = @"The ""Salary To"" amount must be greater than the ""Salary From"" amount";

        // Notifications
        public const string UserSuccessfullyCreated = "User account successfully created";

        public const string UserSuccessfullyAddedToRole = "User has been successfully added to a user role";

        public const string ProfileSuccessfullyCreated = "Your profile has been successfully created";

        public const string ProfileSuccessfullyUpdated = "Your profile has been successfully updated";

        public const string JobApplicationSuccessfullySubmitted = "Your application has been submitted successfully";
        public const string JobApplicationStatusSuccessfullyChanged = "Application status successfully changed";

        public const string JobOfferSuccessfullyPosted = "The job offer has been successfully posted";

        public const string JobOfferSuccessfullyUpdated = "The job offer has been successfully updated";

        public const string JobOfferSuccessfullyDeleted = "The job offer has been successfully deleted";

        public const string DocumentSuccessfullyUploaded = "The document has been successfully uploaded";

        public const string DocumentSuccessfullyDeleted = "The document has been successfully deleted";

        // Email Templates
        public const string NewAccountConfirmation = "Please confirm your account by<a href='{0}'> clicking here</a>.";

        public const string NewJobApplicationReceivedOpening = "Hello, {0}, <br /> A new job application has been received for the following job offer: <strong>{1}</strong><br /><br /><strong>Candidate contact details:</strong><br /><strong>Name:</strong> {2}<br /><strong>Email:</strong> {3}";

        public const string NewJobApplicationReceivedCandidatePhoneNumber = "<br /><strong>Phone Number:</strong> {0}";

        public const string NewJobApplicationReceivedClosing = "<br /> You can review the application details <a href='{0}'>here</a>";

        public const string PasswordChanged = "Your are receiving this email because your password has been successfully reset. If the password change was not requested by you, please contact us at {0} for assistance.";

        public const string EmailAddressChanged = "You are receiving this email because there has been a request to update the email address for your account. Please confirm your new email address by <a href='{0}'>clicking here</a>.";
    }
}
