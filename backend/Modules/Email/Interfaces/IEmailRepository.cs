using backend.Entities;

namespace backend.Modules.Email.Interfaces;

public interface IEmailRepository
{
    Task<EmailTemplate?> GetTemplateAsync(Guid organizationId, Guid templateId);
    Task<IEnumerable<EmailTemplate>> GetTemplatesAsync(Guid organizationId);
    Task<EmailTemplate> AddTemplateAsync(EmailTemplate template);
    Task UpdateTemplateAsync(EmailTemplate template);
    Task DeleteTemplateAsync(EmailTemplate template);
}
