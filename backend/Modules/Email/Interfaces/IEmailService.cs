using backend.Entities;

namespace backend.Modules.Email.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(Guid organizationId, string to, string subject, string htmlContent);
    Task SendTemplateEmailAsync(Guid organizationId, string to, Guid templateId, Dictionary<string, string> variables);
}
