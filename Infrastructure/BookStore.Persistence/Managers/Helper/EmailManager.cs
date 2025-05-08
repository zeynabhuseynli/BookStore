using System.Net;
using System.Net.Mail;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Domain.Entities.Users;
using Microsoft.Extensions.Configuration;

namespace BookStore.Persistence.Managers;
public class EmailManager:IEmailManager
{
    private readonly IConfiguration _configuration;

    public EmailManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendOtpAsync(string toEmail, string otpCode)
    {
        var smtpSettings = _configuration.GetSection("Smtp");

        var smtpClient = new SmtpClient(smtpSettings["Host"])
        {
            Port = int.Parse(smtpSettings["Port"]),
            Credentials = new NetworkCredential(smtpSettings["UserName"], smtpSettings["Password"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["UserName"]),
            Subject = "Your OTP Code",
            Body = $"Your OTP code is: {otpCode}",
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string messageBody)
    {
        var smtpSettings = _configuration.GetSection("Smtp");

        var smtpClient = new SmtpClient(smtpSettings["Host"])
        {
            Port = int.Parse(smtpSettings["Port"]),
            Credentials = new NetworkCredential(smtpSettings["UserName"], smtpSettings["Password"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["UserName"]),
            Subject = subject,
            Body = messageBody,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendEmailForSubscribers(IEnumerable<User> subscribers, string subject, string title, string description)
    {
        if (subscribers != null && subscribers.Count() > 0)
        {
            foreach (var item in subscribers)
            {
                await SendEmailAsync(
                    item.Email,
                    $"{subject}",
                    $"<html><body><h1>{title}</h1><p>{description}</p></body></html>"
                );
            }
        }
    }

    public async Task SendPdfAsync(string toEmail, string subject, string messageBody, string pdfFilePath)
    {
        var smtpSettings = _configuration.GetSection("Smtp");

        var smtpClient = new SmtpClient(smtpSettings["Host"])
        {
            Port = int.Parse(smtpSettings["Port"]),
            Credentials = new NetworkCredential(smtpSettings["UserName"], smtpSettings["Password"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["UserName"]),
            Subject = subject,
            Body = messageBody,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        if (!File.Exists(pdfFilePath))
            throw new FileNotFoundException("PDF faylı tapılmadı.", pdfFilePath);

        mailMessage.Attachments.Add(new Attachment(pdfFilePath, "application/pdf"));

        await smtpClient.SendMailAsync(mailMessage);
    }
}

