using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;

namespace PoundPupLegacy.Services.Implementation;

public class EmailSender(
    ILogger<EmailSender> logger, 
    ISiteDataService siteDataService
) : IEmailSender
{

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var tenant = siteDataService.GetTenant();
        var client = new SmtpClient();
        try {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse($"no-reply@{tenant.DomainName}"));
            emailMessage.To.Add(MailboxAddress.Parse(toEmail));
            emailMessage.Subject = subject;

            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.TextBody = message;
            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            client.Connect(tenant.SmtpConnection.Host, tenant.SmtpConnection.Port, false);
            client.Authenticate(tenant.SmtpConnection.Username, tenant.SmtpConnection.Password);
            await client.SendAsync(emailMessage);

        }
        catch (Exception ex) {
            logger.LogError(ex.Message);
        }
        finally {
            client.Disconnect(true);
        }
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        await Task.CompletedTask;
    }
}