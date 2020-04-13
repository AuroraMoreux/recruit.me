namespace RecruitMe.Common
{
    public static class GlobalConstants
    {
        // Variables
        public const string SystemName = "Recruit.Me";

        public const string AdministratorRoleName = "Administrator";

        public const string CandidateRoleName = "Candidate";

        public const string EmployerRoleName = "Employer";

        // Validation Errors
        public const string UserAlreadyExists = "The provided email address is already registered";

        public const string UicCannotBeNull = "The UIC value cannot be null";

        public const string UicMustBeNineOrThirteenDigits = "The UIC must contain 9 or 13 digits";

        public const string InvalidUic = "The provided UIC is invalid";

        public const string DocumentFileCannotBeNull = "Please provide a file to upload";

        public const string DocumentFileExtensionNotSupported = "The file extension is not supported";

        public const string DocumentFileSizeTooLarge = "File size exceeds the maximum allowed limit of 10MB";

        public const string DocumentFileNameTooLong = "File size name exceeds the maximum length of 50 characters";

        public const string DocumentAlreadyExists = "A document with the same name already exists for your profile";

        public const string AtLeastOneDocumentRequired = "At least one document must be attached to the application";

        public const string CandidateAlreadyApplied = "You have already submitted an application for this offer";

        // Notifications
        public const string UserSuccessfullyCreated = "User created a new account with password.";

        public const string UserSuccessfullyAddedToRole = "User has successfully been added to a user role";

        public const string ProfileSuccessfullyCreated = "Your profile has been successfully created";

        public const string ProfileSuccessfullyUpdated = "Your profile has been successfully updated";

        public const string JobApplicationSuccessfullySubmitted = "Your application was submitted successfully";
    }
}
