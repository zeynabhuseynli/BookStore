using BookStore.Domain.Entities.Users;

namespace BookStore.Application.Interfaces.IManagers;
public interface IEmailManager
{
    Task SendOtpAsync(string email, string otpCode);
    Task SendEmailAsync(string toEmail, string subject, string messageBody);
    Task SendEmailForSubscribers(IEnumerable<User> subscribers, string subject, string title, string description);
    Task SendPdfAsync(string toEmail, string subject, string messageBody, string pdfFilePath);
}

