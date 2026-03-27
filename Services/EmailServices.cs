using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MyCustomUmbracoProject.Models; 


namespace MyCustomUmbracoProject.Services.Interfaces;

public interface IEmailServices
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}

public class EmailService(IOptions<SmtpSettings> newSmtpSettings) : IEmailServices //IOptions is a wrapper around SmtpSettings, so we need to extract the value from it in the constructor
{

    private readonly SmtpSettings _smtp = newSmtpSettings.Value; //must extract the value from IOptions
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtp.FromName, _smtp.FromEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();

        // For local server with no SSL
        await client.ConnectAsync(_smtp.Host, _smtp.Port, SecureSocketOptions.None);

        // Only authenticate if credentials are provided
        if (!string.IsNullOrEmpty(_smtp.Username))
            await client.AuthenticateAsync(_smtp.Username, _smtp.Password);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
