namespace RoundaboutBlog.Settings;

public class SmtpSettings
{
  public required string Host { get; set; }
  public required int Port { get; set; }

  public required string User { get; set; }
  public required string Password { get; set; }
  public bool Ssl { get; set; }

  public string? Company { get; set; }
}