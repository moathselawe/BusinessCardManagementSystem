using System.Net.Mail;

namespace HireMind.Application.Services;

public class NotificationService : INotificationService
{
    private readonly SmtpClient _smtpClient;

    public NotificationService(SmtpClient smtpClient)
    {
        _smtpClient = smtpClient;
    }
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var mail = new MailMessage("moathselawe12@gmail.com", to, subject, body);
        mail.IsBodyHtml = true; 
        await _smtpClient.SendMailAsync(mail);
    }

}
