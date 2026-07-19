using backend.Modules.Email.Interfaces;
using backend.Entities;

namespace backend.Modules.Email.Services;

public class EmailService : IEmailService
{
    private readonly IEmailRepository _emailRepository;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IEmailRepository emailRepository, ILogger<EmailService> logger)
    {
        _emailRepository = emailRepository;
        _logger = logger;
    }

    public async Task SendEmailAsync(Guid organizationId, string to, string subject, string htmlContent)
    {
        // Mock email sending. In a real scenario, integrate with SMTP, SendGrid, Amazon SES, etc.
        _logger.LogInformation($"Sending email to {to} with subject: {subject}");
        await Task.CompletedTask;
    }

    public async Task SendTemplateEmailAsync(Guid organizationId, string to, Guid templateId, Dictionary<string, string> variables)
    {
        var template = await _emailRepository.GetTemplateAsync(organizationId, templateId);
        if (template == null) throw new KeyNotFoundException("Email template not found.");

        string finalHtml = template.HtmlContent;
        foreach (var variable in variables)
        {
            finalHtml = finalHtml.Replace($"{{{{{variable.Key}}}}}", variable.Value);
        }

        await SendEmailAsync(organizationId, to, template.Subject, finalHtml);
    }
}
