using backend.Entities;

namespace backend.Modules.WhatsApp.Interfaces;

public interface IWhatsAppRepository
{
    Task<WhatsAppSettings?> GetSettingsAsync(Guid organizationId);
    Task<WhatsAppSettings> SaveSettingsAsync(WhatsAppSettings settings);
    
    Task<IEnumerable<WhatsAppTemplate>> GetTemplatesAsync(Guid organizationId);
    Task<WhatsAppTemplate> SaveTemplateAsync(WhatsAppTemplate template);
    Task DeleteTemplatesAsync(Guid organizationId);
}
