
namespace BookStore.Infrastructure.BaseMessages;
public static class UIMessage
{
    public const string ADD_MESSAGE = "Əlavə Edildi";
    public const string UPDATE_MESSAGE = "Məlumat Yeniləndi";
    public const string DELETED_MESSAGE = "Məlumat Silindi";
    public const string NOT_DELETED_MESSAGE = "Məlumat Silinmisdir ve ya ID Duzgun Daxil Edilmeyib ";
    public const string DELETED_MESSAGES = "Məlumat Birdəfəlik Silindi";
    public const string HARD_DELETED_MESSAGE = "Məlumat Birdəfəlik Silindi";
    public static string GetUniqueNamedMessage(string propName)
    {
        return $"An {propName} with this name already exists.!";
    }
    public static string GetFormatMessage(string propName)
    {
        return $"{propName} formati duzgun deyil.!";
    }
    public static string GetRequiredMessage(string propName)
    {
        return $"{propName} boş ola bilməz!";
    }

    public static string GetNotFoundMessage(string propName)
    {
        return $"{propName} tapilmadı!";
    }

    public static string GetMinLengthMessage(int length, string propName)
    {
        return $"{propName} {length} simvoldan aşağı ola bilməz!";
    }

    public static string GetMaxLengthMessage(int length, string propName)
    {
        return $"{propName} {length} simvoldan yuxarı ola bilməz!";
    }
}

