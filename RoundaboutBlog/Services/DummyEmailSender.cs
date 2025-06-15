using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RoundaboutBlog.Settings;

namespace RoundaboutBlog.Services;

public class DummyEmailSender : IEmailSender
{
  public DummyEmailSender(IOptions<SmtpSettings> smtpSettings)
  {
  }

  public async Task SendEmailAsync(string email, string subject, string htmlMessage)
  {
  }


}