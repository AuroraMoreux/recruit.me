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

        // Notifications
        public const string UserSuccessfullyCreated = "User created a new account with password.";

        public const string UserSuccessfullyAddedToRole = "User has successfully been added to a user role";

        public const string ProfileSuccessfullyCreated = "Your profile has been successfully created";

        public const string ProfileSuccessfullyUpdated = "Your profile has been successfully updated";
    }
}
