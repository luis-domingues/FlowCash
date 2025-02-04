using SendGrid;
using SendGrid.Helpers.Mail;

namespace FlowCash.Serivces;

public class EmailServices
{
    private readonly string _sendGridApiKey;

    public EmailServices(IConfiguration configuration)
    {
        _sendGridApiKey = configuration["SendGrid:ApiKey"];
    }

    public async Task<bool> SendPasswordResetEmailAsync(string email, string resetToken)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var from = new EmailAddress("lpds550@gmail.com", "FlowCash");
        var subject = "Recuperação de senha - FlowCash";
        var to = new EmailAddress(email);
        var plainTextContext = $"Token para redefinição de senha: {resetToken}";
        var htmlContent = $"<p>Token para redefinição de senha: <strong>{resetToken}</strong></p>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContext, htmlContent);
        var response = await client.SendEmailAsync(msg);
        return response.StatusCode == System.Net.HttpStatusCode.Accepted;
    }
}