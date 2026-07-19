using backend.Entities;

namespace backend.Interfaces;

public interface IWhatsAppService
{
    Task<WhatsAppSettings?> GetSettingsAsync(Guid organizationId);
    Task<WhatsAppSettings> SaveSettingsAsync(WhatsAppSettings settings);
    Task<bool> SendMessageAsync(Guid organizationId, string to, string templateName, string languageCode);
    Task<List<WhatsAppTemplate>> SyncTemplatesAsync(Guid organizationId);
}
