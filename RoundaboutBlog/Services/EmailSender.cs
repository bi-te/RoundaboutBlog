using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RoundaboutBlog.Settings;

namespace RoundaboutBlog.Services;

public class EmailSender: IEmailSender
{
    private readonly SmtpSettings _smtpSettings;

    public EmailSender(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }
    
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpSettings.Company, _smtpSettings.User));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, _smtpSettings.Ssl);
        
        await client.AuthenticateAsync(_smtpSettings.User, _smtpSettings.Password);
        await client.SendAsync(message);
        
        await client.DisconnectAsync(true);
    }
    
    
}