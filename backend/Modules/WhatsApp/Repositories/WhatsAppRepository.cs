using backend.Entities;
using backend.Modules.WhatsApp.Interfaces;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.WhatsApp.Repositories;

public class WhatsAppRepository : IWhatsAppRepository
{
    private readonly ApplicationDbContext _context;

    public WhatsAppRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WhatsAppSettings?> GetSettingsAsync(Guid organizationId)
    {
        return await _context.WhatsAppSettings
            .FirstOrDefaultAsync(s => s.OrganizationId == organizationId && !s.IsDeleted);
    }

    public async Task<WhatsAppSettings> SaveSettingsAsync(WhatsAppSettings settings)
    {
        var existing = await GetSettingsAsync(settings.OrganizationId);
        if (existing == null)
        {
            _context.WhatsAppSettings.Add(settings);
        }
        else
        {
            existing.PhoneNumberId = settings.PhoneNumberId;
            existing.BusinessAccountId = settings.BusinessAccountId;
            existing.AccessToken = settings.AccessToken;
            _context.WhatsAppSettings.Update(existing);
        }
        await _context.SaveChangesAsync();
        return settings;
    }

    public async Task<IEnumerable<WhatsAppTemplate>> GetTemplatesAsync(Guid organizationId)
    {
        return await _context.WhatsAppTemplates
            .Where(t => t.OrganizationId == organizationId && !t.IsDeleted)
            .ToListAsync();
    }

    public async Task<WhatsAppTemplate> SaveTemplateAsync(WhatsAppTemplate template)
    {
        _context.WhatsAppTemplates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task DeleteTemplatesAsync(Guid organizationId)
    {
        var templates = await _context.WhatsAppTemplates
            .Where(t => t.OrganizationId == organizationId)
            .ToListAsync();
            
        _context.WhatsAppTemplates.RemoveRange(templates);
        await _context.SaveChangesAsync();
    }
}
