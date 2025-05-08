namespace BookStore.Infrastructure.BaseMessages;
public static class UIMessage
{
    public const string ADD_MESSAGE = "The item has been successfully added.";
    public const string UPDATE_MESSAGE = "The information was successfully updated.";
    public const string DELETED_MESSAGE = "The item has been successfully deleted.";
    public const string HARD_DELETED_MESSAGE = "The item has been permanently removed.";
    public const string NOT_DELETED_MESSAGE = "Deletion failed or the provided ID is invalid.";
    public const string FILE_MANAGER_MESSAGE = "The file is too large. Maximum allowed is 100 MB.";
    public const string INVALID_MESSAGE = "Incorrect email or password.";
    public const string BIRTHDAY_MESSAGE = "Date of birth cannot be in the future.";
    public const string CATEGORY_HAS_SUBCATEGORIES = "This category cannot be deleted because it has subcategories.";
    public const string CATEGORY_LINKED_TO_BOOKS = "This category cannot be deleted because it is associated with one or more books.";
    public const string INVALID_CLAIM_PARSING = "Failed to parse user claim: NameIdentifier is not a valid integer.";
    public const string USER_NOT_AUTHENTICATED = "The current user is not authenticated.";
    public const string USER_MISSING_REQUIRED_CLAIM = "The current user does not have the required permissions or claims to perform this action.";

    public const string ACCOUNT_LOCKED_AFTER_FAILED_ATTEMPTS =
    "You have entered an incorrect password 5 times. Your account has been temporarily deactivated. Please contact an administrator for assistance.";

    public const string OTP_INVALID_OR_EXPIRED = "The OTP code is either incorrect or has expired.";
    public const string PASSWORD_SAME_AS_OLD = "The new password cannot be the same as your previous password.";
    public const string BOOK_PDF_SENT = "The book's PDF file has been sent to your email.";
    public const string EMAIL_OR_PASSWORD_INVALID = "Email or password is incorrect.";

    public const string NO_PERMISSION = "You do not have permission to perform this operation.";

    public static string GetFileTooLargeMessage(int maxMb)
    => $"The file size exceeds the maximum allowed limit of {maxMb} MB.";

    public static string FILE_PATH_NULL_OR_EMPTY(string relativePath)
   => $"The specified file path is invalid: '{relativePath}'";

    public static string FILE_PATH_INVALID(string relativePath)
       => $"Invalid characters detected in the file path: '{relativePath}'";

    public static string GetUniqueNamedMessage(string propName)
        => $"An item with the same {propName} already exists.";

    public static string GetFormatMessage(string propName)
        => $"{propName} is not in a valid format.";

    public static string GetRequiredMessage(string propName)
        => $"{propName} is required.";

    public static string GetNotFoundMessage(string entityName)
        => $"{entityName} was not found.";

    public static string GetMinLengthMessage(int length, string propName)
        => $"{propName} must be at least {length} characters long.";

    public static string GetMaxLengthMessage(int length, string propName)
        => $"{propName} must not exceed {length} characters.";
}