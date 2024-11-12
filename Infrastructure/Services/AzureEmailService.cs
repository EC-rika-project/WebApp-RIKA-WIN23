using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Azure.Communication.Email;
using Azure;

namespace Infrastructure.Services;

public class AzureEmailService(IConfiguration configuration) : IEmailService
{
    private readonly string _connectionString = configuration["ConnectionStrings:AzureCommunicationServices"] ??
            throw new ArgumentNullException("Azure Communication Services connection string is missing");
    private readonly string _senderAddress = configuration.GetValue<string>("SenderAddress") ??
            throw new ArgumentNullException("Azure Communication Services sender address is missing");
    public async Task SendPasswordResetEmailAsync(string recipientEmail, string resetLink)
    {
        try
        {
            var emailClient = new EmailClient(_connectionString);
            var emailMessage = new EmailMessage(
                senderAddress: _senderAddress,
                content: new EmailContent("Reset your password - RikaApp")
                {
                    PlainText = "Reset your password for RikaApp",
                    Html = $@"
                    <html>
                        <body>
                            <h2>Password Reset Request</h2>
                            <p>We received a request to reset your password. If you didn't make this request, you can ignore this email.</p>
                            <p>To reset your password, click the link below:</p>
                            <p><a href='{resetLink}'>Reset Password</a></p>
                            <p>This link will expire in 1 hour for security reasons.</p>
                            <p>Best regards,<br>RikaApp Team</p>
                        </body>
                    </html>"
                },
                recipients: new EmailRecipients([new(recipientEmail)]));
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                WaitUntil.Completed,
                emailMessage);
        }
        catch (Exception ex)
        {
            throw new EmailServiceException("Failed to send password reset email", ex);
        }
    }
}

public class EmailServiceException(string message, Exception innerException) : Exception(message, innerException)
{
}
