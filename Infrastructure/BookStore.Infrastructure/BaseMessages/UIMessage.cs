
namespace BookStore.Infrastructure.BaseMessages;
public static class UIMessage
{
    public const string INVALID_MESSAGE = "Invalid email or password";
    public const string BIRTHDAY_MESSAGE = "The date of birth cannot be in the future.";
    public const string ADD_MESSAGE = "Added";
    public const string UPDATE_MESSAGE = "Information Updated";
    public const string DELETED_MESSAGE = "Data Deleted";
    public const string NOT_DELETED_MESSAGE = "Data not deleted or ID entered incorrectly";
    public const string HARD_DELETED_MESSAGE = "Data Permanently Deleted";
    public static string GetUniqueNamedMessage(string propName)
    {
        return $"An {propName} with this name already exists.!";
    }
    public static string GetFormatMessage(string propName)
    {
        return $"{propName} is not in the correct format!";
    }
    public static string GetRequiredMessage(string propName)
    {
        return $"{propName} cannot be empty!";
    }

    public static string GetNotFoundMessage(string propName)
    {
        return $"{propName} is not found!";
    }

    public static string GetMinLengthMessage(int length, string propName)
    {
        return $"{propName} cannot be less than {length} characters!";
    }

    public static string GetMaxLengthMessage(int length, string propName)
    {
        return $"{propName} cannot be longer than {length} characters!";
    }
}

