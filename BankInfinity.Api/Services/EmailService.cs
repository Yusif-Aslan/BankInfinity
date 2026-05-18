using BankInfinity.Api.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BankInfinity.Api.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings");

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("BankInfinity API", "mamedovaslan562@gmail.com"));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = body };
        email.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        
        try
        {
            await client.ConnectAsync(smtpSettings["Server"], int.Parse(smtpSettings["Port"]!), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpSettings["Username"], smtpSettings["Password"]); 

            await client.SendAsync(email);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}